#include <assert.h>
#include <limits.h>
#include <stdbool.h>
#include <stdio.h>
#include <time.h>

#include "auxiliaries.h"

void swap(int* a, int* b) {
    int temp = *a;
    *a = *b;
    *b = temp;
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

void testCaseFibonacciFirst20() {
    int precalculatedFibonacci[20] = {1, 1, 2, 3, 5, 8, 13, 21, 34, 55,
                                      89, 144, 233, 377, 610, 987, 1597, 2584, 4181, 6765};

    int result = 0, error = 0;
    for (int i = 0; i < 20; i++) {
        result = fibonacciIterative(i + 1, &error);
        assert(result == precalculatedFibonacci[i] && error == 0);

        result = fibonacciRecursive(i + 1, &error);
        assert(result == precalculatedFibonacci[i] && error == 0);
    }
}

void testCaseFibonacciNonNatural() {
    int error = 0;
    fibonacciIterative(0, &error);
    assert(error == -1);

    fibonacciRecursive(0, &error);
    assert(error == -1);

    fibonacciIterative(-1, &error);
    assert(error == -1);

    fibonacciRecursive(-1, &error);
    assert(error == -1);
}

void testCaseFibonacciOverflow() {
    int error = 0;
    fibonacciIterative(47, &error);
    assert(error == 1);

    fibonacciRecursive(47, &error);
    assert(error == 1);
}

void testAll() {
    printf("Testing...\n");
    testCaseFibonacciFirst20();
    testCaseFibonacciNonNatural();
    testCaseFibonacciOverflow();

    printf("Tests are passed\n");
}

int main() {
    testAll();

    int error = 0;
    clock_t timeDifference = 0, timeIterativeBegin = 0, timeIterativeEnd = 0, timeRecursiveBegin = 0, timeRecursiveEnd = 0;
    int recursiveSlowerTimes = 1000000;
    int recursiveTimeLimitSeconds = 5;
    bool restrictInformed = false;
    bool overflowFlag = 0;

    int i = 0;
    while (timeDifference < recursiveSlowerTimes) {
        i++;

        error = 0;
        timeIterativeBegin = clock();
        int result = fibonacciIterative(i, &error);
        timeIterativeEnd = clock() + 1;  // fix division by zero

        if (error == 1) {
            overflowFlag = true;
            break;
        }

        error = 0;
        timeRecursiveBegin = clock();
        fibonacciRecursive(i, &error);
        timeRecursiveEnd = clock();
        timeDifference = (timeRecursiveEnd - timeRecursiveBegin) / (timeIterativeEnd - timeIterativeBegin);

        if (!restrictInformed && (double)(timeRecursiveEnd - timeRecursiveBegin) / CLOCKS_PER_SEC > recursiveTimeLimitSeconds) {
            printf("Recursive takes more than %ds. Press CTRL-C not to wait anymore.\nChoose another recursiveSlowerTimes value\n", recursiveTimeLimitSeconds);
            restrictInformed = 1;
        }

        if (error == 1) {
            overflowFlag = true;
            break;
        } else {
            printf("fib(%d) = %d\n", i, result);
        }
    }

    if (overflowFlag) {
        printf("Overflowed before difference became %d times\nTry another recursiveSlowerTimes", recursiveSlowerTimes);
    } else {
        printf("Since n=%d Recursive is slower than Iterative more than %d times\n", i, recursiveSlowerTimes);
    }
}