#include <locale.h>
#include <stdbool.h>
#include <stdio.h>
#include <wchar.h>

bool isPalindrome(wchar_t* string) {
    int size = wcslen(string);
    int left = 0;
    int right = size - 1;
    while (left < right) {
        if (string[left] == L' ') {
            left++;
            continue;
        }

        if (string[right] == L' ') {
            right--;
            continue;
        }
        if (string[left] != string[right]) {
            return false;
        }

        left++;
        right--;
    }
    return true;
}

int main() {
    setlocale(LC_ALL, "ru_RU.utf8");
    wchar_t string[] = L"я иду с мечем судия";
    if (isPalindrome(string)) {
        printf("Palindrome\n");
    }
}