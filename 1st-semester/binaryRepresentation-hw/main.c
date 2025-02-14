#include <locale.h>
#include <stdbool.h>
#include <stdio.h>
#include <stdlib.h>
#include <time.h>

#include "auxiliaries.h"
#include "binaryRepresentation.h"
#include "tests.h"

int main() {
    srand(time(NULL));
    setlocale(LC_ALL, "Russian");

    testBinaryAddOne(&binaryAddOne);
    testInvertBits(&invertBits);
    testNegativeBinary(&negativeBinary);
    testConvertDecimalTo2Complement(&convertDecimalTo2Complement);
    testBinaryAdd(&binaryAdd);
    testConvert2ComplementToDecimal(&convert2ComplementToDecimal);
    printf("[+] Тесты успешно пройдены\n");

    int number1 = 0, number2 = 0;
    printf("Введите первое число: ");
    while (scanf("%d", &number1) != 1) {
        scanf("%*[^\n]");
        printf("Введите первое число -2^31 <= x < 2^31: ");
    }

    printf("Введите второе число: ");
    while (scanf("%d", &number2) != 1) {
        scanf("%*[^\n]");
        printf("Введите второе число -2^31 <= x < 2^31: ");
    }

    char* number1Bits = convertDecimalTo2Complement(number1, BINARY_SIZE);
    char* number2Bits = convertDecimalTo2Complement(number2, BINARY_SIZE);
    binaryAdd(number1Bits, number2Bits, BINARY_SIZE);
    int number3 = convert2ComplementToDecimal(number1Bits, BINARY_SIZE);
    printf("Сумма %d и %d = %d\n", number1, number2, number3);

    free(number1Bits);
    free(number2Bits);
}