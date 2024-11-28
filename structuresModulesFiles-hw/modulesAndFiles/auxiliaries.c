#include <assert.h>
#include <limits.h>
#include <stdbool.h>
#include <stdio.h>
#include <stdlib.h>

void swap(int* a, int* b) {
    int temp = *a;
    *a = *b;
    *b = temp;
}

void* checkedMalloc(size_t size) {
    void* p = malloc(size);
    if (!p) {
        fprintf(stderr, "Can't allocate memory!\n");
        assert(false);
    }
    return p;
}

void printArray(int* array, int size) {
    for (int i = 0; i < size; i++) {
        printf("%d ", array[i]);
    }
    puts(" ");
}

void fillArrayRand(int* array, int size, int rangeMax) {
    for (int i = 0; i < size; i++) {
        array[i] = rand() % rangeMax;
    }
}

bool addIntOverflow(int a, int b) {
    if (0ll + a + b > INT_MAX || 0ll + a + b < INT_MIN) {
        return true;
    }
    return false;
}

int parseArrayFromFile(char* filename, int** array) {
    FILE* file = fopen(filename, "r");
    int arraySize = 0;

    fscanf(file, "%d", &arraySize);
    *array = checkedMalloc(sizeof(int) * arraySize);

    int k = 0;
    for (int i = 0; i < arraySize; i++) {
        if (fscanf(file, "%d", (*array + i)) == 1) {
            k++;
        }
    }
    fclose(file);
    return k;
}