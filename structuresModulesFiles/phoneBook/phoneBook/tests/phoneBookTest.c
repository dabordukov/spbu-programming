#include <assert.h>
#include <stdio.h>
#include <string.h>

#include "../phoneBook.h"

#define JOIN(x, y) x #y

#define PHONEBOOK_TESTS_DIR "phoneBook/tests/"
#define DB_LOAD_TEST_PATH JOIN(PHONEBOOK_TESTS_DIR, dbLoad.test)
bool testLoadDatabase() {
    phoneBook book;
    wchar_t names[][PHONEBOOK_NAME_LENGTH_MAX] = {L"qwertyuiop", L"йцукенгшщд"};
    char numbers[][PHONEBOOK_NUMBER_LENGTH_MAX] = {"123456789", " 0 2"};

    loadPhoneBook(&book, DB_LOAD_TEST_PATH);

    FILE* test = fopen(DB_LOAD_TEST_PATH, "r");
    if (test == NULL) {
        fprintf(stderr, "Error: Can't open %s for reading.\n", DB_LOAD_TEST_PATH);
        return false;
    }
    bool passed = true;

    // compare bytes of file and structure
    char byte[1];
    for (int i = 0; i < sizeof(phoneBook); i++) {
        if (fread(byte, 1, 1, test) != 1 || byte[0] != ((char*)&book)[i]) {
            passed = false;
            break;
        }
    }
    // if the file's size is bigger than the structure's
    if (fread(byte, 1, 1, test) != 0) {
        passed = false;
    }

    fclose(test);

    for (int i = 0; i < book.size; i++) {
        if (wcscmp(book.entries[i].name, names[i]) != 0 || strcmp(book.entries[i].number, numbers[i]) != 0) {
            return false;
        }
    }

    return passed;
}

#define DB_ADD_ENTRY_TEST_PATH JOIN(PHONEBOOK_TESTS_DIR, addEntry.test)
bool testAddEntry() {
    phoneBook book;
    loadPhoneBook(&book, DB_LOAD_TEST_PATH);

    wchar_t name[] = L"тесттесттест1234";
    char number[] = "000000000000";
    phoneBookAddEntry(&book, name, number);
    book.saved = true;

    FILE* test = fopen(DB_ADD_ENTRY_TEST_PATH, "r");
    if (test == NULL) {
        fprintf(stderr, "Error: Can't open %s for reading.\n", DB_ADD_ENTRY_TEST_PATH);
        return false;
    }
    bool passed = true;

    // compare bytes of file and structure
    char byte[1];
    for (int i = 0; i < sizeof(phoneBook); i++) {
        if (fread(byte, 1, 1, test) != 1 || byte[0] != ((char*)&book)[i]) {
            passed = false;
            break;
        }
    }
    // if the file's size is bigger than the structure's
    if (fread(byte, 1, 1, test) != 0) {
        passed = false;
    }

    fclose(test);

    return passed;
}

#define DB_SAVE_TEST_PATH DB_ADD_ENTRY_TEST_PATH
#define DB_SAVE_CURR_DB JOIN(PHONEBOOK_TESTS_DIR, db.dat)
bool testSaveDatabase() {
    phoneBook book;

    loadPhoneBook(&book, DB_LOAD_TEST_PATH);

    wchar_t name[] = L"тесттесттест1234";
    char number[] = "000000000000";
    phoneBookAddEntry(&book, name, number);
    savePhoneBook(&book, DB_SAVE_CURR_DB);

    FILE* test = fopen(DB_SAVE_TEST_PATH, "r");
    if (test == NULL) {
        fprintf(stderr, "Error: Can't open %s for reading.\n", DB_SAVE_TEST_PATH);
        return false;
    }
    FILE* db = fopen(DB_SAVE_CURR_DB, "r");
    if (test == NULL) {
        fprintf(stderr, "Error: Can't open %s for reading.\n", DB_SAVE_CURR_DB);
        return false;
    }

    bool passed = true;
    int db_char = 0;
    int test_char = 0;
    while (db_char != EOF && test_char != EOF) {
        db_char = getc(db);
        test_char = getc(test);
        if (test_char != db_char) {
            printf("%d, || %d \n", test_char, db_char);
            passed = false;
            break;
        }
    }

    fclose(db);
    fclose(test);
    remove(DB_SAVE_CURR_DB);

    return passed;
}

#define SEARCH_BY_NAME_TEST_PATH JOIN(PHONEBOOK_TESTS_DIR, nameSearch.test)
bool testSearchByName() {
    phoneBook book;
    loadPhoneBook(&book, SEARCH_BY_NAME_TEST_PATH);

    int findResults[PHONEBOOK_MAX_ENTRIES];
    int entriesCount = findSimilarNamesPhoneBook(&book, L"test", findResults);
    if (!(entriesCount == 3 && findResults[0] == 0 && findResults[1] == 1 && findResults[2] == 4)) {
        return false;
    }

    entriesCount = findSimilarNamesPhoneBook(&book, L"test12345", findResults);
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
    assert(testSearchByName());
    assert(testSaveDatabase());
    assert(testSearchByPhoneNumber());

    return 0;
}