// SPDX-License-Identifier: Apache-2.0
// Copyright 2019 Charles University

#include <proc/sem.h>

/** Initializes given semaphore.
 *
 * @param sem Semaphore to initialize.
 * @param value Initial semaphore value (1 effectively creates mutex).
 * @return Error code.
 * @retval EOK Semaphore was successfully initialized.
 */
errno_t sem_init(sem_t* sem, int value) {
    assert(sem != NULL);

    sem->value = value;
    list_init(&sem->waiting_queue);

    return EOK;   
}

/** Destroys given semaphore.
 *
 * The function must panic if there are threads waiting for this semaphore.
 *
 * @param sem Semaphore to destroy.
 */
void sem_destroy(sem_t* sem) {
    bool was_disabled = interrupts_disable();

    if (!list_is_empty(&sem->waiting_queue)) {
        panic("destroying semaphor with waiting threads");
    }

    interrupts_restore(was_disabled);
}

/** Get current value of the semaphore.
 *
 * @param sem Semaphore to query.
 * @return Current semaphore value.
 */
int sem_get_value(sem_t* sem) {
    return sem->value;
}

/** Locks (downs) the semaphore.
 *
 * Decrement the value of this semaphore. If the new value would be negative,
 * block and wait for someone to call sem_post() first.
 *
 * @param sem Semaphore to be locked.
 */
void sem_wait(sem_t* sem) {
    bool was_disabled = interrupts_disable();

    if (sem->value == 0) {
        thread_t *ct = thread_get_current();
        add_waiting_thread(&sem->waiting_queue, ct);
        thread_suspend();
    } else {
        sem->value--;
    }

    interrupts_restore(was_disabled);
}

/** Unlocks (ups/signals) the sempahore.
 *
 * Increment the value of this semaphore or wake-up one of blocked threads
 * inside sem_wait().
 *
 * @param sem Semaphore to be unlocked.
 */
void sem_post(sem_t* sem) {
    bool was_disabled = interrupts_disable();

    if (list_is_empty(&sem->waiting_queue)) {
        sem->value++;
    } else {
        thread_t* wt = pop_waiting_thread(&sem->waiting_queue);
        thread_wakeup(wt);
    }

    interrupts_restore(was_disabled);
}

/** Try to lock the semaphore without waiting.
 *
 * If the call to sem_wait() would block, do nothing and return EBUSY.
 *
 * @param sem Semaphore to be locked.
 * @return Error code.
 * @retval EOK Semaphore was successfully locked.
 * @retval EBUSY Semaphore has value of 0 and locking would block.
 */
errno_t sem_trywait(sem_t* sem) {
    bool was_disabled = interrupts_disable();

    if (sem->value > 0) {
        sem->value -= 1;
        interrupts_restore(was_disabled);
        return EOK;
    } else {
        interrupts_restore(was_disabled);
        return EBUSY;
    }
}
