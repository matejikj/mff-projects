// SPDX-License-Identifier: Apache-2.0
// Copyright 2019 Charles University

#include <proc/context.h>
#include <proc/scheduler.h>
#include <proc/thread.h>
#include <mm/heap.h>
#include <mm/as.h>
#include <mm/frame.h>
#include <debug.h>
#include <drivers/timer.h>
#include <exc.h>

/** Initialize support for threading.
 *
 * Called once at system boot.
 */
void threads_init(void) {
}

/** Main function of the thread.
 * 
 * Ends the execution properly.
 */
static void thread_main_func(thread_entry_func_t entry, void* data) {
    timer_interrupt_after(10000);
    void* result = entry(data);
    thread_finish(result);
}

/**
 * char* to char[]
*/
static void str_to_carray(char str[THREAD_NAME_MAX_LENGTH + 1], const char *it) {
    int i = 0;

    while (*it != '\0') {
        str[i] = *it;
        it++;
        i++;
    }
}

static errno_t thread_create_internal(thread_t** thread_out, thread_entry_func_t entry, void* data, unsigned int flags, const char* name, as_t* as) {
    *thread_out = kmalloc(sizeof(thread_t));
    if (*thread_out == NULL) {
        return ENOMEM;
    }

    uintptr_t stack_frame;
    errno_t stack_result = frame_alloc(1, &stack_frame);
    if (stack_result != EOK) {
        return ENOMEM;
    }

    (*thread_out)->entry_func = entry;
    (*thread_out)->data = data;
    (*thread_out)->as = as;
    as->used_by_count++;
    str_to_carray((*thread_out)->name, name);

    // Initialize the stack;
    size_t* stack = (size_t*)(stack_frame | 0x80000000);
    size_t* stack_base = stack + THREAD_STACK_SIZE;
    (*thread_out)->stack = stack;

    // Save the context. 
    context_t* context = (context_t*)(stack_base - sizeof(context_t));
    context->sp = (unative_t)stack_base;
    context->ra = (unative_t)&thread_main_func;
    context->a0 = (unative_t)entry;
    context->a1 = (unative_t)data;
    context->status = 0xFF00 | CP0_STATUS_IE_BIT; // ksu;
    // TODO: set gp.

    (*thread_out)->stack_top = (void*)context;

    scheduler_add_ready_thread(*thread_out);

    return EOK;
}

/** Create a new thread.
 *
 * The thread is automatically placed into the queue of ready threads.
 *
 * This function allocates space for both stack and the thread_t structure
 * (hence the double <code>**</code> in <code>thread_out</code>.
 *
 * This thread will use the same address space as the current one.
 *
 * @param thread_out Where to place the initialized thread_t structure.
 * @param entry Thread entry function.
 * @param data Data for the entry function.
 * @param flags Flags (unused).
 * @param name Thread name (for debugging purposes).
 * @return Error code.
 * @retval EOK Thread was created and started (added to ready queue).
 * @retval ENOMEM Not enough memory to complete the operation.
 * @retval INVAL Invalid flags (unused).
 */
errno_t thread_create(thread_t** thread_out, thread_entry_func_t entry, void* data, unsigned int flags, const char* name) {
    return thread_create_internal(thread_out, entry, data, flags, name, thread_get_current()->as);
}

/** Create a new thread with new address space.
 *
 * The thread is automatically placed into the queue of ready threads.
 *
 * This function allocates space for both stack and the thread_t structure
 * (hence the double <code>**</code> in <code>thread_out</code>.
 *
 * @param thread_out Where to place the initialized thread_t structure.
 * @param entry Thread entry function.
 * @param data Data for the entry function.
 * @param flags Flags (unused).
 * @param name Thread name (for debugging purposes).
 * @param as_size Address space size, aligned at page size (0 is correct though not very useful).
 * @return Error code.
 * @retval EOK Thread was created and started (added to ready queue).
 * @retval ENOMEM Not enough memory to complete the operation.
 * @retval INVAL Invalid flags (unused).
 */
errno_t thread_create_new_as(thread_t** thread_out, thread_entry_func_t entry, void* data, unsigned int flags, const char* name, size_t as_size) {
    return thread_create_internal(thread_out, entry, data, flags, name, as_create(as_size, flags));
}

/** Return information about currently executing thread.
 *
 * @retval NULL When no thread was started yet.
 */
thread_t* thread_get_current(void) {
    return scheduler_get_current_thread();
}

/** Yield the processor. */
void thread_yield(void) {
    scheduler_schedule_next();
}

/** Current thread stops execution and is not scheduled until woken up. */
void thread_suspend(void) {
    scheduler_schedule_next_with_action(suspend, NULL, NULL, NULL, NULL);
}

/** Terminate currently running thread.
 *
 * Thread can (normally) terminate in two ways: by returning from the entry
 * function of by calling this function. The parameter to this function then
 * has the same meaning as the return value from the entry function.
 *
 * Note that this function never returns.
 *
 * @param retval Data to return in thread_join.
 */
void thread_finish(void* retval) {
    scheduler_schedule_next_with_action(finish, NULL, NULL, retval, NULL);
    while (1) {
    }
}

/** Tells if thread already called thread_finish() or returned from the entry
 * function.
 *
 * @param thread Thread in question.
 */
bool thread_has_finished(thread_t* thread) {
    return scheduler_has_thread_finished(thread);
}

/** Wakes-up existing thread.
 *
 * Note that waking-up a running (or ready) thread has no effect (i.e. the
 * function shall not count wake-ups and suspends).
 *
 * Note that waking-up a thread does not mean that it will immediatelly start
 * executing.
 *
 * @param thread Thread to wake-up.
 * @return Error code.
 * @retval EOK Thread was woken-up (or was already ready/running).
 * @retval EINVAL Invalid thread.
 * @retval EEXITED Thread already finished its execution.
 */
errno_t thread_wakeup(thread_t* thread) {
    return scheduler_wake_thread(thread);
}

/** Joins another thread (waits for it to terminate.
 *
 * Note that <code>retval</code> could be <code>NULL</code> if the caller
 * is not interested in the returned value.
 *
 * @param thread Thread to wait for.
 * @param retval Where to place the value returned from thread_finish.
 * @return Error code.
 * @retval EOK Thread was joined.
 * @retval EBUSY Some other thread is already joining this one.
 * @retval EKILLED Thread was killed.
 * @retval EINVAL Invalid thread.
 */
errno_t thread_join(thread_t* thread, void** retval) {
    errno_t result = scheduler_schedule_next_with_action(suspend, thread, retval, NULL, NULL);

    if (is_killed(thread)) {
        return EKILLED;
    }

    return result;
}

/** Switch CPU context to a different thread.
 *
 * Note that this function must work even if there is no current thread
 * (i.e. for the very first context switch in the system).
 *
 * @param thread Thread to switch to.
 */
void thread_switch_to(thread_t* thread) {
    scheduler_switch_to(thread);    
}

/** Add thread to waiting queue
 *
 * @param list List.
 * @param thread Thread to add.
 */
void add_waiting_thread(list_t* list, thread_t* thread) {
    list_append(list, &thread->waiting_link);
}

/** Pop first item from list
 *
 * @param list List.
 */
thread_t* pop_waiting_thread(list_t* list) {
    link_t* next_thread_link = list_pop(list);
    if (next_thread_link == NULL) {
        return NULL;
    }
    return list_item(next_thread_link, thread_t, waiting_link); 
}

/** Get address space of given thread. */
as_t* thread_get_as(thread_t* thread) {
    return thread->as;
}

/** Kills given thread.
 *
 * Note that this function shall work for any existing thread, including
 * currently running one.
 *
 * Joining a killed thread results in EKILLED return value from thread_join.
 *
 * @param thread Thread to kill.
 * @return Error code.
 * @retval EOK Thread was killed.
 * @retval EINVAL Invalid thread.
 * @retval EEXITED Thread already finished its execution.
 */
errno_t thread_kill(thread_t* thread) {
    return kill_thread(thread);
}
