#include <stdio.h>

#include "ioAuxiliaries.h"
#include "shuntingYard.h"

int main() {
    char* out = NULL;
    char* in = NULL;
    int err = 0;
    readLine(&in, &err);
    if (in == NULL) {
        printf("Не удалось прочитать строку\n");
        return 1;
    }
    shuntingYardAlgorithm(in, &out);

    printf("RPN:\n");
    if (out != NULL) {
        printf("%s\n", out);
    } else {
        printf("Ошибка..\n");
    }
}