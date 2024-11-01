/*Считать текст из файла и вывести его на консоль,
заменяя каждую последовательность повторяющихся символов одним символом.
Например, aafgbbba должно выводиться как afgba.*/

#include <assert.h>
#include <locale.h>
#include <stdbool.h>
#include <stdio.h>
#include <wchar.h>

#include "ioAuxiliaries.h"

void printWithoutRepeatingSequence(FILE* in, FILE* out) {
    // char* input = NULL;
    // int err = 0;
    // int lengthInput = freadLine(in, &input, &err);

    char prev = EOF;
    char ch = EOF;
    while ((ch = fgetc(in)) != EOF) {
        if (ch != prev) {
            putc(ch, out);
        }
        prev = ch;
    }
    putc('\n', out);
}

bool test() {
    FILE* compareFile = NULL;
    FILE* toCheck = fopen("test1", "r");
    FILE* temp = fopen("test1.tmp", "w");
    if (toCheck && temp) {
        printWithoutRepeatingSequence(toCheck, temp);
    } else {
        printf("1111");
        return false;
    }
    fclose(toCheck);
    fclose(temp);

    temp = fopen("test1.tmp", "r");
    compareFile = fopen("compareFile", "r");

    char a = fgetc(temp);
    char b = fgetc(compareFile);
    if (a != b) {
        printf("%c ? %c\n", a, b);

        return false;
    }
    while (a != EOF && b != EOF) {
        a = fgetc(temp);
        b = fgetc(compareFile);

        if (a != b) {
            printf("%d ? %d\n", a, b);

            return false;
        }
    }

    fclose(temp);
    fclose(compareFile);
    remove("test1.tmp");

    return true;
}

#define PATH_MAX_LENGTH 4096
int main(int argc, char* argv[]) {
    setlocale(LC_ALL, "Russian");
    assert(test());

    FILE* stream = NULL;
    if (argc > 1) {
        stream = fopen(argv[1], "r");

    } else {
        char buffer[PATH_MAX_LENGTH + 1] = {0};
        printf("Введите имя файла: ");
        readLineN(buffer, PATH_MAX_LENGTH + 1);
        stream = fopen(buffer, "r");
    }

    if (stream) {
        printWithoutRepeatingSequence(stream, stdout);
        fclose(stream);
    }
}