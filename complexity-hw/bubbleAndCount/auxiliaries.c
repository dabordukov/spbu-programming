#include <stdio.h>
#include <stdlib.h>

void swap(int* a, int* b) {
    int temp = *a;
    *a = *b;
    *b = temp;
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
