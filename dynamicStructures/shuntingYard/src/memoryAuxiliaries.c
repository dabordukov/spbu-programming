#include <stdio.h>
#include <stdlib.h>

void* checkedCalloc(size_t nmemb, size_t size) {
    void* p = calloc(nmemb, size);
    if (!p) {
        fprintf(stderr, "Can't allocate %lu bytes of memory!\n", size);
        abort();
    }
    return p;
}