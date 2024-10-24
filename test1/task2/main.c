#include <stdio.h>
#include <stdlib.h>
#include <time.h>

#include "auxiliaries.h"
#include "sortingTests.h"

void shellSort(int* array, int size) {
    for (int step = size / 2; step > 0; step /= 2) {
        for (int i = 0; i < size - step; i++) {
            for (int j = 0; j < size; j += step) {
                if (j + step >= size) {
                    break;
                }

                if (array[j] > array[j + step]) {
                    swap(array + j, array + j + step);
                }
            }
        }
    }
}

#define N 25
int main() {
    testSortingAll(&shellSort);
    srand(time(NULL));

    int array[N] = {0};
    fillArrayRand(array, N, 100);

    printf("Исходный массив:\n");
    printArray(array, N);

    shellSort(array, N);

    printf("Отсортированный массив:\n");
    printArray(array, N);
}