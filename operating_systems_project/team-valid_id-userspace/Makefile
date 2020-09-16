# SPDX-License-Identifier: Apache-2.0
# Copyright 2019 Charles University

GDB_PORT = 10001
DIFF = diff

-include config.mk

### Phony targets

.PHONY: all clean distclean kernel userspace run-msim-gdb run-gdb cstyle fix-cstyle



### Default target

all: kernel userspace

kernel:
	$(MAKE) -C kernel

userspace:
	$(MAKE) -C userspace

clean:
	$(MAKE) -C kernel clean
	$(MAKE) -C userspace clean

distclean:
	$(MAKE) -C kernel distclean
	$(MAKE) -C userspace distclean
	rm -f config.mk

run-msim-gdb:
	msim -g $(GDB_PORT)

run-gdb:
	$(GDB) -ix gdbinit -iex "target remote :$(GDB_PORT)"

cstyle:
	find kernel/ userspace/ -name '*.[ch]' | while read fname; do clang-format -style=file "$$fname" | $(DIFF) -ud "$$fname" -; done

fix-cstyle:
	find kernel/ userspace/ -name '*.[ch]' -exec clang-format -style=file -i {} \;
