// SPDX-License-Identifier: Apache-2.0
// Copyright 2019 Charles University

#include <mm/tlb.h>
#include <mm/as.h>
#include <lib/print.h>
#include <proc/thread.h>
#include <exc.h>

uint32_t va_offset = 12;

static void write_tlb_entry(uint8_t asid, uintptr_t address, int32_t even_pfn, int32_t odd_pfn) {
    uint32_t vpn = (address >> va_offset);
    uint32_t vpn2 = vpn / 2;

    //printk("EPFN: %x, OPFN: %x, VPN: %x\n", even_pfn, odd_pfn, vpn2);

    cp0_write_pagemask_4k();
    cp0_write_entrylo0(even_pfn, true, even_pfn != -1, false);
    cp0_write_entrylo1(odd_pfn, true, odd_pfn != -1, false);
    cp0_write_entryhi(vpn2, asid);
    cp0_tlb_write_random();
}

void handle_tlb_refill(context_t* context) {
    bool disabled = interrupts_disable();
    thread_t* thread = thread_get_current();
    as_t* as = thread_get_as(thread);
    //printk("ASID %d, SIZE: %x, ADDRESS: %x \n", as->id, as->size, context->badva);

    uintptr_t phys;
    errno_t error = as_get_mapping(as, context->badva, &phys);
    if (error != EOK) {
        thread_kill(thread);
    }

    uintptr_t aligned_vaddr = context->badva & 0xfffff000;
    uintptr_t even_vaddr = aligned_vaddr % (2 * PAGE_SIZE) == 0 ? aligned_vaddr : aligned_vaddr - PAGE_SIZE;

    uintptr_t even_phys;
    uintptr_t odd_phys;
    errno_t even_result = as_get_mapping(as, even_vaddr, &even_phys);
    errno_t odd_result = as_get_mapping(as, even_vaddr + PAGE_SIZE, &odd_phys);

    //printk("ALIGNED EVEN VADDR: %x, EP: %x, OP: %x\n", even_vaddr, even_phys, odd_phys);

    int32_t even_pfn = even_result == EOK ? (int32_t)(even_phys >> va_offset) : -1;
    int32_t odd_pfn = odd_result == EOK ? (int32_t)(odd_phys >> va_offset) : -1;
    
    if (even_result == EOK || odd_result == EOK) {
        write_tlb_entry(as->id, even_vaddr, even_pfn, odd_pfn);
    }
    interrupts_restore(disabled);
}
    
