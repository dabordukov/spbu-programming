#include <stdio.h>
#include <string.h>
#include <stdbool.h>

bool isBalanced(char *string) {
	int counter = 0;
	for (int i = 0; i < strlen(string); i++) {
		if (string[i] == ')') {
			if (counter == 0) {
				return 0;
			}
			counter--;
		} else if (string[i] == '(') {
			counter++;
		}
	}
	
	return counter == 0;
}

int main() {
	char string[50];
    printf("Enter string:\n");
	fgets(string, 49, stdin);
	if (isBalanced(string)) {
		printf("[+] Balanced\n");
	} else {
		printf("[-] Not balanced\n");
	}
}