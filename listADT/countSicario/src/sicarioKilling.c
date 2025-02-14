#include <stdint.h>

#include "circularList.h"

void nothing(void* _) {
    return;
}

intptr_t sicarioKilling(int n, int m) {
    CircularList* list = circularListInitWithDestructor(&nothing);
    for (intptr_t i = 1; i < n + 1; i++) {
        circularListAppend(list, (void*)i);
    }

    CircularListPosition* iter = circularListFirst(list);
    while (circularListSize(list) != 1) {
        circularListPosMove(&iter, m - 1);
        CircularListPosition* victim = iter;
        circularListPosMove(&iter, 1);
        circularListRemoveNode(list, &victim);
    }

    intptr_t lastOneStanding = (intptr_t)circularListPosGetData(iter);
    circularListFree(&list);
    return lastOneStanding;
}
