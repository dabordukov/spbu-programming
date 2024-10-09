#include <stdio.h>
#include <stdlib.h>
#include <time.h>

#include "auxiliaries.h"

void shellSort(int* array, int size) {
    for (int step = size / 2; step > 0; step /= 2) {
        for (int i = step; i < size; i++) {
            for (int j = i - step; j >= 0 && array[j] > array[j + step]; j -= step) {
                swap(array + j, array + j + step);
            }
        }
    }
}

int main() {
    srand(time(NULL));
    int array[25] = {0};
    fillArrayRand(array, sizeof(array) / sizeof(int), 100);
    shellSort(array, sizeof(array) / sizeof(int));
    printArray(array, sizeof(array) / sizeof(int));
}