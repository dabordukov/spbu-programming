#include "ioAuxiliaries.h"

#include <stdbool.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

void flushSTDIN(void) {
    int c = EOF;
    while ((c = getchar()) != '\n' && c != EOF);
}

void clearWhitespaces(FILE* stream) {
    int c = EOF;
    while ((c = getc(stream)) != EOF) {
        if (c != '\t' && c != ' ') {
            ungetc(c, stream);
            break;
        }
    }
}

#define BUFFER_SIZE 4096
size_t fReadLine(FILE* stream, char** string, int* error) {
    *error = 0;
    char buffer[BUFFER_SIZE] = {'\0'};
    int dataSize = 0;
    char* data = NULL;

    while (fgets(buffer, BUFFER_SIZE, stream) != NULL) {
        int chunkLen = strlen(buffer);
        char* newData = realloc(data, dataSize + chunkLen + 1);
        if (newData == NULL) {
            break;
        }
        data = newData;
        memcpy(data + dataSize, buffer, chunkLen);
        dataSize += chunkLen;
        if (data[dataSize - 1] == '\n') {
            data[dataSize - 1] = '\0';
            break;
        } else {
            data[dataSize] = '\0';
        }
    }

    if (data == NULL) {
        *error = 1;
    }

    *string = data;
    return dataSize;
}

size_t readLine(char** string, int* error) {
    return fReadLine(stdin, string, error);
}
