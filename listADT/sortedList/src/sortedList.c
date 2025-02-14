#include <stdint.h>

#include "list.h"
bool compareInt(void* dataL, void* dataR) {
    return (intptr_t)dataL == (intptr_t)dataR;
}

bool comparatorInsertAscendingInt(void* dataList, void* dataInsert) {
    return (intptr_t)dataList >= (intptr_t)dataInsert;
}

bool comparatorInsertDescendingInt(void* dataList, void* dataInsert) {
    return (intptr_t)dataList <= (intptr_t)dataInsert;
}

void printListInt(void* data) {
    printf("%ld ", (intptr_t)data);
}

void nothing(void* data) {
    return;
}

ListPosition* listInsertAfterSorted(List* list, void* data, bool (*comparator)(void*, void*)) {
    ListPosition* pos = listFindData(list, data, comparator);
    if (pos != NULL) {
        pos = listPrevNode(list, pos);
        return listInsertAfter(list, data, pos);
    } else {
        return listAppend(list, data);
    }
}