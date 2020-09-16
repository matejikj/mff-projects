// SPDX-License-Identifier: Apache-2.0
// Copyright 2019 Charles University

#include <mm/heap.h>
#include <debug/mm.h>
#include <main.h>
#include <lib/print.h>
#include <proc/mutex.h>
#include <debug/code.h>
#include <mm/frame.h>

void* memory_start;
void* memory_end;
mutex_t heap_lock;

// Heap block structure:
// 4B - indicating if it is free or taken
// 4B - size of the data 
// XB - data itself
// 4B - size of the data (repeated for backwards iteration)
typedef struct heap_block heap_block;
struct heap_block {
    size_t is_free;
    size_t data_size;
    size_t data [0];
};

// Size of the block without data.
int heap_block_metadata_size = sizeof(heap_block) + sizeof(size_t);

void init_block(heap_block* block, size_t data_size, size_t is_free);
size_t get_block_size(heap_block* block);
heap_block* get_next_block(heap_block* current);
heap_block* get_previous_block(heap_block* current);
void split_free_block(heap_block* block, size_t new_size);

void init_block(heap_block* block, size_t size, size_t is_free) {
    block->is_free = is_free;
    block->data_size = size;

    size_t* size_after = (size_t*)((uintptr_t)block + sizeof(heap_block) + size); 
    *size_after = size;
} 

size_t get_block_size(heap_block* block) {
   return heap_block_metadata_size + block->data_size;
}

heap_block* get_next_block(heap_block* current) {
    uintptr_t next_block_address = (uintptr_t)current + get_block_size(current);
    if (next_block_address >= (uintptr_t)memory_end) {
        return NULL;
    } else {
        return (heap_block*)next_block_address;
    }
}

heap_block* get_previous_block(heap_block* current) {
    if (current == memory_start) {
        return NULL;
    }

    size_t prev_block_data_size = *((size_t*)((uintptr_t)current - sizeof(size_t)));
    uintptr_t prev_block_address = (uintptr_t)current - prev_block_data_size - heap_block_metadata_size; 
    return (heap_block*)prev_block_address;
}

void split_free_block(heap_block* block, size_t new_size) {
    // Splitting is not worth if there would not no space for data. 
    size_t not_worth_splitting = new_size >= block->data_size - heap_block_metadata_size; 
    if (not_worth_splitting) {
        block->is_free = 0;
        return;
    }

    size_t original_size = get_block_size(block);
    init_block(block, new_size, 0);
    heap_block* remaining_block = get_next_block(block);
    if (remaining_block == NULL) {
        panic("Incorrect block split.");
    }
    size_t remaining_size = original_size - get_block_size(block);
    size_t remaining_data_size = remaining_size - heap_block_metadata_size; 
    init_block(remaining_block, remaining_data_size, 1);
}

void heap_init(void) {
    size_t memory_size = debug_get_base_memory_size();
    size_t page_count = memory_size / FRAME_SIZE;
    size_t heap_page_count = 3 + page_count / 5; // Heap has eight of the memory.
    uintptr_t first_page;
    frame_alloc(heap_page_count, &first_page);
    memory_start = (void*)(first_page | 0x80000000);
    size_t heap_memory_size = heap_page_count * FRAME_SIZE;
    memory_end = (void*)((uintptr_t)memory_start + heap_memory_size);
    printk("HEAP MEMORY - FROM %x TO %x\n", memory_start, memory_end);

    // Initialize the first block.
    init_block(memory_start, heap_memory_size - heap_block_metadata_size, 1); 
    mutex_init(&heap_lock);
}

void* kmalloc(size_t size) {
//    mutex_lock(&heap_lock);
    bool was_disabled = interrupts_disable();
    size_t rounded_size = size + ((4 - size % 4) % 4);

    heap_block* best_fit = NULL;
    heap_block* current_block = (heap_block*)memory_start;
    while (current_block != NULL) {
        if (current_block->is_free == 1) {
            if (current_block->data_size == rounded_size) {
                current_block->is_free = 0;
                // mutex_unlock(&heap_lock);
                interrupts_restore(was_disabled);
                return (void*)&(current_block->data);
            } else if (current_block->data_size > rounded_size) {
                if (best_fit == NULL || best_fit->data_size > current_block->data_size) {
                    best_fit = current_block;
                }
            }
        }
        
        current_block = get_next_block(current_block); 
    }
    
    if (best_fit == NULL) {
        //mutex_unlock(&heap_lock);
        interrupts_restore(was_disabled);
        return NULL;
    } else {
        split_free_block(best_fit, rounded_size);
        // mutex_unlock(&heap_lock);
        interrupts_restore(was_disabled);
        return (void*)(&(best_fit->data));
    }
}

void kfree(void* ptr) {
    // mutex_lock(&heap_lock);
    bool was_disabled = interrupts_disable();

    heap_block* block = (heap_block*)((uintptr_t)ptr - sizeof(heap_block)); 
    heap_block* first_right_full_block = get_next_block(block);
    while (first_right_full_block != NULL && first_right_full_block->is_free == 1) {
        first_right_full_block = get_next_block(first_right_full_block);
    }
    
    heap_block* first_left_full_block = get_previous_block(block);
    while (first_left_full_block != NULL && first_left_full_block->is_free == 1) {
        first_left_full_block = get_previous_block(first_left_full_block);
    }

    heap_block* new_free_block;
    if (first_left_full_block == NULL) {
        new_free_block = (heap_block*)memory_start;
    } else {
        new_free_block = get_next_block(first_left_full_block);
    }
    
    void* new_block_end;
    if (first_right_full_block == NULL) {
        new_block_end = memory_end;
    } else {
        new_block_end = first_right_full_block;
    }

    size_t new_size = ((uintptr_t)new_block_end - (uintptr_t)new_free_block) - heap_block_metadata_size; 
    init_block(new_free_block, new_size, 1); 

    interrupts_restore(was_disabled);
    // mutex_unlock(&heap_lock);
}
