#include <assert.h>
#include <limits.h>
#include <stdbool.h>
#include <stdio.h>
#include <time.h>

#include "auxiliaries.h"
#include "fibonacci.h"
#include "fibonacciTests.h"

void testAll() {
    assert(testCaseFibonacciFirst20(fibonacciIterative) == 1);
    assert(testCaseFibonacciNonNatural(fibonacciIterative) == 1);
    assert(testCaseFibonacciOverflowReturnsError(fibonacciIterative) == 1);

    assert(testCaseFibonacciFirst20(fibonacciRecursive) == 1);
    assert(testCaseFibonacciNonNatural(fibonacciRecursive) == 1);
}
int main() {
    printf("Testing..\n");
    testAll();
    printf("All tests passed\n");

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