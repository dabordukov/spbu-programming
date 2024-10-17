#include <assert.h>
#include <stdio.h>
#include <string.h>

#include "../ioAuxiliaries.h"
#include "../phoneBook.h"

#define JOIN(x, y) x #y

#define PHONEBOOK_TESTS_DIR "phoneBook/tests/"
#define DB_LOAD_TEST_PATH JOIN(PHONEBOOK_TESTS_DIR, dbLoad.test)
bool testLoadDatabase() {
    phoneBook book;
    char names[][PHONEBOOK_NAME_LENGTH_MAX] = {"qwertyuiop", "тесттест тест"};
    char numbers[][PHONEBOOK_NUMBER_LENGTH_MAX] = {"123456789", "+0000 222"};

    if (loadPhoneBook(&book, DB_LOAD_TEST_PATH)) {
        return false;
    }

    for (int i = 0; i < book.size; i++) {
        if (strncmp(book.entries[i].name, names[i], PHONEBOOK_NAME_LENGTH_MAX) != 0 || strncmp(book.entries[i].number, numbers[i], PHONEBOOK_NUMBER_LENGTH_MAX) != 0) {
            return false;
        }
    }

    return true;
}

#define DB_ADD_ENTRY_TEST_PATH JOIN(PHONEBOOK_TESTS_DIR, addEntry.test)
bool testAddEntry() {
    phoneBook book;
    // loadPhoneBook(&book, DB_LOAD_TEST_PATH);
    char name[] = "тесттесттест1234";
    char number[] = "000000000000";
    phoneBookAddEntry(&book, name, number);

    if (strncmp(book.entries[book.size - 1].name, name, PHONEBOOK_NAME_LENGTH_MAX) != 0 ||
        strncmp(book.entries[book.size - 1].number, number, PHONEBOOK_NUMBER_LENGTH_MAX) != 0) {
        return false;
    }

    return true;
}

#define DB_SAVE_TEST_PATH DB_ADD_ENTRY_TEST_PATH
#define DB_SAVE_CURR_DB JOIN(PHONEBOOK_TESTS_DIR, db.dat)
bool testSaveDatabase() {
    phoneBook book;

    loadPhoneBook(&book, DB_LOAD_TEST_PATH);

    char name[] = "тесттесттест1234";
    char number[] = "000000000000";
    phoneBookAddEntry(&book, name, number);
    savePhoneBook(&book, DB_SAVE_CURR_DB);

    FILE* db = fopen(DB_SAVE_CURR_DB, "r");
    if (db == NULL) {
        fprintf(stderr, "Error: Can't open %s for reading.\n", DB_SAVE_CURR_DB);
        return false;
    }

    int size = 0;
    if (fscanf(db, "%d\n", &size) != 1 || size != book.size) {
        return false;
    }

    for (int i = 0; i < size; i++) {
        if (freadLineN(db, name, PHONEBOOK_NAME_LENGTH_MAX) == 0 ||
            freadLineN(db, number, PHONEBOOK_NUMBER_LENGTH_MAX) == 0) {
            return false;
        }

        if (strncmp(name, book.entries[i].name, PHONEBOOK_NAME_LENGTH_MAX) != 0 ||
            strncmp(number, book.entries[i].number, PHONEBOOK_NUMBER_LENGTH_MAX) != 0) {
            return false;
        }
    }

    fclose(db);
    remove(DB_SAVE_CURR_DB);

    return true;
}

#define SEARCH_BY_NAME_TEST_PATH JOIN(PHONEBOOK_TESTS_DIR, nameSearch.test)
bool testSearchByName() {
    phoneBook book;
    loadPhoneBook(&book, SEARCH_BY_NAME_TEST_PATH);

    int findResults[PHONEBOOK_MAX_ENTRIES];
    int entriesCount = findSimilarNamesPhoneBook(&book, "тест", findResults);
    if (!(entriesCount == 3 && findResults[0] == 0 && findResults[1] == 1 && findResults[2] == 4)) {
        return false;
    }

    entriesCount = findSimilarNamesPhoneBook(&book, "test12345", findResults);
    if (!(entriesCount == 1 && findResults[0] == 3))
        return false;

    return true;
}

#define SEARCH_BY_PHONE_NUMBER_TEST_PATH JOIN(PHONEBOOK_TESTS_DIR, phoneNumberSearch.test)
bool testSearchByPhoneNumber() {
    phoneBook book;
    loadPhoneBook(&book, SEARCH_BY_PHONE_NUMBER_TEST_PATH);

    char number[] = "12345678";
    int findResults[PHONEBOOK_MAX_ENTRIES];
    int entriesCount = findNumberPhoneBook(&book, number, findResults);
    if (!(entriesCount == 2 && findResults[0] == 0 && findResults[1] == 5)) {
        return false;
    }
    return true;
}

int runTest() {
    assert(testLoadDatabase());
    assert(testAddEntry());
    assert(testSaveDatabase());
    assert(testSearchByName());
    assert(testSearchByPhoneNumber());

    return 0;
}