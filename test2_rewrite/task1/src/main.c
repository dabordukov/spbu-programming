#include <locale.h>
#include <stdio.h>

#include "processFile.h"

int main() {
#ifdef _WIN32
    system("chcp 65001");
    setlocale(LC_CTYPE, "Ru.UTF-8");
#else
    setlocale(LC_CTYPE, "");
#endif

    int a = 0;
    int b = 0;
    char inFilename[256] = {'\0'};
    char outFilename[256] = {'\0'};

    int read = 0;
    while (read == 0) {
        printf("Введите a: ");
        read = scanf("%d", &a);
    }

    read = 0;
    while (read == 0) {
        printf("Введите b: ");
        read = scanf("%d", &b);
    }
    read = 0;
    while (read == 0) {
        printf("Введите имя входного файла: ");
        read = scanf("%255s", inFilename);
    }
    read = 0;
    while (read == 0) {
        printf("Введите имя выходного файла: ");
        read = scanf("%255s", outFilename);
    }

    int result = processFile(inFilename, outFilename, a, b);

    if (result == 1) {
        fprintf(stderr, "Can't open %s for reading\n", inFilename);
    } else if (result == 2) {
        fprintf(stderr, "Can't open %s for writing\n", outFilename);
    } else {
        printf("Успешно.\n");
    }
}