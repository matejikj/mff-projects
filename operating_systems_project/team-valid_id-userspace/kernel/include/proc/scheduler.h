// SPDX-License-Identifier: Apache-2.0
// Copyright 2019 Charles University

#ifndef _PROC_SCHEDULER_H
#define _PROC_SCHEDULER_H

#include <proc/thread.h>

typedef enum {
    suspend = 0,
    schedule = 1,
    finish = 2,
    kill = 3
} action_t;

void scheduler_init(void);
void scheduler_add_ready_thread(thread_t* id);
void scheduler_remove_thread(thread_t* id);
void scheduler_schedule_next(void);
void scheduler_switch_to(thread_t* thread);
errno_t scheduler_schedule_next_with_action(action_t current_thread_action, thread_t* waiting_for, void** waiting_for_result, void* result, thread_t* switch_to);
bool scheduler_has_thread_finished(thread_t* thread);
thread_t* scheduler_get_current_thread(void);
errno_t scheduler_wake_thread(thread_t* thread);
bool is_killed(thread_t* thread);
errno_t kill_thread(thread_t* thread);
 
#endif
