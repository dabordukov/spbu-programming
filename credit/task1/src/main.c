#include <locale.h>
#include <stdio.h>
#include <stdlib.h>

#include "binary.h"
#include "ioAuxiliaries.h"

int main() {
#ifdef _WIN32
    system("chcp 65001");
    setlocale(LC_CTYPE, "Ru.UTF-8");
#else
    setlocale(LC_CTYPE, "");
#endif

    printf("Введите двоичное число:\n");
    char* binaryNumber = NULL;
    readLine(&binaryNumber);

    int error = 0;
    unsigned number = binaryToInteger(binaryNumber, &error);
    if (error == 0) {
        printf("Десятичное число: %u\n", number);
    } else {
        printf("Ошибка\n");
    }

    free(binaryNumber);
}