#include <assert.h>
#include <stdbool.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <time.h>

void swap(int* a, int* b) {
    int temp = *a;
    *a = *b;
    *b = temp;
}

void printArray(const int* array, int size) {
    for (int i = 0; i < size; i++) {
        printf("%d ", array[i]);
    }
    puts("");
}

void fillRand(int* array, int size) {
    for (int i = 0; i < size; i++) {
        array[i] = rand() % 100;
    }
}

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
    int t = rand() % size;
    int pivot = array[t];

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

#define TEST_ARRAY_SIZE 1000
bool testSortingAllEqual(void (*sort)(int*, int)) {
    int allEqualArray[TEST_ARRAY_SIZE] = {0};
    for (int i = 0; i < TEST_ARRAY_SIZE; allEqualArray[i++] = 2);
    sort(allEqualArray, TEST_ARRAY_SIZE);
    return isSorted(allEqualArray, TEST_ARRAY_SIZE);
}

bool testSortingSorted(void (*sort)(int*, int)) {
    int sortedArray[TEST_ARRAY_SIZE] = {0};
    for (int i = 0; i < TEST_ARRAY_SIZE; i++) {
        sortedArray[i] = i - TEST_ARRAY_SIZE / 2;
    }
    sort(sortedArray, TEST_ARRAY_SIZE);

    return isSorted(sortedArray, TEST_ARRAY_SIZE);
}

bool testSortingReverseSorted(void (*sort)(int*, int)) {
    int reverseSortedArray[TEST_ARRAY_SIZE] = {0};
    for (int i = 0; i < TEST_ARRAY_SIZE; i++) {
        reverseSortedArray[i] = -i + TEST_ARRAY_SIZE / 2;
    }
    sort(reverseSortedArray, TEST_ARRAY_SIZE);

    return isSorted(reverseSortedArray, TEST_ARRAY_SIZE);
}

#define LARGE_ARRAY_SIZE 10000000
bool testSortingLargeArray(void (*sort)(int*, int)) {
    int* largeArray = malloc(LARGE_ARRAY_SIZE * sizeof(int));
    fillRand(largeArray, LARGE_ARRAY_SIZE);
    sort(largeArray, LARGE_ARRAY_SIZE);
    bool result = isSorted(largeArray, LARGE_ARRAY_SIZE);
    free(largeArray);
    return result;
}

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
    fillRand(exampleArray, 50);
    printArray(exampleArray, 50);
    qSort(exampleArray, 50);
    printf("Sorted:\n");
    printArray(exampleArray, 50);
}