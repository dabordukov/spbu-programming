#include <assert.h>
#include <stdbool.h>

#include "auxiliaries.h"

bool testBinarySearchElementExists(bool (*search)(int*, int, int)) {
    int array[50] = {0};
    array[49] = 1;

    return search(array, 50, 1);
}

bool testBinarySearchElementNotExists(bool (*search)(int*, int, int)) {
    int array[50] = {0};
    for (int i = 0; i < 50; i++) {
        array[i] = i + 2;
    }

    return !search(array, 50, 1);
}

bool testBinarySearchOddArray(bool (*search)(int*, int, int)) {
    int array[49] = {0};
    array[48] = 1;

    return search(array, 49, 1);
}

bool testBinarySearchLeftBorder(bool (*search)(int*, int, int)) {
    int array[50] = {0};
    for (int i = 0; i < 50; i++) {
        array[i] = i + 1;
    }

    return search(array, 50, 1);
}

bool testBinarySearchRightBorder(bool (*search)(int*, int, int)) {
    int array[50] = {0};
    for (int i = 0; i < 50; i++) {
        array[i] = -50 + i + 2;
    }

    return search(array, 50, 1);
}

void testBinarySearch(bool (*search)(int*, int, int)) {
    assert(testBinarySearchElementExists(search) == true);
    assert(testBinarySearchElementNotExists(search) == true);
    assert(testBinarySearchLeftBorder(search) == true);
    assert(testBinarySearchRightBorder(search) == true);
    assert(testBinarySearchOddArray(search) == true);
}