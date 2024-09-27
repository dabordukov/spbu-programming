#include <stdbool.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <time.h>

#define N 100000
#define RANGE_MAX 10000

void swap(int* a, int* b) {
    int temp = *a;
    *a = *b;
    *b = temp;
}

void bubbleSort(int* array, int size) {
    for (int i = 1; i < size - 1; i++) {
        for (int j = i; j >= 0; j--) {
            if (array[j] > array[j + 1]) {
                swap(array + j, array + j + 1);
            }
        }
    }
}

void printArray(int* array, int size) {
    for (int i = 0; i < size; i++) {
        printf("%d ", array[i]);
    }
    puts(" ");
}

void countSort(int* array, int size) {
    int countArray[RANGE_MAX] = {0};
    for (int i = 0; i < size; i++) {
        countArray[array[i]]++;
    }

    for (int i = 0, j = 0; i < RANGE_MAX; i++) {
        while (countArray[i]--) {
            array[j++] = i;
        }
    }
}

void fillRand(int* array, int size) {
    for (int i = 0; i < size; i++) {
        array[i] = rand() % RANGE_MAX;
    }
}

int main() {
    srand(time(NULL));
    int* arrayBubbleSort = malloc(N * sizeof(int));
    if (arrayBubbleSort == NULL) {
        printf("Can't allocate array!");
        return 1;
    }
    fillRand(arrayBubbleSort, N);

    int* arrayCountSort = malloc(N * sizeof(int));
    if (arrayCountSort == NULL) {
        printf("Can't allocate array!");
        return 1;
    }
    memcpy(arrayCountSort, arrayBubbleSort, N * sizeof(int));

    clock_t timeBegin, timeEnd;
    timeBegin = clock();
    bubbleSort(arrayBubbleSort, N);
    timeEnd = clock();
    printf("%.3fs bubbleSort\n", (double)(timeEnd - timeBegin) / CLOCKS_PER_SEC);

    timeBegin = clock();
    countSort(arrayCountSort, N);
    timeEnd = clock();
    printf("%.0fns countSort\n", (double)(timeEnd - timeBegin) / CLOCKS_PER_SEC * 1e6);

    free(arrayBubbleSort);
    free(arrayCountSort);
}
