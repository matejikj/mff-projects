// SPDX-License-Identifier: Apache-2.0
// Copyright 2019 Charles University

#include <drivers/timer.h>
#include <exc.h>
#include <lib/print.h>
#include <proc/thread.h>
#include <debug.h>
#include <mm/as.h>

void handle_exception_general(context_t* context) {
    bool is_timer_interrupt = cp0_cause_is_interrupt_pending(context->status, 7);
    bool is_invalid_tlb_access = cp0_cause_get_exc_code(context->cause) == 3; 
    if (is_invalid_tlb_access) {
        printk("INVALID TLB ACCESS ON %x BY %s %x\n", context->badva, thread_get_current()->name, context->epc);
        thread_kill(thread_get_current());
    } else if (is_timer_interrupt) {
        thread_yield();
        timer_interrupt_after(100000);
    } else {
        panic("Unhandled exception occured");
    }
}

bool interrupts_disable(void) {
    unative_t status = cp0_read(REG_CP0_STATUS);
    cp0_write(REG_CP0_STATUS, status & ~CP0_STATUS_IE_BIT);
    return (status & CP0_STATUS_IE_BIT) > 0;
}

void interrupts_restore(bool enable) {
    unative_t status = cp0_read(REG_CP0_STATUS);
    if (enable) {
        status = status | CP0_STATUS_IE_BIT;
    } else {
        status = status & ~CP0_STATUS_IE_BIT;
    }
    cp0_write(REG_CP0_STATUS, status);
}
