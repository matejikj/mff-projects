// SPDX-License-Identifier: Apache-2.0
// Copyright 2019 Charles University

#include <proc/mutex.h>

/** Initializes given mutex.
 *
 * @param mutex Mutex to initialize.
 * @return Error code.
 * @retval EOK Mutex was successfully initialized.
 */
errno_t mutex_init(mutex_t* mutex) {
    assert(mutex != NULL);

    mutex->locked = false;
    mutex->thread = NULL;
    list_init(&mutex->waiting_queue);

    return EOK;
}

/** Destroys given mutex.
 *
 * The function must panic if the mutex is still locked when being destroyed.
 *
 * @param mutex Mutex to destroy.
 */
void mutex_destroy(mutex_t* mutex) {
    if ( mutex->locked ) {
        panic("destroying locked mutex");
    } else {
    }
}

/** Locks the mutex.
 *
 * Note that when this function returns, the mutex must be locked.
 *
 * @param mutex Mutex to be locked.
 */
void mutex_lock(mutex_t* mutex) {
    bool was_enabled = interrupts_disable();

    if ( mutex->locked ) {
        thread_t* ct = thread_get_current();
        if (ct != mutex->thread) {
            add_waiting_thread(&mutex->waiting_queue, ct);
            thread_suspend();
        }
    } else {
        mutex->locked = true;
        mutex->thread = thread_get_current();
    }

    interrupts_restore(was_enabled);
}

/** Unlocks the mutex.
 *
 * Note that when this function returns, the mutex might be already locked
 * by a different thread.
 *
 * This function shall panic if the mutex is unlocked by a different thread
 * than the one that locked it.
 *
 * @param mutex Mutex to be unlocked.
 */
void mutex_unlock(mutex_t* mutex) {
    bool was_disabled = interrupts_disable();

    if ( mutex->thread != thread_get_current() ) {
        panic("unlocked by a different thread than the one that locked it");
    }

    if ( !list_is_empty(&mutex->waiting_queue) ) {
        thread_t *wt = pop_waiting_thread(&mutex->waiting_queue);
        mutex->thread = wt;
        if (wt != NULL) {
            thread_wakeup(wt);
        }
    } else {
        mutex->locked = false;
    }

    interrupts_restore(was_disabled);
}

/** Try to lock the mutex without waiting.
 *
 * If the mutex is already locked, do nothing and return EBUSY.
 *
 * @param mutex Mutex to be locked.
 * @return Error code.
 * @retval EOK Mutex was successfully locked.
 * @retval EBUSY Mutex is currently locked by a different thread.
 */
errno_t mutex_trylock(mutex_t* mutex) {
    bool was_disabled = interrupts_disable();

    if ( !mutex->locked ) {
        mutex->locked = true;
        mutex->thread = thread_get_current();
        interrupts_restore(was_disabled);
        return EOK;
    } else {
        interrupts_restore(was_disabled);
        return EBUSY;
    }
}
