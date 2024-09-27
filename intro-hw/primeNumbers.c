/*
Простые числа
Напишите программу, печатающую все простые числа, не превосходящие заданного числа.
*/

#include <stdbool.h>
#include <stdio.h>
#include <stdlib.h>

void printFirstPrimes(int n) {
    bool* sieve = malloc((n + 1) * sizeof(bool));
    sieve[0] = false;
    sieve[1] = false;
    for (int i = 2; i <= n; i++) {
        sieve[i] = true;
    }

    for (int i = 2; 1ll * i * i <= n; i++) {
        if (sieve[i]) {
            for (int k = i; 1ll * i * k <= n; k++) {
                sieve[i * k] = false;
            }
        }
    }

    for (int i = 2; i <= n; i++) {
        if (sieve[i]) {
            printf("%d ", i);
        }
    }
    puts("");
}

int main() {
    int a = 0;
    printFirstPrimes(1000000);
}