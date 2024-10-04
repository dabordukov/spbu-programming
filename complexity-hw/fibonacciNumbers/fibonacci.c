#include <time.h>

#include "auxiliaries.h"

clock_t computeRuntimeFibonacci(int (*fibonacci)(int, int*), int n, int* returnResult, int* returnError) {
    int error = 0;
    clock_t start = clock();
    *returnResult = fibonacci(n, &error);
    clock_t end = clock();
    *returnError = error;

    return end - start + 1;
}
/**Sets error = -1 on bad N
   error = 1 on overflow **/
int fibonacciRecursive(int n, int* error) {
    if (*error != 0) {  // true only if user forgets to set error = 0. it doesn't affect the recursion
        *error = 0;
    }

    if (n <= 0) {
        *error = -1;
        return 0;
    }

    if (n <= 2) {
        return 1;
    }

    int a = fibonacciRecursive(n - 1, error);
    if (*error || a == 0) {
        return 0;
    }

    int b = fibonacciRecursive(n - 2, error);
    if (*error || b == 0) {
        return 0;
    }

    if (addIntOverflow(a, b)) {
        *error = 1;
        return 0;
    }

    return a + b;
}

/**Sets error = -1 on bad N
   error = 1 on overflow **/
int fibonacciIterative(int n, int* error) {
    *error = 0;

    if (n <= 0) {
        *error = -1;
        return 0;
    }

    int fibonacci[2] = {1, 0};
    while (n--) {
        swap(fibonacci, fibonacci + 1);
        if (addIntOverflow(fibonacci[0], fibonacci[1])) {
            *error = 1;
            return 0;
        }
        fibonacci[1] += fibonacci[0];
    }

    return fibonacci[1];
}
