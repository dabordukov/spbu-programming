#include <stdint.h>

#include "circularList.h"

void nothing(void* _) {
    return;
}

intptr_t sicarioKilling(int n, int m) {
    CircList* list = circListInitWithDestructor(&nothing);
    for (intptr_t i = 1; i < n + 1; i++) {
        circListAppend(list, (void*)i);
    }

    CircListPosition* iter = circListFirst(list);
    while (circListSize(list) != 1) {
        circListPosMove(&iter, m - 1);
        CircListPosition* victim = iter;
        circListPosMove(&iter, 1);
        circListRemoveNode(list, &victim);
    }

    intptr_t lastOneStanding = (intptr_t)circListPosGetData(iter);
    circListFree(&list);
    return lastOneStanding;
}
