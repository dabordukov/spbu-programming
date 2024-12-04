#include <stdbool.h>
#include <stdio.h>

#include "tree.h"

bool sortAscending(int left, int right) {
    return left < right;
}

int main() {
    int array[11] = {5, 2, 7, 9, 1, 6, 1, 112, -124, -5, -25};
    printf("Before: ");
    for (int i = 0; i < 11; i++) {
        printf("%d ", array[i]);
    }
    printf("\n");

    treeSort(array, sizeof(array) / sizeof(int), &sortAscending);
    printf("After: ");
    for (int i = 0; i < 11; i++) {
        printf("%d ", array[i]);
    }
    printf("\n");
}