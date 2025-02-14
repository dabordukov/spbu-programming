/*
Массив
Дан массив целых чисел [x_1...x_m + n], рассматриваемый как соединение двух его отрезков: 
начала x_1...x_m длины m и конца x_m + 1...x_m + n длины n.

Не используя дополнительных массивов, переставить местами начало и конец.
Делать способом, обсуждавшимся на паре — перевернуть один фрагмент, другой, и весь массив. 
Нужна функция, переворачивающая данный кусок массива.
*/

#include <stdio.h>
#include <stdlib.h>
#include <stdbool.h>

void flush_stdin() {
    int c;
    while ((c = getchar()) != '\n' && c != EOF);
}

void swap(int* a, int* b) {
	if (a != b) {
		*a ^= *b;
		*b ^= *a;
		*a ^= *b;
	}
}

void reverseArray(int* array, int len) {
    int* left = array;
    int* right = array + len - 1;

    while (left < right) {
        swap(left++, right--);
    }
}

void printArray(int* array, int len) {
    for (int i = 0; i < len; i++) {
        printf("%d ", array[i]);
    }
    puts(" ");
}

void flipSegments(int* array, int len, int m) {
    reverseArray(array, m);
    reverseArray(array + m, len - m);
    reverseArray(array, len);
}

int main() {
    int len = 0;
    while (len <= 0 || len > 10) {
        printf("Enter array length (0 < m + n <= 10): ");
        if (scanf("%d", &len) != 1) {
            printf("Invalid value\n");
            flush_stdin();
        }
    }
    
    int array[10] = {0};
    printf("Enter %d numbers:\n", len);
    for (int i = 0; i < len; i++){
        if (scanf("%d", array + i) != 1) {
            printf("Invalid value\n");
            flush_stdin();
            i--;
        }
    }
    
    int m = 0;
    printf("If you want to exit enter Q\n");
    while (true) {
        printf("Enter m: ");
        if (scanf("%d", &m) != 1) {
            int c = getchar();
            if (c == 'Q' || c == 'q') {
                return 0;
            }

            printf("Invalid value\n");
            flush_stdin();
            continue;
        }

        if (m <= 0 || m > len) {
            printf("m should be 1 <= m < %d\n", len);
            continue;
        }
        
        flipSegments(array, len, m);
        printArray(array, len);
    }
}