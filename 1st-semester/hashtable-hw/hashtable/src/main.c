#include <stdio.h>

#include "hashtable.h"
#include "ioAuxiliaries.h"

int main(int argc, char* argv[]) {
    char* filename = NULL;
    if (argc > 1) {
        filename = argv[1];
    } else {
        printf("Enter filename: ");
        int error = 0;
        readLine(&filename, &error);
        if (error != 0) {
            printf("Not enough memory");
            return 1;
        }
    }

    FILE* file = fopen(filename, "r");
    if (file == NULL) {
        fprintf(stderr, "File %s not found!\n", filename);
        return 2;
    }

    Hashtable* words = countWordFrequencies(file);
    char const** keys = hashtableKeys(words);

    for (size_t i = 0; i < hashtableSize(words); i++) {
        int error = 0;
        printf("%s - %lu\n", keys[i], hashtableGet(words, keys[i], &error));
    }

    fclose(file);
}