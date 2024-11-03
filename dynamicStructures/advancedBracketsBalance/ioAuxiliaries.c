#include "ioAuxiliaries.h"

#include <stdbool.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

void flushSTDIN(void) {
    int c = EOF;
    while ((c = getchar()) != '\n' && c != EOF);
}

static void* checkedRealloc(void* ptr, size_t size, int* error) {
    *error = 0;

    void* p = realloc(ptr, size);
    if (p == NULL) {
        *error = 1;
    }

    return p;
}

#define BUFFER_SIZE 8
size_t fReadLine(FILE* stream, char** string, int* error) {
    *error = 0;
    char buffer[BUFFER_SIZE] = {'\0'};
    int dataSize = 0;
    char* data = NULL;
    int errorRealloc = 0;

    int countBlocks = 0;
    while (fgets(buffer, BUFFER_SIZE, stream) != NULL) {
        int chunkLen = strlen(buffer);
        data = checkedRealloc(data, dataSize + chunkLen + 1, &errorRealloc);
        if (errorRealloc) {
            break;
        }

        memcpy(data + dataSize, buffer, chunkLen);
        dataSize += chunkLen;
        countBlocks++;
        if (data[dataSize - 1] == '\n') {
            data[dataSize - 1] = '0';
            break;
        }
    }

    if (data == NULL) {
        *error = 1;
    }

    *string = data;
    return dataSize - 1;
}

size_t readLine(char** string, int* error) {
    return fReadLine(stdin, string, error);
}