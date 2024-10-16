#include <stdbool.h>
#include <stdio.h>
#include <stdlib.h>

void* checkedMalloc(size_t size) {
    void* p = malloc(size);
    if (!p) {
        fprintf(stderr, "Can't allocate %zu bytes of memory!\n", size);
        abort();
    }
    return p;
}

void* checkedCalloc(size_t nmemb, size_t size) {
    void* p = calloc(nmemb, size);
    if (!p) {
        fprintf(stderr, "Can't allocate %zu bytes of memory!\n", size);
        abort();
    }
    return p;
}

void swapPtr(void** a, void** b) {
    void* t = *a;
    *a = *b;
    *b = t;
}

void swapInt(int* a, int* b) {
    int t = *a;
    *a = *b;
    *b = t;
}