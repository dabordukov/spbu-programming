#include <math.h>
#include <stdbool.h>
#include <stdio.h>
#include <stdlib.h>

#include "pow.h"

int main() {
    double number = 0;
    int power = 0;
    printf("Enter base  n = ");
    while (scanf("%lf", &number) != 1) {
        scanf("%*[^\n]");
        printf("Enter base  n = ");
    }

    printf("Enter power p = ");
    while (scanf("%d", &power) != 1) {
        scanf("%*[^\n]");
        printf("Enter decimal p = ");
    }

    int error = 0;
    double result = 0;
    result = powLinear(number, power, &error);
    printf("powLinear: ");
    if (error == 0) {
        if (fabs(result) < 1e-6) {
            printf("%g\n", result);
        } else {
            printf("%lf\n", result);
        }
    } else {
        printf("Overflowed\n");
    }

    result = powLogarithmic(number, power, &error);

    printf("powLogarithmic: ");
    if (error == 0) {
        if (fabs(result) < 1e-6) {
            printf("%g\n", result);
        } else {
            printf("%lf\n", result);
        }
    } else {
        printf("Overflowed\n");
    }
}
