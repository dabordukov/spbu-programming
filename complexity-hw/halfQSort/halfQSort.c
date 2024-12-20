#include <limits.h>
#include <stdio.h>
#include <stdlib.h>
#include <time.h>

#include "auxiliaries.h"

#define N 20

int halfQSort(int* array, int size) {
    int pivot = array[0];

    int left = 0, right = N - 1;
    while (left < right) {
        while (array[left] < pivot) {
            left++;
        }
        while (array[right] >= pivot) {
            right--;
        }

        if (left >= right) {
            break;
        }

        swap(array + left, array + right);
    }
    return right;
}

int main() {
    srand(time(NULL));

    int* numbers = malloc(N * sizeof(int));
    if (numbers == NULL) {
        printf("Can't allocate memory\n");
        return 1;
    }
    fillArrayRand(numbers, N, 500);

    printf("Initial array:\n");
    printArray(numbers, N);
    printf("Pivot: %d\n\n", numbers[0]);

    int partitionPoint = halfQSort(numbers, N);
    printf("Half-QSorted:\n");
    printf("< pivot: ");
    printArray(numbers, partitionPoint);
    printf(">= pivot: ");
    printArray(numbers + partitionPoint + 1, N - partitionPoint - 1);

    free(numbers);
}