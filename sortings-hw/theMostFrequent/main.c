#include <assert.h>
#include <limits.h>
#include <stdbool.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <time.h>

#include "auxiliaries.h"
#include "sortings.h"
#include "tests.h"

int findMostFrequent(const int* array, int size) {
    int* copiedArray = malloc(size * sizeof(int));
    memcpy(copiedArray, array, size * sizeof(int));
    qSort(copiedArray, size);

    int mostFrequentElement = 0;
    int mostFrequency = 0;

    int currentElement = copiedArray[0];
    int currentFrequency = 0;
    for (int i = 0; i < size; i++) {
        if (copiedArray[i] == currentElement) {
            currentFrequency++;
        } else {
            currentElement = copiedArray[i];
            currentFrequency = 1;
        }
        if (currentFrequency > mostFrequency) {
            mostFrequency = currentFrequency;
            mostFrequentElement = currentElement;
        }
    }

    free(copiedArray);
    return mostFrequentElement;
}

void allTests() {
    assert(testMostFrequentEveryElementOccursOnce(&findMostFrequent) == true);
    assert(testMostFrequentEveryElementEquals(&findMostFrequent) == true);
    assert(testMostFrequentArrayOfOne(&findMostFrequent) == true);
}

int main() {
    srand(time(NULL));
    allTests();

    int exampleArray[50] = {0};
    fillArrayRand(exampleArray, 50, 20);
    printf("Array:\n");
    printArray(exampleArray, 50);
    printf("Most frequent element: %d\n", findMostFrequent(exampleArray, 50));
}