// SPDX-License-Identifier: Apache-2.0
// Copyright 2019 Charles University

#include <drivers/timer.h>
#include <debug.h>
#include <proc/scheduler.h>
#include <proc/context.h>
#include <adt/list.h>
#include <mm/heap.h>
#include <mm/as.h>
#include <exc.h>

typedef struct ready_thread {
    thread_t* thread;
    link_t list_link;
} ready_thread_t;

typedef struct suspended_thread {
    thread_t* thread;
    thread_t* waiting_for; // Can be NULL. 
    void** waiting_for_result; // This can be NULL when caller is not interested.
    link_t list_link;
} suspended_thread_t;

typedef struct finished_thread {
    thread_t* thread;
    bool killed;
    void* result;
    link_t list_link;
} finished_thread_t;

list_t ready_queue;
list_t suspended_threads;
list_t finished_threads;
ready_thread_t* current_thread;

static suspended_thread_t* new_suspended_thread(thread_t* thread, thread_t* waiting_for, void** waiting_for_result) {
    suspended_thread_t* st = kmalloc(sizeof(suspended_thread_t));
    panic_if(st == NULL, "Failed to allocate memory for suspended thread.");
    st->thread = thread;
    st->waiting_for = waiting_for;
    st->waiting_for_result = waiting_for_result;
    link_init(&st->list_link);
    return st;
}

static void scheduler_add_suspended_thread(thread_t* thread, thread_t* waiting_for, void** waiting_for_result) {
   suspended_thread_t* st = new_suspended_thread(thread, waiting_for, waiting_for_result);
   list_append(&suspended_threads, &st->list_link);
}

static ready_thread_t* new_ready_thread(thread_t* thread) {
    ready_thread_t* rt = kmalloc(sizeof(ready_thread_t));
    panic_if(rt == NULL, "Failed to allocate memory for ready thread.");
    rt->thread = thread;
    return rt;
}

static ready_thread_t* pop_next_thread() {
    link_t* next_thread_link = list_pop(&ready_queue);
    if (next_thread_link == NULL) {
        return NULL;
    }

    return list_item(next_thread_link, ready_thread_t, list_link); 
}

static ready_thread_t* get_ready_thread(thread_t* thread) {
    if (!list_is_empty(&ready_queue)) {
        list_foreach(ready_queue, ready_thread_t, list_link, _it) {
            if (_it->thread == thread) {
                return _it;
            }
        }
    }

    return NULL;     
}

static suspended_thread_t* get_suspended_thread(thread_t* thread) {
    if (!list_is_empty(&suspended_threads)) {
        list_foreach(suspended_threads, suspended_thread_t, list_link, _it) {
            if (_it->thread == thread) {
                return _it;
            }
        }
    }

    return NULL;     
}

static finished_thread_t* get_finished_thread(thread_t* thread) {
    if (!list_is_empty(&finished_threads)) {
        list_foreach(finished_threads, finished_thread_t, list_link, _it ) {
            if (_it->thread == thread) {
                return _it;
            }
        }
    }

    return NULL;     
}

static void finish_thread(thread_t* thread, void* result, bool killed) {
    finished_thread_t* ft = kmalloc(sizeof(finished_thread_t));
    panic_if(ft == NULL, "Failed to allocate memory for finished thread.");
    ft->thread = thread;
    ft->killed = killed;
    ft->result = result;
    list_append(&finished_threads, &ft->list_link);

    // Wake all threads waiting for this one.
    link_t* current_link = suspended_threads.head.next;
    while (current_link != (link_t*)&suspended_threads.head) {
        suspended_thread_t *st = list_item(current_link, suspended_thread_t, list_link);
        current_link = current_link->next;
        if (st->waiting_for == thread){
            scheduler_add_ready_thread(st->thread);
            list_remove(&st->list_link);

            if (st->waiting_for_result != NULL) {
                *st->waiting_for_result = result;
            }

            kfree(st);
        }
    }

    thread->as->used_by_count--;
    if (thread->as->used_by_count <= 0) {
        as_destroy(thread->as);
    }
}

bool is_killed(thread_t* thread) {
    finished_thread_t* ft = get_finished_thread(thread);
    return ft != NULL && ft->killed;
}

errno_t kill_thread(thread_t* thread) {
    if (thread == current_thread->thread) {
        return scheduler_schedule_next_with_action(kill, NULL, NULL, NULL, NULL);
    }

    ready_thread_t* rt = get_ready_thread(thread);
    if (rt != NULL) {
        list_remove(&rt->list_link);
        finish_thread(rt->thread, NULL, true);
        kfree(rt);
        return EOK;
    }

    suspended_thread_t* st = get_suspended_thread(thread);
    if (st != NULL) {
        list_remove(&st->list_link);
        finish_thread(st->thread, NULL, true);
        kfree(st);
        return EOK;
    }

    finished_thread_t* ft = get_finished_thread(thread);
    if (ft != NULL) {
        return EEXITED;
    }

    return EINVAL;
}

thread_t* scheduler_get_current_thread(void) {
    if (current_thread == NULL) {
        return NULL;
    }
    return current_thread->thread;
}

/** Initialize support for scheduling.
 *
 * Called once at system boot.
 */
void scheduler_init(void) {
    list_init(&ready_queue);
    list_init(&suspended_threads);
    list_init(&finished_threads);
}

/** Marks given thread as ready to be executed.
 *
 * It is expected that this thread would be added at the end
 * of the queue to run in round-robin fashion.
 *
 * @param thread Thread to make runnable.
 */
void scheduler_add_ready_thread(thread_t* thread) {
    bool was_disabled = interrupts_disable();
    ready_thread_t* rt = new_ready_thread(thread); 
    list_append(&ready_queue, &rt->list_link);
    interrupts_restore(was_disabled);
}

/** Removes given thread from scheduling.
 *
 * Expected to be called when thread is suspended, terminates thread
 * etc.
 *
 * @param thread Thread to remove from the queue.
 */
void scheduler_remove_thread(thread_t* thread) {
    bool was_disabled = interrupts_disable();
    finished_thread_t* finished_thread = get_finished_thread(thread);
    if (finished_thread != NULL) {
        list_remove(&finished_thread->list_link);
        kfree(finished_thread);
        kfree(thread);
        interrupts_restore(was_disabled);
        return;
    }
    
    ready_thread_t* ready_thread = get_ready_thread(thread);
    if (ready_thread != NULL) {
        list_remove(&ready_thread->list_link);
        kfree(ready_thread);
        kfree(thread);
        interrupts_restore(was_disabled);
        return;
    }

    suspended_thread_t* suspended_thread = get_suspended_thread(thread);
    if (suspended_thread != NULL) {
        list_remove(&suspended_thread->list_link);
        kfree(suspended_thread);
        kfree(thread);
        interrupts_restore(true);
    }
}

bool scheduler_has_thread_finished(thread_t* thread) {
    return get_finished_thread(thread) != NULL;      
}

/** Switch to next thread in the queue. */
void scheduler_schedule_next(void) {
    scheduler_schedule_next_with_action(schedule, NULL, NULL, NULL, NULL);
}

/** Switch current thread to thread in parameter */
void scheduler_switch_to(thread_t* thread) {
    scheduler_schedule_next_with_action(schedule, NULL, NULL, NULL, thread);
}

/** Perfoms the actual context switch on thread level.
 *
 * @param old Can be null if there is no running thread.
 * @param new Can be null, then next one from queue is taken.
 **/
static errno_t scheduler_context_switch(thread_t* old, thread_t* new, bool restore) {
    ready_thread_t* next_thread;
    if (new != NULL) {
        next_thread = get_ready_thread(new);
        if (next_thread == NULL) {
            return EINVAL;
        }
        list_remove(&next_thread->list_link);
    } else {
        next_thread = pop_next_thread();
    }

    current_thread = next_thread;

    as_t* as = current_thread->thread->as;
    uint8_t asid = as->id;

    if (old != NULL) {
        cpu_switch_context(&(old->stack_top), &(current_thread->thread->stack_top), asid);
    } else {
        void* old_stack;
        cpu_switch_context(&old_stack, &(current_thread->thread->stack_top), asid);
    } 

    interrupts_restore(restore);

    return EOK;
}
 
/** Performs context switch and handles the current thread accordingly
 *
 * @param action
 * When action = schedule, current thread is appended to the end of the queue to run again.
 * When action = suspend, current thread is suspended and set as waiting for accordingly to
 * waiting_for and waiting_for_result in case of join, both can be null.
 * When action = finish, current thread is finishd with result parameter.
 *
 * @param switch_to is the next thread to be run, if null, next thread from queue is taken.
 **/
errno_t scheduler_schedule_next_with_action(action_t action, thread_t* waiting_for, void** waiting_for_result, void* result, thread_t* switch_to) {
    bool was_disabled = interrupts_disable();
    // Check if the thread needs to be suspended.
    if (action == suspend && waiting_for != NULL) {
        finished_thread_t* finished_thread = get_finished_thread(waiting_for);
        if (finished_thread != NULL) {
            if (waiting_for_result != NULL) {
                *waiting_for_result = finished_thread->result;
            }
            interrupts_restore(was_disabled);
            return EOK;
        }
    }

    // Change current thread according to action.
    thread_t* current_thread_info = NULL;
    if (current_thread != NULL) {
        current_thread_info = current_thread->thread;
        ready_thread_t* previous_thread = current_thread;
        switch (action) {
            case schedule: 
                list_append(&ready_queue, &current_thread->list_link);
                break;
            case suspend:
                scheduler_add_suspended_thread(previous_thread->thread, waiting_for, waiting_for_result);
                kfree(previous_thread);
                break;
            case finish:
                finish_thread(current_thread->thread, result, false);
                kfree(current_thread);
                current_thread = NULL;
                break;
            case kill:
                finish_thread(current_thread->thread, NULL, true);
                kfree(current_thread);
                current_thread = NULL;
                break;
        }
    }

    return scheduler_context_switch(current_thread_info, switch_to, was_disabled);
}

/** Wakes given thread from suspended.
 *
 * @param thread Thread to wake from the suspended.
 */
errno_t scheduler_wake_thread(thread_t* thread) {
    if (current_thread != NULL){
        if (current_thread->thread == thread) {
            return EOK;
        }
    }

    if (scheduler_has_thread_finished(thread)) {
        return EEXITED;
    }

    ready_thread_t* ready_thread = get_ready_thread(thread);
    if (ready_thread != NULL) {
        return EOK;
    }

    suspended_thread_t* suspended_thread = get_suspended_thread(thread);
    if (suspended_thread != NULL) {
        scheduler_add_ready_thread(thread);
        list_remove(&suspended_thread->list_link);
        kfree(suspended_thread);

        return EOK;
    }
    
    return EINVAL;
}
