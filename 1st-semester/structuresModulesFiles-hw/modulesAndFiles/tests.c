#include <stdbool.h>
#include <stdlib.h>

#define TEST_ARRAY_SIZE 50

bool testMostFrequentEveryElementOccursOnce(int (*findMostFrequent)(const int*, int)) {
    int range[TEST_ARRAY_SIZE] = {0};
    for (int i = 0; i < TEST_ARRAY_SIZE; i++) {
        range[i] = i - TEST_ARRAY_SIZE / 2;
    }
    int result = findMostFrequent(range, TEST_ARRAY_SIZE);

    return (-TEST_ARRAY_SIZE / 2 <= result) && (result <= TEST_ARRAY_SIZE - TEST_ARRAY_SIZE / 2);
}

bool testMostFrequentEveryElementEquals(int (*findMostFrequent)(const int*, int)) {
    int range[TEST_ARRAY_SIZE] = {0};
    int randomElement = rand();
    for (int i = 0; i < TEST_ARRAY_SIZE; range[i++] = randomElement);

    return randomElement == findMostFrequent(range, TEST_ARRAY_SIZE);
}
bool testMostFrequentArrayOfOne(int (*findMostFrequent)(const int*, int)) {
    int array[1] = {rand()};
    return array[0] == findMostFrequent(array, 1);
}
