#include <stdio.h>
#include <stdlib.h>

#include "auxiliaries.h"
#include "sortings.h"
#include "tests.h"

#define RANGE_MAX 50

// array must be sorted
bool binarySearch(int* array, int size, int value) {
    int left = 0, right = size - 1;
    int middle = 0;
    while (left <= right) {
        middle = (left + right) / 2;
        if (value == array[middle]) {
            return true;
        }

        if (value < array[middle]) {
            right = middle - 1;
        } else {
            left = middle + 1;
        }
    }
    return false;
}

int main() {
    testBinarySearch(&binarySearch);
    printf("Tests are passed\n");

    int n = 0,
        k = 0;

    printf("Enter n: ");
    while (scanf("%d", &n) != 1) {
        scanf("%*[^\n]");
        printf("Enter integer n>0: ");
    }

    printf("Enter k: ");
    while (scanf("%d", &k) != 1) {
        scanf("%*[^\n]");
        printf("Enter integer k>0: ");
    }

    int* array = malloc(n * sizeof(int));
    int* numbersToCheck = malloc(k * sizeof(int));

    fillArrayRand(array, n, RANGE_MAX);
    fillArrayRand(numbersToCheck, k, RANGE_MAX);

    printf("array:\n");
    printArray(array, n);

    qSort(array, n);

    for (int i = 0; i < k; i++) {
        if (binarySearch(array, n, numbersToCheck[i])) {
            printf("[+] %d is in array\n", numbersToCheck[i]);
        } else {
            printf("[-] %d is not in array\n", numbersToCheck[i]);
        }
    }
    puts("");
}