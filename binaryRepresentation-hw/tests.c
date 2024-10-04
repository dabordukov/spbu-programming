#include <assert.h>
#include <stdbool.h>
#include <stdio.h>
#include <stdlib.h>

#include "auxiliaries.h"
#include "binaryRepresentation.h"

bool testCaseBinaryAddOneNegativeOne(void (*binaryAddOne)(char*, int)) {
    char bits[BINARY_SIZE] = {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1};
    binaryAddOne(bits, sizeof(bits));
    for (int i = 0; i < sizeof(bits); i++) {
        if (bits[i] == 1) {
            return false;
        }
    }

    return true;
}

bool testCaseBinaryAddOneZero(void (*binaryAddOne)(char*, int)) {
    char bits[BINARY_SIZE] = {0};
    binaryAddOne(bits, sizeof(bits));
    for (int i = 0; i < sizeof(bits) - 1; i++) {
        if (bits[i] == 1) {
            return false;
        }
    }

    return bits[sizeof(bits) - 1] == 1;
}

bool testCaseBinaryAddOneOne(void (*binaryAddOne)(char*, int)) {
    char bits[BINARY_SIZE] = {0};
    bits[sizeof(bits) - 1] = 1;
    binaryAddOne(bits, sizeof(bits));
    for (int i = 0; i < sizeof(bits) - 2; i++) {
        if (bits[i] == 1) {
            return false;
        }
    }

    return bits[sizeof(bits) - 2] == 1 && bits[sizeof(bits) - 1] == 0;
}

void testBinaryAddOne(void (*binaryAddOne)(char*, int)) {
    assert(testCaseBinaryAddOneNegativeOne(binaryAddOne));
    assert(testCaseBinaryAddOneZero(binaryAddOne));
    assert(testCaseBinaryAddOneOne(binaryAddOne));
}

bool testCaseInvertBits(void (*invertBits)(char*, int)) {
    char bits[BINARY_SIZE] = {1, 1, 1, 1, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 1, 1, 0, 1, 0, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 1, 0, 1};
    char check[BINARY_SIZE] = {0, 0, 0, 0, 1, 0, 1, 1, 1, 0, 1, 1, 1, 1, 0, 0, 1, 0, 1, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 1, 0};
    invertBits(bits, sizeof(bits));
    for (int i = 0; i < sizeof(bits); i++) {
        if (bits[i] != check[i]) {
            return false;
        }
    }
    return true;
}

void testInvertBits(void (*invertBits)(char*, int)) {
    assert(testCaseInvertBits(invertBits));
}

bool testCaseNegativeBinaryNegativeOne(void (*negativeBinary)(char*, int)) {
    char bits[BINARY_SIZE] = {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1};
    negativeBinary(bits, sizeof(bits));

    for (int i = 0; i < sizeof(bits) - 1; i++) {
        if (bits[i] == 1) {
            return false;
        }
    }

    return bits[sizeof(bits) - 1] == 1;
}

bool testCaseNegativeBinary(void (*negativeBinary)(char*, int)) {
    char bits[BINARY_SIZE] = {1, 1, 1, 1, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 1, 1, 0, 1, 0, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 1, 0, 1};
    char check[BINARY_SIZE] = {0, 0, 0, 0, 1, 0, 1, 1, 1, 0, 1, 1, 1, 1, 0, 0, 1, 0, 1, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 1, 1};
    negativeBinary(bits, sizeof(bits));
    for (int i = 0; i < sizeof(bits); i++) {
        if (bits[i] != check[i]) {
            return false;
        }
    }
    return true;
}

void testNegativeBinary(void (*negativeBinary)(char*, int)) {
    assert(testCaseNegativeBinary(negativeBinary));
    assert(testCaseNegativeBinaryNegativeOne(negativeBinary));
}

bool testCaseConvertDecimalTo2ComplementSomeNumber(char* (*convertDecimalTo2Complement)(int, int)) {
    int x = -714979;
    char check[BINARY_SIZE] = {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 0, 1, 0, 0, 0, 1, 0, 1, 1, 1, 0, 0, 0, 1, 1, 1, 0, 1};
    char* bits = convertDecimalTo2Complement(x, sizeof(check));
    for (int i = 0; i < sizeof(check); i++) {
        if (bits[i] != check[i]) {
            free(bits);
            return false;
        }
    }
    free(bits);

    return true;
}

void testConvertDecimalTo2Complement(char* (*convertDecimalTo2Complement)(int, int)) {
    assert(testCaseConvertDecimalTo2ComplementSomeNumber(convertDecimalTo2Complement));
}

bool testCaseBinaryAdd(void (*binaryAdd)(char*, char*, int)) {
    int x = -714979;
    int y = 1082122;
    char* check = convertDecimalTo2Complement(x + y, BINARY_SIZE);
    char* bitsX = convertDecimalTo2Complement(x, BINARY_SIZE);
    char* bitsY = convertDecimalTo2Complement(y, BINARY_SIZE);
    binaryAdd(bitsX, bitsY, BINARY_SIZE);
    free(bitsY);

    for (int i = 0; i < BINARY_SIZE; i++) {
        if (bitsX[i] != check[i]) {
            free(bitsX);
            free(check);
            return false;
        }
    }

    free(bitsX);
    free(check);
    return true;
}

void testBinaryAdd(void (*binaryAdd)(char*, char*, int)) {
    assert(testCaseBinaryAdd(binaryAdd));
}

bool testCaseConvert2ComplementToDecimal(int (*convert2ComplementToDecimal)(char*, int)) {
    for (int i = -5; i < 5; i++) {
        int test = rand() % (RAND_MAX / 5) * i;
        char* bits = convertDecimalTo2Complement(test, BINARY_SIZE);
        int result = convert2ComplementToDecimal(bits, BINARY_SIZE);
        free(bits);
        if (result != test) {
            return false;
        }
    }

    return true;
}

void testConvert2ComplementToDecimal(int (*convert2ComplementToDecimal)(char*, int)) {
    assert(testCaseConvert2ComplementToDecimal(convert2ComplementToDecimal));
}