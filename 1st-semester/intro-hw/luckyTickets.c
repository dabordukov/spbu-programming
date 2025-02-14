#include <stdio.h>

int main() {
	int sums[28] = {0};
	for (int a = 0; a < 10; a++) {
		for (int b = 0; b < 10; b++) {
			for (int c = 0; c < 10; c++) {
				sums[a + b + c]++;
			}
		}
	}

	int result = 0;
	for (int i = 0; i < 28; i++){
		result += sums[i] * sums[i];
	}
	printf("%d lucky tickets\n", result);
}