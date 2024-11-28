#include <assert.h>
#include <stdbool.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <time.h>

#include "auxiliaries.h"
#include "sortingTests.h"
#include "sortings.h"

void allTests() {
    assert(testSortingArrayOfOne(&insertionSort) == true);
    assert(testSortingArrayOfOne(&qSort) == true);

    assert(testSortingAllEqual(&insertionSort) == true);
    assert(testSortingAllEqual(&qSort) == true);

    assert(testSortingSorted(&insertionSort) == true);
    assert(testSortingSorted(&qSort) == true);

    assert(testSortingReverseSorted(&insertionSort) == true);
    assert(testSortingReverseSorted(&qSort) == true);

    assert(testSortingLargeArray(&qSort) == true);
}

int main() {
    srand(time(NULL));
    allTests();

    int exampleArray[50] = {0};
    printf("Original array:\n");
    fillArrayRand(exampleArray, 50, 500);
    printArray(exampleArray, 50);
    qSort(exampleArray, 50);
    printf("Sorted:\n");
    printArray(exampleArray, 50);
}