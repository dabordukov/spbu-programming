#include <stdio.h>
#include <stdlib.h>

#ifdef _WIN32
#include <conio.h>
#else
#define clrscr() printf("\e[1;1H\e[2J")
#endif

#include "auxiliaries.h"
#include "phoneBook.h"

void printMenu() {
    printf(
        "0) Выйти\n\
1) Добавить запись (имя и телефон)\n\
2) Распечатать все имеющиеся записи\n\
3) Найти телефон по имени\n\
4) Найти имя по телефону\n\
5) Сохранить текущие данные в файл");
}

void menu() {
    clrscr();
    char userChoice = 0;
    while (userChoice != '0') {
        clrscr();
        printMenu();
        printf("Выберите действие: ");
        scanf("%c", &userChoice);
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
                savePhoneBook();
                break;
        }
    }
}

int main() {
}