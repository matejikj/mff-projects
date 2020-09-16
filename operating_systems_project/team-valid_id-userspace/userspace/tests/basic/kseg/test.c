// SPDX-License-Identifier: Apache-2.0
// Copyright 2019 Charles University

#include <np/types.h>
#include <np/utest.h>
#include <stddef.h>
#include <stdio.h>

/*
 * Checks that accessing kernel memory causes forced process termination.
 */

static volatile uintptr_t kseg_address = 0x80000020;
static volatile uint8_t blackhole;

int main(void) {
    utest_start("basic/kseg");
    utest_expect_abort();

    uint8_t* cause_page_fault = (uint8_t*)kseg_address;
    blackhole = *cause_page_fault;

    printf("Survived access to kernel memory.\n");

    utest_failed();

    return 0;
}
