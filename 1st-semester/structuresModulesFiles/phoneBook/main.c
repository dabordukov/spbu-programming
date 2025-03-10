#include <locale.h>
#include <math.h>
#include <stdbool.h>
#include <stdio.h>
#include <stdlib.h>
#include <wchar.h>

#include "phoneBook/ioAuxiliaries.h"
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

    char name[PHONEBOOK_NAME_LENGTH_MAX] = {0};
    printf("Введите имя (не более %lu символов): ", PHONEBOOK_NAME_LENGTH_MAX / sizeof(wchar_t));
    readLineN(name, PHONEBOOK_NAME_LENGTH_MAX / sizeof(wchar_t));

    char number[PHONEBOOK_NUMBER_LENGTH_MAX] = {0};
    do {
        printf("Введите номер (не более %lu цифр): ", PHONEBOOK_NUMBER_LENGTH_MAX - 1);
        readLineN(number, PHONEBOOK_NUMBER_LENGTH_MAX);
    } while (!isValidPhoneNumber(number));

    phoneBookAddEntry(&book, name, number);
}

void printPhoneBook() {
    phoneBookPrint(&book);
}

void findEntryByName() {
    int findResults[PHONEBOOK_MAX_ENTRIES];

    printf("Введите имя для поиска: ");
    char name[PHONEBOOK_NAME_LENGTH_MAX] = {0};
    readLineN(name, PHONEBOOK_NAME_LENGTH_MAX / sizeof(wchar_t));

    int entriesCount = findSimilarNamesPhoneBook(&book, name, findResults);
    for (int i = 0; i < entriesCount; i++) {
        printf("%d) ", i + 1);
        phoneBookEntryPrint(&book.entries[findResults[i]], (int)log10(entriesCount) + 3);
        printf("\n");
    }
}

void findEntryByPhoneNumber() {
    int findResults[PHONEBOOK_MAX_ENTRIES];

    char number[PHONEBOOK_NUMBER_LENGTH_MAX] = {0};
    do {
        printf("Введите номер для поиска (не более %lu цифр): ", PHONEBOOK_NUMBER_LENGTH_MAX - 1);
        readLineN(number, PHONEBOOK_NUMBER_LENGTH_MAX);
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
        do {
            printf("Выберите действие: ");
        } while (scanf("%c", &userChoice) != 1);
        flushSTDIN();
        switch (userChoice) {
            case '0':
                exitProgam();
                return;
            case '1':
                addEntry();
                break;
            case '2':
                printPhoneBook();
                break;
            case '3':
                findEntryByName();
                break;
            case '4':
                findEntryByPhoneNumber();
                break;
            case '5':
                save();
                break;
        }
    }
}

int main() {
#ifdef _WIN32
    system("chcp 1251");
    setlocale(LC_ALL, "Russian");
#else
    setlocale(LC_CTYPE, "");
#endif

    runTest();
    load();
    menu();
}