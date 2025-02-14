#include <stdbool.h>
#include <stdlib.h>

#include "auxiliaries.h"

void invertBits(char* bits, int size) {
    while (size--) {
        bits[size] = ~bits[size] & 1;
    }
}

void binaryAddOne(char* bits, int size) {
    for (int i = size - 1; i >= 0; i--) {
        if (bits[i] == 1) {
            bits[i] = 0;
        } else {
            bits[i] = 1;
            return;
        }
    }
}

void negativeBinary(char* bits, int size) {
    invertBits(bits, size);
    binaryAddOne(bits, size);
}

// writes result in first operand a
void binaryAdd(char* a, char* b, int size) {
    for (int i = size - 1; i >= 0; i--) {
        if (b[i]) {
            binaryAddOne(a, i + 1);
        }
    }
}

char* convertDecimalTo2Complement(int x, int size) {
    char* bits = checkedCalloc(size, 1);

    bool negative = x < 0;
    if (negative) {
        x = -x;
    }

    int k = 0;
    // filling bits
    while (x) {
        k++;
        if (x & 1) {
            bits[k - 1] = 1;
        }
        x /= 2;
    }

    reverseBits(bits, size);

    if (negative) {
        invertBits(bits, size);
        binaryAddOne(bits, size);
    }

    return bits;
}

int convert2ComplementToDecimal(char* bits, int size) {
    int result = 0;
    int base = 1;

    bool negative = bits[0] == 1;

    if (negative) {
        negativeBinary(bits, size);
    }

    for (int i = size - 1; i > 0; i--) {
        result += bits[i] * base;
        base *= 2;
    }

    if (negative) {
        result = -result;
    }

    return result;
}