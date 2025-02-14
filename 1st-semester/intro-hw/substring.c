/*
Подстрока
Заданы две строки: S и S1. Найдите количество вхождений S1 в S как подстроки.
*/

#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#define MAX(a,b) (((a) >= (b)) ? (a) : (b))
#define MIN(a,b) (((a) <= (b)) ? (a) : (b))

int* zFunction(char* pattern, char* string) {
    int string_len = strlen(string);
    int pattern_len = strlen(pattern);
    int* zArray = malloc(string_len * sizeof(int));
    memset(zArray, 0, string_len);

    int left = 0;
    int right = 0;
    for (int i = 1; i < string_len; i++) {
        zArray[i] = MAX(0, MIN(right - i, zArray[i - left]));

        while (zArray[i] <= pattern_len && i + zArray[i] <= string_len && pattern[zArray[i]] == string[i + zArray[i]]) {
            zArray[i]++;
        }
        
        if (i + zArray[i] > right) {
            left = i;
            right = i + zArray[i];
        }
    }

    return zArray;
}

int main() {
    char string[] = "The intricate pattern of the ancient mosaic caught my eye, with its repeating pattern of blue and green tiles that seemed to dance across the floor. \
I followed the pattern of the winding staircase to the top of the castle, where I discovered a hidden pattern of symbols etched into the stone wall. \
As I descended back down, I noticed a pattern of similar marks on the ancient bookshelves, each one leading to a new and exciting discovery. \
The pattern of the day's events seemed to be unfolding in a way that was both familiar and yet surprisingly unpredictable, like a puzzle waiting to be solved.";
    char pattern[] = "pattern";

    int count = 0;
    int* zArray = zFunction(pattern, string);
    int str_len = strlen(string);
    int pattern_len = strlen(pattern);
    for (int i = 0; i < str_len; i++) {
        if (zArray[i] == pattern_len) {
            count++;
        }
    }
    free(zArray);

    printf("%d\n", count);
}
