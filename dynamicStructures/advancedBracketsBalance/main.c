#include <stdio.h>

#include "ioAuxiliaries.h"
#include "stackOnPointers/functions.h"
#include "stackOnPointers/tests/tests.h"

int main() {
    testStackAll();
    printf("Введите строку:\n");

    char* string = NULL;
    int error;
    size_t size = readLine(&string, &error);

    if (error) {
        printf("Something went wrong..\n");
        return 1;
    }

    if (isBracketBalanced(string)) {
        printf("Скобки сбалансированы\n");
    } else {
        printf("Скобки не сбалансированы\n");
    }
}