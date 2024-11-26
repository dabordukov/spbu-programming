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
    char* key = NULL;
    do {
        printf("Введите ключ: ");
        flushSTDIN();
    } while (readLine(&key) == 0);

    if (dictionaryHas(dictionary, key) == false) {
        printf("Запись с ключом %s отсутствует\n", key);
    } else {
        printf("Запись с ключом %s присутствует\n", key);
    }
}

void getValue() {
    char* key = NULL;
    do {
        printf("Введите ключ: ");
        flushSTDIN();
    } while (readLine(&key) == 0);

    if (dictionaryHas(dictionary, key) == false) {
        printf("Запись с ключом %s отсутствует\n", key);
    } else {
        printf("[%s]: %s\n", key, dictionaryGet(dictionary, key));
    }
}

int addEntry() {
    char* key = NULL;
    do {
        printf("Введите ключ: ");
        flushSTDIN();
    } while (readLine(&key) == 0);

    char* string = NULL;
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
    char* key = NULL;
    do {
        printf("Введите ключ: ");
        flushSTDIN();
    } while (readLine(&key) == 0);

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