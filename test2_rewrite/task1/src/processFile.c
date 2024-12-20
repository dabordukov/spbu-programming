#include <stdint.h>
#include <stdio.h>
#include <stdlib.h>

#include "list.h"

void nothing(void* _) {
    return;
};

int processFile(char* inputFilename, char* outputFilename, int a, int b) {
    List* list = listInitWithDestructor(&nothing);

    FILE* inFile = fopen(inputFilename, "r");
    if (inFile == NULL) {
        free(list);
        return 1;
    }

    FILE* outFile = fopen(outputFilename, "w");
    if (outFile == NULL) {
        free(list);
        fclose(inFile);
        return 2;
    }

    int read = 0;
    intptr_t number = 0;
    while ((read = fscanf(inFile, "%ld", &number)) > 0) {
        listAppend(list, (void*)number);
    }
    fclose(inFile);

    ListPosition* pos = listFirst(list);
    while (pos != NULL) {
        number = (intptr_t)listPosGetData(pos);
        ListPosition* toRemove = pos;
        pos = listNextNode(pos);
        if (number < a) {
            listRemoveNode(list, &toRemove);
            fprintf(outFile, "%ld ", number);
        }
    }

    pos = listFirst(list);
    while (pos != NULL) {
        number = (intptr_t)listPosGetData(pos);
        ListPosition* toRemove = pos;
        pos = listNextNode(pos);
        if (number <= b) {
            listRemoveNode(list, &toRemove);
            fprintf(outFile, "%ld ", number);
        }
    }

    pos = listFirst(list);
    while (pos != NULL) {
        number = (intptr_t)listPosGetData(pos);
        fprintf(outFile, "%ld ", number);
        listRemoveNode(list, &pos);
        pos = listFirst(list);
    }

    fclose(outFile);
    listFree(&list);
    return 0;
}