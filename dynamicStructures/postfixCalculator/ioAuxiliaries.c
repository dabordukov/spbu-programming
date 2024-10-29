#include "ioAuxiliaries.h"

#include <stdbool.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

void flushSTDIN() {
    int c;
    while ((c = getchar()) != '\n' && c != EOF);
}

size_t freadLineN(FILE* stream, char* string, size_t size) {
    size_t count = 0;
    size--;  // leave one byte for terminator
    int c = EOF;

    while (size-- && (c = fgetc(stream)) != '\n' && c != EOF) {
        if (c == '\r') {
            size++;
            continue;
        }

        string[count++] = c;
    }
    string[count] = 0;  // terminator

    if (stream == stdin && c != '\n' && c != EOF) {
        flushSTDIN();
    }

    return count;
}

size_t readLineN(char* string, size_t size) {
    return freadLineN(stdin, string, size);
}

static void* checkedRealloc(void* ptr, size_t size, int* error) {
    *error = 0;

    void* p = realloc(ptr, size);
    if (!p) {
        *error = 1;
    }

    return p;
}

#define BUFFER_SIZE 8
size_t freadLine(FILE* stream, char** string, int* error) {
    *error = 0;
    char buffer[BUFFER_SIZE] = {0};
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

        // memcpy(data + dataSize, buffer, chunkLen);
        strncpy(data + dataSize, buffer, chunkLen);
        dataSize += chunkLen;
        countBlocks++;
        if (buffer[chunkLen - 1] == '\n') {
            buffer[chunkLen - 1] = 0;
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
    return freadLine(stdin, string, error);
}