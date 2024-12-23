#include <assert.h>
#include <string.h>

#include "list.h"
#include "listFunctions.h"

#define ASSERT_STR(x, y) assert(strcmp(x, y) == 0)

void testCase1(void) {
    char push[3][2] = {"a", "b", "c"};
    List* list = listInit();
    assert(list != NULL);

    for (int i = 0; i < 3; i++) {
        assert(listAppend(list, push[i]) == 0);
    }
    listAppendStringsBeginningWithA(list);

    assert(listSize(list) == 4);

    ListNode* iter = listFirst(list);
    for (int i = 0; i < 3; i++) {
        ASSERT_STR(listNodeGetData(iter), push[i]);
        iter = listNodeNext(iter);
    }
    ASSERT_STR(listNodeGetData(iter), "a");
}

int main(void) {
    testCase1();
}
