#include <limits.h>
#include <stdbool.h>
#include <stdio.h>
#include <stdlib.h>

bool mulIntOverflow(int a, int b) {
    if (1ll * a * b > INT_MAX || 1ll * a * b < INT_MIN) {
        return true;
    }
    return false;
}

int powLinear(int number, int power, int* error) {
    *error = 0;
    int base = abs(number);
    int result = (number < 0) ? -1 : 1;
    while (power--) {
        if (mulIntOverflow(result, base)) {
            *error = 1;
            return 0;
        }
        result *= base;
    }

    return result;
}

int powLogarithmic(int number, int power, int* error) {
    *error = 0;
    int base = abs(number);
    int result = (number < 0) ? -1 : 1;
    bool baseOverflowFlag = false;
    while (power) {
        if (power % 2) {
            if (mulIntOverflow(result, base) || baseOverflowFlag) {
                *error = 1;
                return 0;
            }
            result *= base;
        }

        if (mulIntOverflow(base, base)) {
            baseOverflowFlag = true;  // if we return error here we lose some of result values, so error is returned on mulitiplication only
        }

        base *= base;
        power /= 2;
    }
    return result;
}

int main() {
    int number = 0, power = 0;
    printf("Enter base  n = ");
    while (scanf("%d", &number) != 1) {
        scanf("%*[^\n]");
        printf("Enter base  n = ");
    }

    printf("Enter power  p = ");
    while (scanf("%d", &power) != 1 || power < 0) {
        scanf("%*[^\n]");
        printf("Enter decimal >= 0  p = ");
    }

    int error = 0, result = 0;
    result = powLinear(number, power, &error);
    printf("powLinear: ");
    if (error) {
        printf("Integer overflow\n");
    } else {
        printf("%d\n", result);
    }

    result = powLogarithmic(number, power, &error);
    printf("powLogarithmic: ");
    if (error) {
        printf("Integer overflow\n");
    } else {
        printf("%d\n", result);
    }
}