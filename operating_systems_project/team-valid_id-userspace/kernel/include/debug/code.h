// SPDX-License-Identifier: Apache-2.0
// Copyright 2019 Charles University

#ifndef _DEBUG_CODE_H
#define _DEBUG_CODE_H

#include <types.h>

/** Get current value of stack pointer. */
static inline uintptr_t debug_get_stack_pointer(void) {
    register void* rgrstr __asm__("$29");
    return (uintptr_t)rgrstr;
}

void debug_dump_function(const char* name, uintptr_t address, size_t instruction_count);

#endif
