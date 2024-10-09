#include <assert.h>
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

void testAll() {
    assert(testSortingAllEqual(&shellSort));
    assert(testSortingArrayOfOne(&shellSort));
    assert(testSortingSorted(&shellSort));
    assert(testSortingReverseSorted(&shellSort));
}

#define N 25
int main() {
    testAll();
    srand(time(NULL));
    int array[N] = {0};
    fillArrayRand(array, N, 100);
    printArray(array, N);
    shellSort(array, N);
    printArray(array, N);
}