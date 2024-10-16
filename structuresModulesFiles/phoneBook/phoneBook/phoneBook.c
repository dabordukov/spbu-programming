#include "phoneBook.h"

#include <math.h>
#include <stdbool.h>
#include <stdio.h>
#include <string.h>
#include <wchar.h>

#include "auxiliaries.h"

#define MIN(x, y) (((x) < (y)) ? (x) : (y))

const char bookEntryPrintFormat[] = "%ls\n%s     \n\n";

bool isDigit(char c) {
    return '0' <= c && c <= '9';
}

// returns 1 on full phoneBook
int phoneBookAddEntry(phoneBook* book, wchar_t* name, char* number) {
    if (book->size == PHONEBOOK_MAX_ENTRIES) {
        return 1;
    }

    wcsncpy(book->entries[book->size].name, name, PHONEBOOK_NAME_LENGTH_MAX);
    strncpy(book->entries[book->size].number, number, PHONEBOOK_NUMBER_LENGTH_MAX);

    book->size++;
    book->saved = false;

    return 0;
}

bool isFullPhoneBook(phoneBook* book) {
    return book->size == PHONEBOOK_MAX_ENTRIES;
}

bool isSavedPhoneBook(phoneBook* book) {
    return book->saved;
}

int levenshteinDistanceNamePhoneBook(wchar_t* name, wchar_t* search) {
    wchar_t* string1 = name;
    wchar_t* string2 = search;

    int string1Len = wcslen(name) + 1;
    int string2Len = wcslen(search) + 1;

    if (string1Len > string2Len) {
        swapPtr((void*)&string1, (void*)&string2);
        swapInt(&string1Len, &string2Len);
    }  // string2Len >= string1Len

    int* levenshteinMatrix = checkedCalloc(2 * (string1Len), sizeof(int));

    for (int i = 0; i < string2Len; i++) {
        for (int j = 0; j < string1Len; j++) {
            if (i == 0 && j == 0) {
                continue;
            }

            if (j == 0) {
                levenshteinMatrix[i % 2 * string1Len] = i;
                continue;
            }

            if (i == 0) {
                levenshteinMatrix[j] = j;
                continue;
            }

            int add = levenshteinMatrix[string1Len * ((i - 1) % 2) + j] + 1;
            int remove = levenshteinMatrix[string1Len * (i % 2) + j - 1] + 1;
            int change = levenshteinMatrix[string1Len * ((i - 1) % 2) + j - 1] + ((string1[j - 1] != string2[i - 1]) ? 1 : 0);
            levenshteinMatrix[i % 2 * string1Len + j] = MIN(add, MIN(remove, change));
        }
    }
    int levenshteinDistance = levenshteinMatrix[(string2Len - 1) % 2 * string1Len + string1Len - 1];

    free(levenshteinMatrix);
    return levenshteinDistance;
}

int findSimilarNamesPhoneBook(phoneBook* book, wchar_t* name, int* results) {
    int count = 0;
    for (int i = 0; i < book->size; i++) {
        int distance = levenshteinDistanceNamePhoneBook(book->entries[i].name, name);
        if (wcslen(book->entries[i].name) > distance * 4) {
            results[count++] = i;
        }
    }
    return count;
}

bool isValidPhoneNumber(char* number) {
    int i = 0;
    int len = strlen(number);
    if (number[0] == '+') {
        i++;
    }
    for (; i < len; i++) {
        if (!isDigit(number[i]) && number[i] != ' ') {
            return false;
        }
    }
    return true;
}

bool isEqualNumbers(char* number1, char* number2) {
    char* ptr1 = number1;
    char* ptr2 = number2;

    while (*ptr1 != 0 || *ptr2 != 0) {
        while (*ptr1 != 0 && !isDigit(*ptr1)) {
            ptr1++;
        }
        while (*ptr2 != 0 && !isDigit(*ptr2)) {
            ptr2++;
        }
        if (*ptr1 != *ptr2) {
            return false;
        }

        if (isDigit(*ptr1)) {
            ptr1++;
        }

        if (isDigit(*ptr2)) {
            ptr2++;
        }
    }
    return true;
}

int findNumberPhoneBook(phoneBook* book, char* number, int* results) {
    int count = 0;
    for (int i = 0; i < book->size; i++) {
        if (isEqualNumbers(number, book->entries[i].number)) {
            results[count++] = i;
        }
    }
    return count;
}

void phoneBookEntryFPrint(FILE* stream, phoneBookEntry* entry, int spaces) {
    fprintf(stream, "%ls\n", entry->name);
    while (spaces--) {
        putc(' ', stream);
    }
    fprintf(stream, "%s\n\n", entry->number);
}

void phoneBookEntryPrint(phoneBookEntry* entry, int spaces) {
    phoneBookEntryFPrint(stdout, entry, spaces);
}

void phoneBookFPrint(FILE* stream, phoneBook* book) {
    if (book->size == 0) {
        fprintf(stream, "Записей нет.\n");
        return;
    }

    for (int i = 0; i < book->size; i++) {
        fprintf(stream, "%d) ", i + 1);
        phoneBookEntryFPrint(stream, &book->entries[i], (int)log10(book->size) + 3);
    }
}

void phoneBookPrint(phoneBook* book) {
    phoneBookFPrint(stdout, book);
}

int loadPhoneBook(phoneBook* book, const char* phoneBookDatabase) {
    FILE* storage = fopen(phoneBookDatabase, "r");
    if (storage == NULL) {
        return 1;
    }

    int size = fread(book, sizeof(phoneBook), 1, storage);
    fclose(storage);
    if (size != 1 || !book->saved) {
        fprintf(stderr, "Файл поврежден.\n");
        book->saved = false;
        book->size = 0;
        return 2;
    }
    return 0;
}

int savePhoneBook(phoneBook* book, const char* phoneBookDatabase) {
    FILE* storage = fopen(phoneBookDatabase, "w");
    if (storage == NULL) {
        fprintf(stderr, "Can't access file %s\n", phoneBookDatabase);
        return 2;
    }
    book->saved = true;
    int size = fwrite(book, sizeof(phoneBook), 1, storage);
    fclose(storage);
    if (size != 1) {
        fprintf(stderr, "Can't save file\n");
        book->saved = false;
        return 1;
    }

    return 0;
}