#include <stdbool.h>
#include <stdio.h>
#include <stdlib.h>

#define flushSTDIN() scanf("%*[^\n]")

void* checkedMalloc(size_t size) {
    void* p = malloc(size);
    if (!p) {
        fprintf(stderr, "Can't allocate %zu bytes of memory!\n", size);
        abort();
    }
    return p;
}
