#include <assert.h>
#include <stdbool.h>

bool testCaseFibonacciFirst20(int (*fibonacci)(int*, int)) {
    int precalculatedFibonacci[20] = {1, 1, 2, 3, 5, 8, 13, 21, 34, 55,
                                      89, 144, 233, 377, 610, 987, 1597, 2584, 4181, 6765};

    int result = 0, error = 0;
    for (int i = 0; i < 20; i++) {
        result = fibonacci(i + 1, &error);
        if (result != precalculatedFibonacci[i] || error != 0) {
            return false;
        }
    }

    return true;
}

bool testCaseFibonacciNonNatural(int (*fibonacci)(int*, int)) {
    int error = 0;
    fibonacci(0, &error);
    if (error != -1) {
        return false;
    }

    fibonacci(-1, &error);
    return error == -1
}

bool testCaseFibonacciOverflowReturnsError(int (*fibonacci)(int*, int)) {
    int error = 0;
    fibonacci(47, &error);
    return error == 1;
}

// void fibonacciTestAll(int (*fibonacci)(int, int*)) {
//     testCaseFibonacciFirst20(fibonacci);
//     testCaseFibonacciNonNatural(fibonacci);
//     testCaseFibonacciOverflowReturnsError(fibonacci);
// }
