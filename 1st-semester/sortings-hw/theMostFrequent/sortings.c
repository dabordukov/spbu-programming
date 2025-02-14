#include <stdlib.h>

#include "auxiliaries.h"
void insertionSort(int* array, int size) {
    for (int i = 1; i < size; i++) {
        for (int j = i - 1; j >= 0; j--) {
            if (array[j] > array[j + 1]) {
                swap(array + j, array + j + 1);
            }
        }
    }
}

typedef struct {
    int left;
    int right;
} Borders;

// returns struct {int left; int right} of left border and right border of subarray with elements that equal pivot
Borders partition(int* array, int size) {
    Borders borders = {0, size - 1};
    int pivot = array[rand() % size];

    int left = 0, right = size - 1;
    while (left < right) {
        while (left < right && array[left] < pivot) {
            left++;
        }
        while (left < right && array[right] >= pivot) {
            right--;
        }

        if (left >= right) {
            break;
        }
        swap(array + left, array + right);
    }
    borders.left = right;

    left = right;
    right = size - 1;

    while (left < right) {
        while (left < right && array[right] > pivot) {
            right--;
        }
        while (left < right && array[left] == pivot) {
            left++;
        }

        if (left >= right) {
            break;
        }
        swap(array + left, array + right);
    }

    borders.right = left;
    return borders;
}

void qSort(int* array, int size) {
    if (size < 2) {
        return;
    }
    if (size < 10) {
        insertionSort(array, size);
    } else {
        Borders equal = partition(array, size);
        qSort(array, equal.left);
        qSort(array + equal.right + 1, size - equal.right - 1);
    }
}
