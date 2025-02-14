#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <wchar.h>

#include "ioAuxiliaries.h"
#include "list.h"

typedef struct phoneBookEntry {
    wchar_t* name;
    wchar_t* number;
} phoneBookEntry;

void phoneBookEntryDestructor(void* data) {
    phoneBookEntry* entry = (phoneBookEntry*)data;
    free(entry->name);
    free(entry->number);
    free(entry);
}

static bool splitEntryNameNumber(char* string, char separator, wchar_t** first, wchar_t** second) {
    *first = NULL;
    *second = NULL;
    size_t stringLen = strlen(string);
    size_t midIndex = 0;
    for (; midIndex < stringLen && string[midIndex] != separator; midIndex++);
    if (midIndex == stringLen) {
        return false;
    }

    // remove trailing whitespaces in first substring
    size_t firstLen = midIndex - 1;
    for (; string[firstLen] == ' ' || string[firstLen] == '\t'; firstLen--) {
        string[firstLen] = '\0';
        if (firstLen == 0) {
            break;
        }
    }
    firstLen++;

    *first = malloc((firstLen + 1) * sizeof(wchar_t));
    if (*first == NULL) {
        return false;
    }
    mbstowcs(*first, string, firstLen + 1);
    (*first)[firstLen] = L'\0';

    // remove leading whitespaces in second substring
    size_t secondStart = midIndex + 1;
    for (; secondStart < stringLen; secondStart++) {
        if (string[secondStart] != ' ' && string[secondStart] != '\t') {
            break;
        }
    }
    size_t secondLen = stringLen - secondStart;

    *second = malloc((secondLen + 1) * sizeof(wchar_t));
    if (*second == NULL) {
        free(*first);
        *first = NULL;
        return false;
    }
    mbstowcs(*second, string + secondStart, secondLen + 1);

    return true;
}

List* readEntriesToList(const char* filename) {
    List* list = listInitWithDestructor(&phoneBookEntryDestructor);
    FILE* entries = fopen(filename, "r");
    if (entries == NULL) {
        listFree(&list);
        return NULL;
    }
    char* entryLine = NULL;
    int error = 0;
    while (fReadLine(entries, &entryLine, &error) > 0) {
        wchar_t* name = NULL;
        wchar_t* number = NULL;
        phoneBookEntry* entry = NULL;

        if (splitEntryNameNumber(entryLine, '-', &name, &number)) {
            entry = malloc(sizeof(phoneBookEntry));
            if (entry == NULL) {
                fclose(entries);
                listFree(&list);
                return NULL;
            }

            entry->name = name;
            entry->number = number;
            listAppend(list, (void*)entry);
        }

        free(entryLine);
    }

    fclose(entries);
    return list;
}

bool phoneBookEntryComparatorNumber(void* dataL, void* dataR) {
    return wcscmp(((phoneBookEntry*)dataL)->number, ((phoneBookEntry*)dataR)->number) <= 0;
}

bool phoneBookEntryComparatorName(void* dataL, void* dataR) {
    return wcscmp(((phoneBookEntry*)dataL)->name, ((phoneBookEntry*)dataR)->name) <= 0;
}

void phoneBookEntryPrint(void* entry) {
    printf("%ls - %ls\n", ((phoneBookEntry*)entry)->name, ((phoneBookEntry*)entry)->number);
}

void phoneBookEntryFPrint(FILE* stream, void* entry) {
    fprintf(stream, "%ls - %ls\n", ((phoneBookEntry*)entry)->name, ((phoneBookEntry*)entry)->number);
}
