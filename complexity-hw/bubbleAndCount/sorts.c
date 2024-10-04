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

void countSort(int* array, int size) {
    int countArray[COUNT_SORT_RANGE_MAX] = {0};
    for (int i = 0; i < size; i++) {
        countArray[array[i]]++;
    }

    for (int i = 0, j = 0; i < COUNT_SORT_RANGE_MAX; i++) {
        while (countArray[i]--) {
            array[j++] = i;
        }
    }
}
