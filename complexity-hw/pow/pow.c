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
    int absPower = abs(power);

    while (absPower--) {
        if (doubleMultiplyOverflow(result, number)) {
            *error = 1;
            return 0;
        }

        result *= number;
    }

    return (power < 0) ? 1 / result : result;
}

double powLogarithmic(double number, int power, int* error) {
    *error = 0;
    double result = 1;
    int absPower = abs(power);

    while (absPower) {
        if (absPower % 2 == 1) {
            result *= number;
        }

        if (doubleMultiplyOverflow(number, number)) {
            *error = 1;
            return 0;
        }

        number *= number;
        absPower /= 2;
    }

    return (power < 0) ? 1 / result : result;
}
