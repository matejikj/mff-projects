// SPDX-License-Identifier: Apache-2.0
// Copyright 2019 Charles University

#include <adt/list.h>
#include <debug.h>
#include <drivers/printer.h>
#include <lib/print.h>
#include <lib/stdarg.h>
#include <types.h>
#include <proc/mutex.h>
#include <debug/code.h>

mutex_t print_lock;
mutex_t puts_lock;

enum Case {
    Upper,
    Lower
};

void print_init() {
    mutex_init(&print_lock);
    mutex_init(&puts_lock);
}

static void puts_inline(const char* s) {
    while (*s != '\0') {
        printer_putchar(*s);
        s++;
    }
}

/** Prints given string to console, terminating it with newline.
 *
 * @param s String to print.
 */
void puts(const char* s) {
    mutex_lock(&puts_lock);

    puts_inline(s);
    printer_putchar('\n');
    
    mutex_unlock(&puts_lock);
}

static int getAsciiOffset(int digit, enum Case digitCase) {
    if (digit < 10) {
        return 48;
    } else if (digitCase == Lower) {
        return 87;
    } else {
        return 55;
    }
}

static void printDigit(int digit, enum Case digitCase) {
    printer_putchar((char)(digit + getAsciiOffset(digit, digitCase)));
}

static void printNumberByDigitArray(const int* digits, int digitCount, enum Case digitCase, int minimalDigitCount) {
    int numberStarted = 0;
    for (int j = digitCount - 1; j >= 0; --j) {
        const int digit = digits[j];
        if (digit != 0 || minimalDigitCount > j) {
            numberStarted = 1;
        }

        const int isLastDigit = j == 0;
        if (numberStarted || isLastDigit) {
            printDigit(digit, digitCase);
        }
    }
}

static int abs(int n) {
    if (n < 0) {
        return -n;
    }

    return n;
}

static void printInt(int n) {
    const int maxDigitsInInt = 10;
    const int isNegative = n < 0;
    int digits[maxDigitsInInt];
    for (int i = 0; i < maxDigitsInInt; ++i) {
        digits[i] = abs(n % 10);
        n /= 10;
    }

    if (isNegative) {
        printer_putchar('-');
    }

    printNumberByDigitArray(digits, maxDigitsInInt, Lower, 0);
}

static void printUnsignedInt(unsigned int n) {
    const int maxDigits = 10;
    int digits[maxDigits];
    for (int i = 0; i < maxDigits; ++i) {
        digits[i] = abs(n % 10);
        n /= 10;
    }
    printNumberByDigitArray(digits, maxDigits, Lower, 0);

}

static void printHex(unsigned int n, enum Case digitCase, int minimalDigitCount) {
    const int maxDigitsInUnsignedInt = 8;
    int digits[maxDigitsInUnsignedInt];
    for (int i = 0; i < maxDigitsInUnsignedInt; ++i) {
        digits[i] = n % 16;
        n /= 16;
    }

    printNumberByDigitArray(digits, maxDigitsInUnsignedInt, digitCase, minimalDigitCount);
}

static void printPointer(void* p) {
    puts_inline("0x");
    printHex((unsigned int)p, Lower, 0);
}

static void printList(list_t* list) {
    printPointer(list);
    printer_putchar('[');
    size_t size = list_get_size(list);
    if (size == 0)  {
        puts_inline("empty");
    } else {
        printInt(size);
        printer_putchar(':');
        printer_putchar(' ');

        link_t* headItem = list->head.next;
        printPointer(headItem);
        link_t* item = headItem->next;
        while (item != &(list->head)) {
            printer_putchar('-');
            printPointer(item);
            item = item->next;
        }
    }
    printer_putchar(']');
}

/** Prints given formatted string to console.
 *
 * @param format printf-style formatting string.
 */
void printk(const char* format, ...) {
    va_list args;
    va_start(args, format);

    mutex_lock(&print_lock);

    const char* it = format;
    while (*it != '\0') {
        if (*it == '%') {

            it++;

            switch (*it) {
                case '%': {
                    printer_putchar('%');
                    break;
                }
                case 'd': {
                    const int argument = va_arg(args, int);
                    printInt(argument);
                    break;
                }
                case 'u': {
                    const unsigned int argument = va_arg(args, unsigned int);
                    printUnsignedInt(argument);
                    break;
                }

                case 's': {
                    const char* argument = va_arg(args, char*);
                    puts_inline(argument);
                    break;
                }
                case 'x': {
                    const unsigned int argument = va_arg(args, unsigned int);
                    it++;
                    int minimalDigitCount = ((int)*it) - 48; 
                    if (minimalDigitCount >= 0 && minimalDigitCount <= 9) {
                        printHex(argument, Lower, minimalDigitCount);
                    } else {
                        printHex(argument, Lower, 0);
                        it--;
                    }
                    break;
                }
                case 'X': {
                    const unsigned int argument = va_arg(args, unsigned int);
                    printHex(argument, Upper, 0);
                    break;
                }
                case 'c': {
                    const char argument = va_arg(args, int);
                    printer_putchar(argument);
                    break;
                }
                case 'p': {
                    it++;

                    if (*it == 'L') {
                        list_t* argument = va_arg(args, void*);
                        printList(argument);
                    } else {
                        it--;
                        void* argument = va_arg(args, void*);
                        printPointer(argument);
                    }
                    break;
                }
            }
        } else {
            printer_putchar(*it);
        }

        it++;
    }
    mutex_unlock(&print_lock);

    va_end(args);
}
