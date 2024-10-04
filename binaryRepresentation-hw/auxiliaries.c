#include <assert.h>
#include <stdbool.h>
#include <stdio.h>
#include <stdlib.h>

void* checkedMalloc(size_t size) {
    void* p = malloc(size);
    if (!p) {
        fprintf(stderr, "Can't allocate memory!\n");
        assert(false);
    }
    return p;
}

void* checkedCalloc(size_t nmemb, size_t size) {
    void* p = calloc(nmemb, size);
    if (!p) {
        fprintf(stderr, "Can't allocate memory!\n");
        assert(false);
    }
    return p;
}

void printBits(char* bits, int size) {
    for (int i = 0; i < size; i++) {
        printf("%d ", bits[i]);
    }
    puts(" ");
}

void swapBits(char* a, char* b) {
    char temp = *a;
    *a = *b;
    *b = temp;
}

void reverseBits(char* bits, int size) {
    char* left = bits;
    char* right = bits + size - 1;

    while (left < right) {
        swapBits(left++, right--);
    }
}
