#include <locale.h>
#include <stdio.h>

#include "dictionary.h"
#include "ioAuxiliaries.h"

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
        userChoice = getchar();

        switch (userChoice) {
            case '0':
                return 0;
            case '1':
                checkExists();
                break;
            case '2':
                getValue();
                break;
            case '3':
                if (addEntry()) {
                    return 3;
                }
                break;
            case '4':
                removeEntry();
                break;
            default:

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

    int res = menu();
    if (res != 0) {
        printf("Что-то сломалось\n");
    }

    dictionaryFree(&dictionary);
    return res;
}