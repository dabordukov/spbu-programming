#include <assert.h>
#include <stdbool.h>
#include <stdlib.h>
#include <time.h>

#include "tree.h"

bool isSorted(int* array, size_t size, bool (*compare)(int, int)) {
    for (size_t i = 1; i < size; i++) {
        if (!compare(array[i - 1], array[i])) {
            return false;
        }
    }
    return true;
}

bool sortAscending(int left, int right) {
    return left < right;
}

bool sortDescending(int left, int right) {
    return left > right;
}

#define ARRAY_SIZE 1000
void testCaseAscendingSort() {
    int array[ARRAY_SIZE] = {0};
    for (int i = 0; i < ARRAY_SIZE; i++) {
        array[i] = rand();
    }

    treeSort(array, ARRAY_SIZE, &sortAscending);

    assert(isSorted(array, ARRAY_SIZE, &sortAscending));
}

void testCaseDescendingSort() {
    int array[ARRAY_SIZE] = {0};
    for (int i = 0; i < ARRAY_SIZE; i++) {
        array[i] = rand();
    }

    treeSort(array, ARRAY_SIZE, &sortDescending);

    assert(isSorted(array, ARRAY_SIZE, &sortDescending));
}

int main() {
    srand(time(NULL));
    testCaseAscendingSort();
    testCaseDescendingSort();
}