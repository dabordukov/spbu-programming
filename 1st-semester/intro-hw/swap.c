/*
Swap
Поменять значения двух целочисленных переменных местами (без привлечения третьей переменной и файлов)
*/
#include <stdio.h>
#include <stdlib.h>
void swap(int* a, int* b) {
	if (a != b) {
		*a ^= *b;
		*b ^= *a;
		*a ^= *b;
	}
}

int main() {
	int a, b;
	if (scanf("%d %d", &a, &b) < 2) {
		printf("Invalid values\n");
		exit(1);
	}
	swap(&a, &b);
	printf("%d %d", a, b);
}