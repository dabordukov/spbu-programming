#ifndef FIBONACCI_TESTS
#define FIBONACCI_TESTS 1
bool testCaseFibonacciFirst20(int (*fibonacci)(int*, int));
bool testCaseFibonacciNonNatural(int (*fibonacci)(int*, int));
bool testCaseFibonacciOverflowReturnsError(int (*fibonacci)(int*, int));
#endif