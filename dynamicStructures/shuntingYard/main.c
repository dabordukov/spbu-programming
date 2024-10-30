#include <stdio.h>

#include "shuntingYard.h"

int main() {
    char* out = NULL;
    char in[] = "2 + 3 + 4";
    printf("%s\n", in);
    shuntingYardAlgorithm(in, &out);

    if (out != NULL) {
        printf("RPN:\n%s\n", out);
    } else {
        printf("Ошибка..\n");
    }
}