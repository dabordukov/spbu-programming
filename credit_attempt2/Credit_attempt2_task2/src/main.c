#include <locale.h>
#include <stdio.h>

#include "list.h"
#include "listFunctions.h"

void printList(List* list) {
    ListNode* iter = listFirst(list);
    while (iter != NULL) {
        char* string = listNodeGetData(iter);
        printf("%s ", string);
        iter = listNodeNext(iter);
    }
    printf("\n");
}

int main(void) {
#ifdef _WIN32
    system("chcp 65001");
    setlocale(LC_CTYPE, "Ru.UTF-8");
#else
    setlocale(LC_CTYPE, "");
#endif

    char startElements[5][5] = {"aba", "bab", "aac", "asdq", "aaaa"};
    List* list = listInit();
    if (list == NULL) {
        fprintf(stderr, "Can't allocate list");
        return 1;
    }

    for (int i = 0; i < 3; i++) {
        if (listAppend(list, startElements[i]) == 2) {
            fprintf(stderr, "Can't allocate node");
            return 1;
        }
    }

    printf("Исходный список:\n");
    printList(list);

    listAppendStringsBeginningWithA(list);

    printf("Новый список:\n");
    printList(list);
}
