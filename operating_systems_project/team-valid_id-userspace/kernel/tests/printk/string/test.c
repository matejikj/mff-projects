// SPDX-License-Identifier: Apache-2.0
// Copyright 2019 Charles University

#include <ktest.h>
#include <lib/print.h>

void kernel_test(void) {
    puts(KTEST_EXPECTED "This is ONE string.");
    printk(KTEST_ACTUAL "This is %s string.\n", "ONE");

    puts(KTEST_EXPECTED "This is FIRST and SECOND string.");
    printk(KTEST_ACTUAL "This is %s and %s string.\n", "FIRST", "SECOND");

    // This is actually undefined behaviour but it is probably safer
    // to test for it explicitly...
    // puts(KTEST_EXPECTED "This is NULL string: (null).");
    // printk(KTEST_ACTUAL "This is NULL string: %s.\n", NULL);

    ktest_passed();
}
