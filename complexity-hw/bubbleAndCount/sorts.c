#include "sorts.h"

#include <stdlib.h>

#include "auxiliaries.h"

void bubbleSort(int* array, int size) {
    for (int i = 1; i < size - 1; i++) {
        for (int j = i; j >= 0; j--) {
            if (array[j] > array[j + 1]) {
                swap(array + j, array + j + 1);
            }
        }
    }
}
int countSort(int* array, int size) {
    if (size < 1) {
        return 1;
    }

    long long elementMax = array[0];
    long long elementMin = array[0];
    int* countArray = NULL;
    for (int i = 1; i < size; i++) {
        if (elementMax < array[i]) {
            elementMax = array[i];
        }
        if (elementMin > array[i]) {
            elementMin = array[i];
        }
    }

    size_t countArraySize = elementMax - elementMin + 1;
    countArray = checkedCalloc(countArraySize, sizeof(int));

    for (size_t i = 0; i < size; i++) {
        countArray[array[i] - elementMin]++;
    }

    for (size_t i = 0, j = 0; i < countArraySize; i++) {
        while (countArray[i]--) {
            array[j++] = i + elementMin;
        }
    }
    free(countArray);
    return 0;
}
