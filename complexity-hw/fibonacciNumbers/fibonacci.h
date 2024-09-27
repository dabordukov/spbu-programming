#ifndef FIBONACCI_FUNCTIONS
#define FIBONACCI_FUNCTIONS 1
int fibonacciRecursive(int n, int* error);
int fibonacciIterative(int n, int* error);
clock_t computeRuntimeFibonacci(int (*fibonacci)(int, int*), int n, int* returnResult, int* returnError);
#endif