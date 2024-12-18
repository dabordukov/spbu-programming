#include <assert.h>
#include <stdio.h>
#include <stdlib.h>

#include "hashtable.h"

void testCaseMoreThanInitialCapacity() {
    Hashtable* ht = hashtableInit();
    for (int i = 32; i < 127; i++) {
        char key[2] = {};
        sprintf(key, "%c", i);
        hashtableUpdate(ht, key, i);
    }

    for (int i = 32; i < 127; i++) {
        char key[2] = {};
        sprintf(key, "%c", i);

        int error = 0;
        size_t oldValue = hashtableGet(ht, key, &error);
        assert(error == 0);

        hashtableUpdate(ht, key, oldValue + 1);

        size_t updatedValue = hashtableGet(ht, key, &error);
        assert(error == 0);
        assert(updatedValue == oldValue + 1);
    }

    hashtableFree(&ht);
}

void testWordCounter() {
    FILE* file = fopen(HASHTABLE_WORD_COUNTER_TEST_FILENAME, "r");
    assert(file != NULL);
    Hashtable* words = countWordFrequencies(file);
    fclose(file);

    FILE* testFile = fopen(HASHTABLE_WORD_COUNTER_TEST_RESULT_FILENAME, "r");
    assert(testFile != NULL);
    char buf[50] = {0};
    size_t value = 0;
    while (fscanf(testFile, "%s - %lu", buf, &value) == 2) {
        int error = 0;
        assert(hashtableGet(words, buf, &error) == value);
    }
}

int main() {
    testCaseMoreThanInitialCapacity();
    testWordCounter();
}