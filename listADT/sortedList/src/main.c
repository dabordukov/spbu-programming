#include <locale.h>
#include <stdint.h>
#include <stdio.h>
#include <stdlib.h>

#include "ioAuxiliaries.h"
#include "sortedList.h"

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
#else
    return;
#endif
}

void clearScreen() {
#ifdef __linux__
    system("clear");
#else
    return;
#endif
}

List* list;

const char menuMessage[] =
    "0) Выйти\n\
1) Добавить значение\n\
2) Удалить значение\n\
3) Распечатать список";

int addValue() {
    intptr_t val;
    int scanned = 0;
    while (scanned != 1) {
        printf("Введите значение: ");
        scanned = scanf("%ld", &val);
        flushSTDIN();
    }

    if (listInsertAfterSorted(list, (void*)val, &comparatorInsertAscendingInt) == NULL) {
        return 1;
    }

    return 0;
}

void removeValue() {
    intptr_t val;
    int scanned = 0;
    while (scanned != 1) {
        printf("Введите значение: ");
        scanned = scanf("%ld", &val);
        flushSTDIN();
    }

    ListPosition* pos = listFindData(list, (void*)val, &compareInt);
    if (pos == NULL) {
        printf("Не найдено\n");
        return;
    }

    listRemoveNode(list, &pos);

    return;
}

void printList() {
    if (listIsEmpty(list)) {
        printf("Список пустой\n");
        return;
    }
    listPrint(list, &printListInt);
    putchar('\n');

    return;
}

int menu() {
    char userChoice = 0;
    while (userChoice != '0') {
        puts(menuMessage);
        do {
            printf("Выберите действие: ");
        } while (scanf("%c", &userChoice) != 1);
        flushSTDIN();
        switch (userChoice) {
            case '0':
                return 0;
            case '1':
                clearScreen();
                if (addValue() != 0) {
                    return 1;
                }
                break;
            case '2':
                clearScreen();
                removeValue();
                break;
            case '3':
                clearScreen();
                printList();
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

    list = listInitWithDestructor(&nothing);

    alternateBuffer();
    clearScreen();

    int res = menu();
    alternateBuffer();
    if (res != 0) {
        printf("Что-то сломалось\n");
    }

    listFree(&list);
    return res;
}