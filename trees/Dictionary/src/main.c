#include <locale.h>
#include <stdio.h>

#include "dictionary.h"
#include "ioAuxiliaries.h"

void alternateBuffer() {
#ifdef __linux__
    static bool alternated = false;
    if (!alternated) {
        system("tput smcup");
        alternated = true;
    } else {
        system("tput rmcup");
        alternated = false;
    }
#endif
    return;
}

void clearScreen() {
#ifdef __linux__
    system("clear");
#endif
    return;
}

const char menuMessage[] =
    "0) Выйти\n\
1) Проверить наличие ключа\n\
2) Получить значение по ключу\n\
3) Добавить значение\n\
4) Удалить значение\n";

Dictionary* dictionary;

void checkExists() {
    int key = 0;
    int scanned = 0;
    do {
        printf("Введите ключ: ");
        scanned = scanf("%d", &key);
        flushSTDIN();
    } while (scanned != 1);

    if (dictionaryHas(dictionary, key) == false) {
        printf("Запись с ключом %d отсутствует\n", key);
    } else {
        printf("Запись с ключом %d присутствует\n", key);
    }
}

void getValue() {
    int key = 0;
    int scanned = 0;
    do {
        printf("Введите ключ: ");
        scanned = scanf("%d", &key);
        flushSTDIN();
    } while (scanned != 1);

    if (dictionaryHas(dictionary, key) == false) {
        printf("Запись с ключом %d отсутствует\n", key);
    } else {
        printf("[%d]: %s\n", key, dictionaryGet(dictionary, key));
    }
}

int addEntry() {
    int key = 0;
    do {
        printf("Введите ключ: ");
    } while (scanf("%d", &key) != 1);

    char* string = NULL;
    flushSTDIN();
    do {
        printf("Введите строку, которая будет записана\n");

    } while (readLine(&string) == 0);

    int res = dictionaryInsert(&dictionary, key, string);

    if (res == 0) {
        printf("Добавлено\n");
    }

    return res;
}

void removeEntry() {
    int key = 0;
    int scanned = 0;
    do {
        printf("Введите ключ: ");
        scanned = scanf("%d", &key);
    } while (scanned != 1);
    flushSTDIN();

    dictionaryEntryRemove(&dictionary, key);
    printf("Удалено\n");
}

int menu() {
    char userChoice = 0;
    while (userChoice != '0') {
        puts(menuMessage);
        printf("Выберите действие: \n");
        scanf("%c", &userChoice);

        switch (userChoice) {
            case '0':
                return 0;
            case '1':
                clearScreen();
                checkExists();
                break;
            case '2':
                clearScreen();
                getValue();
                break;
            case '3':
                clearScreen();
                if (addEntry()) {
                    return 3;
                }
                break;
            case '4':
                clearScreen();
                removeEntry();
                break;
            default:
                clearScreen();

                continue;
        }
    }
    return 0;
}

int main() {
#ifdef _WIN32
    system("chcp 65001");
    setlocale(LC_CTYPE, "Ru.UTF-8");
#else
    setlocale(LC_CTYPE, "");
#endif

    alternateBuffer();
    clearScreen();

    int res = menu();
    alternateBuffer();
    if (res != 0) {
        printf("Что-то сломалось\n");
    }

    dictionaryFree(&dictionary);
    return res;
}