#ifndef SORTING_TESTS
#define SORTING_TESTS
bool testSortingArrayOfOne(void (*sort)(int*, int));
bool testSortingLargeArray(void (*sort)(int*, int));
bool testSortingAllEqual(void (*sort)(int*, int));
bool testSortingSorted(void (*sort)(int*, int));
bool testSortingReverseSorted(void (*sort)(int*, int));
bool isSorted(int* array, int size);
#endif