#include <limits.h>
#include <stdbool.h>
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
        int newElement = rand() % rangeMax;
        array[i] = (rand() % 2) ? newElement : -newElement;
    }
}

bool addIntOverflow(int a, int b) {
    if (0ll + a + b > INT_MAX || 0ll + a + b < INT_MIN) {
        return true;
    }
    return false;
}
