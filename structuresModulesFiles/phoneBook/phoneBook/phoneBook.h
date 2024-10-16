#ifndef PHONEBOOK_H
#define PHONEBOOK_H 1
#include <stdbool.h>
#include <stdio.h>
#include <wchar.h>

#define PHONEBOOK_MAX_ENTRIES (size_t)100
#define PHONEBOOK_NUMBER_LENGTH_MAX 15
#define PHONEBOOK_NAME_LENGTH_MAX 50

typedef struct phoneBookEntry {
    char number[PHONEBOOK_NUMBER_LENGTH_MAX];
    wchar_t name[PHONEBOOK_NAME_LENGTH_MAX];
} phoneBookEntry;

struct phoneBook {
    phoneBookEntry entries[PHONEBOOK_MAX_ENTRIES];
    int size;
    bool saved;
};

typedef struct phoneBook phoneBook;

bool isSavedPhoneBook(phoneBook* book);
bool isFullPhoneBook(phoneBook* book);
bool isValidPhoneNumber(char* number);
void phoneBookPrint(phoneBook* book);
void phoneBookFPrint(FILE* stream, phoneBook* book);
void phoneBookEntryPrint(phoneBookEntry* entry, int spaces);
void phoneBookEntryFPrint(FILE* stream, phoneBookEntry* entry, int spaces);
int phoneBookAddEntry(phoneBook* book, wchar_t* name, char* number);
int levenshteinDistanceNamePhoneBook(wchar_t* name, wchar_t* search);
int findSimilarNamesPhoneBook(phoneBook* book, wchar_t* name, int* results);
int findNumberPhoneBook(phoneBook* book, char* number, int* results);
int loadPhoneBook(phoneBook* book, const char* phoneBookDatabase);
int savePhoneBook(phoneBook* book, const char* phoneBookDatabase);
#endif