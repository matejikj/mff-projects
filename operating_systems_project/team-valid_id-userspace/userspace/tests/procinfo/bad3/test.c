// SPDX-License-Identifier: Apache-2.0
// Copyright 2019 Charles University

#include <np/proc.h>
#include <np/utest.h>
#include <stddef.h>

/*
 * Checks that accessing kernel memory causes forced process termination.
 */

int main(void) {
    utest_start("procinfo/bad3");

    bool ok = np_proc_info_get((np_proc_info_t*)0x80000080);

    utest_assert(!ok, "bad address shall not abort the program");

    utest_passed();

    return 0;
}
