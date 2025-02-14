#include "ioAuxiliaries.h"

#include <stdbool.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

void flushSTDIN(void) {
    int c = EOF;
    while ((c = getchar()) != '\n' && c != EOF);
}

#define BUFFER_SIZE 4096
size_t fReadLine(FILE* stream, char** string) {
    char buffer[BUFFER_SIZE] = {'\0'};
    int dataLen = 0;
    char* data = NULL;

    while (fgets(buffer, BUFFER_SIZE, stream) != NULL) {
        int chunkLen = strlen(buffer);
        char* newData = realloc(data, dataLen + chunkLen + 1);
        if (newData == NULL) {
            break;
        }
        data = newData;
        memcpy(data + dataLen, buffer, chunkLen);
        dataLen += chunkLen;
        if (data[dataLen - 1] == '\n') {
            data[dataLen - 1] = '\0';
            dataLen--;
            break;
        } else {
            data[dataLen] = '\0';
        }
    }

    *string = data;
    return dataLen;
}

size_t readLine(char** string) {
    return fReadLine(stdin, string);
}
