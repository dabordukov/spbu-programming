#include <assert.h>
#include <stdbool.h>
#include <stdio.h>

#include "postfixCalculator.h"

void testPostfixCalculator(int (*calculate)(char*, int*), char* expression, int expected, int errorCode) {
    int error = 0;
    assert(calculate(expression, &error) == expected);
    assert(error == errorCode);
}

int main() {
    testPostfixCalculator(&calculate, " 1 2 +", 3, 0);
    testPostfixCalculator(&calculate, " 1 2 -", -1, 0);
    testPostfixCalculator(&calculate, " 1 2 *", 2, 0);
    testPostfixCalculator(&calculate, " 1 2 /", 0, 0);
    testPostfixCalculator(&calculate, " 1 2 9 2 +++", 14, 0);
    testPostfixCalculator(&calculate, " 1 2 9 2 +*+", 23, 0);
    testPostfixCalculator(&calculate, " 1 2 9 2 +*+/", 0, 1);
}