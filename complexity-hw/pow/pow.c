#include <float.h>
#include <limits.h>
#include <math.h>
#include <stdbool.h>
#include <stdio.h>
#include <stdlib.h>

bool doubleMultiplyOverflow(double a, double b) {
    return fabs(a) >= DBL_EPSILON && b > DBL_MAX / a;
}

double powLinear(double number, int power, int* error) {
    *error = 0;
    double result = 1;

    bool reverse = false;
    if (power < 0) {
        reverse = true;
        power = -power;
    }

    while (power--) {
        if (doubleMultiplyOverflow(result, number)) {
            *error = 1;
            return 0;
        }

        result *= number;
    }

    return (reverse) ? 1 / result : result;
}

double powLogarithmic(double number, int power, int* error) {
    *error = 0;
    double result = 1;
    bool reverse = false;
    if (power < 0) {
        reverse = true;
        power = -power;
    }

    while (power) {
        if (power % 2) {
            result *= number;
        }

        if (doubleMultiplyOverflow(number, number)) {
            *error = 1;
            return 0;
        }

        number *= number;
        power /= 2;
    }

    return (reverse) ? 1 / result : result;
}
