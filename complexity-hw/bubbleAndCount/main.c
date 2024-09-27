#include <stdbool.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <time.h>

#include "auxiliaries.h"
#include "sorts.h"
#define N 100000
#define RANGE_MAX COUNT_SORT_RANGE_MAX

int main() {
    srand(time(NULL));
    int* arrayBubbleSort = malloc(N * sizeof(int));
    if (arrayBubbleSort == NULL) {
        printf("Can't allocate array!");
        return 1;
    }
    fillArrayRand(arrayBubbleSort, N, RANGE_MAX);

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
