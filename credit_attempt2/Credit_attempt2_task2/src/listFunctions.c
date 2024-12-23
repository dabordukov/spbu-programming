#include "listFunctions.h"

#include <stddef.h>

#include "list.h"

int listAppendStringsBeginningWithA(List* list) {
    ListNode* iter = listFirst(list);
    ListNode* end = listLast(list);

    while (iter != NULL) {
        char* string = listNodeGetData(iter);
        if (string[0] == 'a') {
            if (listAppend(list, string) != 0) {
                return 1;
            }
        }

        if (iter == end) {
            return 0;
        }
        iter = listNodeNext(iter);
    }

    return 0;
}
