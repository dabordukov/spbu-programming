#include <assert.h>
#include <limits.h>
#include <stdbool.h>
#include <stdlib.h>

#include "auxiliaries.h"

#define DEFAULT_ARRAY_SIZE 1000
#define LARGE_ARRAY_SIZE 10000

bool isSorted(int* array, int size) {
    for (int i = 0; i < size - 1; i++) {
        if (array[i] > array[i + 1]) {
            return false;
        }
    }
    return true;
}

bool testSortingArrayOfOne(void (*sort)(int*, int)) {
    int arrayOfOne[1] = {5};
    sort(arrayOfOne, 1);
    return arrayOfOne[0] == 5;
}

bool testSortingAllEqual(void (*sort)(int*, int)) {
    int allEqualArray[DEFAULT_ARRAY_SIZE] = {0};
    for (int i = 0; i < DEFAULT_ARRAY_SIZE; allEqualArray[i++] = 2);
    sort(allEqualArray, DEFAULT_ARRAY_SIZE);
    return isSorted(allEqualArray, DEFAULT_ARRAY_SIZE);
}

bool testSortingSorted(void (*sort)(int*, int)) {
    int sortedArray[DEFAULT_ARRAY_SIZE] = {0};
    for (int i = 0; i < DEFAULT_ARRAY_SIZE; i++) {
        sortedArray[i] = i - DEFAULT_ARRAY_SIZE / 2;
    }
    sort(sortedArray, DEFAULT_ARRAY_SIZE);

    return isSorted(sortedArray, DEFAULT_ARRAY_SIZE);
}

bool testSortingReverseSorted(void (*sort)(int*, int)) {
    int reverseSortedArray[DEFAULT_ARRAY_SIZE] = {0};
    for (int i = 0; i < DEFAULT_ARRAY_SIZE; i++) {
        reverseSortedArray[i] = -i + DEFAULT_ARRAY_SIZE / 2;
    }
    sort(reverseSortedArray, DEFAULT_ARRAY_SIZE);

    return isSorted(reverseSortedArray, DEFAULT_ARRAY_SIZE);
}

bool testSortingLargeArray(void (*sort)(int*, int)) {
    int* largeArray = malloc(LARGE_ARRAY_SIZE * sizeof(int));
    fillArrayRand(largeArray, LARGE_ARRAY_SIZE, INT_MAX);
    sort(largeArray, LARGE_ARRAY_SIZE);
    bool result = isSorted(largeArray, LARGE_ARRAY_SIZE);
    free(largeArray);
    return result;
}

void testSortingAll(void (*sort)(int*, int)) {
    assert(testSortingAllEqual(sort));
    assert(testSortingArrayOfOne(sort));
    assert(testSortingSorted(sort));
    assert(testSortingReverseSorted(sort));
}