#include <locale.h>
#include <stdio.h>

#include "ioAuxiliaries.h"
#include "postfixCalculator.h"

int main() {
#ifdef _WIN32
    system("chcp 1251");
    setlocale(LC_ALL, "Russian");
#else
    setlocale(LC_CTYPE, "");
#endif
    printf("Введите выражение в постфиксной форме:\n");
    char* expression = NULL;
    int error;
    readLine(&expression, &error);
    if (error != 0) {
        fprintf(stderr, "Something went wrong...\n");
        return 1;
    }

    int result = calculate(expression, &error);
    if (error != 0) {
        printf("Ошибка в выражении\n");
    } else {
        printf("Результат: %d\n", result);
    }
    free(expression);
}