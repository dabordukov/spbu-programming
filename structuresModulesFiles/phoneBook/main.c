#include <locale.h>
#include <math.h>
#include <stdbool.h>
#include <stdio.h>
#include <stdlib.h>
#include <wchar.h>

#ifdef _WIN32
#define clrscr() system("cls")
#include <windows.h>
#else
#define clrscr() system("clear")
#endif

#include "auxiliaries.h"
#include "phoneBook/phoneBook.h"
#include "phoneBook/tests/phoneBookTest.h"

const char phoneBookDatabase[] = "db.dat";
const char menuMessage[] =
    "0) Выйти\n\
1) Добавить запись (имя и телефон)\n\
2) Распечатать все имеющиеся записи\n\
3) Найти телефон по имени (Нечеткий поиск)\n\
4) Найти имя по телефону\n\
5) Сохранить текущие данные в файл";

phoneBook book;
void load() {
    int error = loadPhoneBook(&book, phoneBookDatabase);
    if (error == 0) {
        printf("Файл загружен.\n");
    }
}
void save() {
    int error = savePhoneBook(&book, phoneBookDatabase);
    if (error == 0) {
        printf("Файл сохранен.\n");
    }
}

void exitProgam() {
    if (!isSavedPhoneBook(&book)) {
        char userInput = 0;
        do {
            printf("Книга не сохранена!\nХотите сохранить? [n/Y]: ");
        } while (scanf("%c", &userInput) != 1);
        switch (userInput) {
            case 'N':
            case 'n':
                break;
            default:
                save();
        }
    }

    printf("Выход..\n");
}

void addEntry() {
    if (isFullPhoneBook(&book)) {
        printf("Книга заполнена!\n");
        return;
    }

    char name[PHONEBOOK_NAME_LENGTH_MAX * sizeof(wchar_t)] = {0};
    printf("Введите имя (не более %d символов): ", PHONEBOOK_NAME_LENGTH_MAX - 1);
    readStringN(name, PHONEBOOK_NAME_LENGTH_MAX * sizeof(wchar_t));

    char number[PHONEBOOK_NUMBER_LENGTH_MAX] = {0};
    do {
        printf("Введите номер (не более %d цифр): ", PHONEBOOK_NUMBER_LENGTH_MAX - 1);
        readStringN(number, PHONEBOOK_NUMBER_LENGTH_MAX);
    } while (!isValidPhoneNumber(number));

    wchar_t nameWideChar[PHONEBOOK_NAME_LENGTH_MAX] = {0};
    mbstowcs(nameWideChar, name, PHONEBOOK_NAME_LENGTH_MAX - 1);
    printf("RESULT: name: %ls number: %s\n", nameWideChar, number);

    phoneBookAddEntry(&book, nameWideChar, number);
}

void printPhoneBook() {
    phoneBookPrint(&book);
}

void findEntryByName() {
    int findResults[PHONEBOOK_MAX_ENTRIES];

    printf("Введите имя для поиска: ");
    char name[PHONEBOOK_NAME_LENGTH_MAX] = {0};
    readStringN(name, PHONEBOOK_NAME_LENGTH_MAX);

    wchar_t nameWideChar[PHONEBOOK_NAME_LENGTH_MAX] = {0};
    mbstowcs(nameWideChar, name, sizeof(name));

    int entriesCount = findSimilarNamesPhoneBook(&book, nameWideChar, findResults);
    for (int i = 0; i < entriesCount; i++) {
        phoneBookEntryPrint(&book.entries[findResults[i]], (int)log10(entriesCount));
    }
}

void findEntryByPhoneNumber() {
    int findResults[PHONEBOOK_MAX_ENTRIES];

    char number[PHONEBOOK_NUMBER_LENGTH_MAX] = {0};
    do {
        printf("Введите номер для поиска (не более %d цифр): ", PHONEBOOK_NUMBER_LENGTH_MAX - 1);
        readStringN(number, PHONEBOOK_NUMBER_LENGTH_MAX);
    } while (!isValidPhoneNumber(number));

    int entriesCount = findNumberPhoneBook(&book, number, findResults);
    for (int i = 0; i < entriesCount; i++) {
        phoneBookEntryPrint(&book.entries[findResults[i]], (int)log10(entriesCount));
    }
}

void menu() {
    char userChoice = 0;
    while (userChoice != '0') {
        puts(menuMessage);
        printf("Выберите действие: ");
        scanf("%c", &userChoice);
        flushSTDIN();
        switch (userChoice) {
            case '0':
                exitProgam();
                return;
            case '1':
                clrscr();
                addEntry();
                break;
            case '2':
                clrscr();
                printPhoneBook();
                break;
            case '3':
                clrscr();
                findEntryByName();
                break;
            case '4':
                clrscr();
                findEntryByPhoneNumber();
                break;
            case '5':
                clrscr();
                save();
                break;
        }
    }
}

int main() {
#ifdef _WIN32
    system("chcp 1251");
    setlocale(LC_ALL, "Rus");
#else
    setlocale(LC_CTYPE, "");
#endif
    runTest();
    clrscr();

    load();
    menu();
}