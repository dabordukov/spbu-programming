#ifndef PHONEBOOK_MEMORY_AUX_H
#define PHONEBOOK_MEMORY_AUX_H 1
#include <stdlib.h>

void* checkedMalloc(size_t size);
void* checkedCalloc(size_t nmemb, size_t size);
void swapPtr(void** a, void** b);
void swapInt(int* a, int* b);
#endif