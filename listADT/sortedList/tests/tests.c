#include <assert.h>
#include <stdint.h>

#include "list.h"
#include "sortedList.h"

bool compareAscendingInt(void* dataL, void* dataR) {
    return (intptr_t)dataL <= (intptr_t)dataR;
}

int main() {
    List* list = listInitWithDestructor(&nothing);
    listInsertAfterSorted(list, (void*)555, &comparatorInsertAscendingInt);
    listInsertAfterSorted(list, (void*)123, &comparatorInsertAscendingInt);
    listInsertAfterSorted(list, (void*)1, &comparatorInsertAscendingInt);
    listInsertAfterSorted(list, (void*)999, &comparatorInsertAscendingInt);
    listInsertAfterSorted(list, (void*)999, &comparatorInsertAscendingInt);
    listInsertAfterSorted(list, (void*)-50, &comparatorInsertAscendingInt);
    assert(listIsSorted(list, &compareAscendingInt));
    assert(listPosGetData(listFirst(list)) == (void*)-50);
    assert(listPosGetData(listLast(list)) == (void*)999);
}