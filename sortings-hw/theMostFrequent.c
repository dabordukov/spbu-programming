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

#define TEST_ARRAY_SIZE 50
bool testMostFrequentEveryElementOccursOnce() {
    int range[TEST_ARRAY_SIZE] = {0};
    for (int i = 0; i < TEST_ARRAY_SIZE; i++) {
        range[i] = i - TEST_ARRAY_SIZE / 2;
    }
    int result = findMostFrequent(range, TEST_ARRAY_SIZE);

    return (-TEST_ARRAY_SIZE / 2 <= result) && (result <= TEST_ARRAY_SIZE - TEST_ARRAY_SIZE / 2);
}

bool testMostFrequentEveryElementEquals() {
    int range[TEST_ARRAY_SIZE] = {0};
    int randomElement = rand();
    for (int i = 0; i < TEST_ARRAY_SIZE; range[i++] = randomElement);

    return randomElement == findMostFrequent(range, TEST_ARRAY_SIZE);
}
bool testMostFrequentArrayOfOne() {
    int array[1] = {rand()};
    return array[0] == findMostFrequent(array, 1);
}

void allTests() {
    assert(testMostFrequentEveryElementOccursOnce() == true);
    assert(testMostFrequentEveryElementEquals() == true);
    assert(testMostFrequentArrayOfOne() == true);
}

int main() {
    srand(time(NULL));
    allTests();

    int exampleArray[50] = {0};
    fillRand(exampleArray, 50);
    printf("Array:\n");
    printArray(exampleArray, 50);
    printf("Most frequent element: %d\n", findMostFrequent(exampleArray, 50));
}