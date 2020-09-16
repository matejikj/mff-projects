// SPDX-License-Identifier: Apache-2.0
// Copyright 2019 Charles University

#include <lib/print.h>
#include <debug/mm.h>
#include <main.h>
#include <exc.h>

size_t memory_size;
bool computed;

/** Probe available base physical memory.
 *
 * Do not check for non-continuous memory blocks or for memory available via
 * TLB only.
 *
 * @return Amount of memory available in bytes.
 */
size_t debug_get_base_memory_size(void) {
    if (computed) {
        return memory_size;
    }

    bool was_disabled = interrupts_disable();
    int* p = (int*)(&_kernel_end);
    int size = 0;
    while (*p != -1) {
        size += sizeof(p);
        p++;
    }
    interrupts_restore(was_disabled);

    memory_size = size;
    computed = true;
    printk("MEMORY SIZE %d\n", size);

    return size;
}

