#include <assert.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#include "../include/list.h"

void printStr(void* data) {
    puts((const char*)data);
}

bool compareSTR(void* data1, void* data2) {
    return strcmp((const char*)data1, (const char*)data2) == 0;
}

void nothing() {
    return;
}

#define ASSERT_STR(x, y) assert(strcmp((char*)x, (char*)y) == 0)

void testListInitFree() {
    List* list = listInitWithDestructor(&nothing);
    assert(list);
    listAppend(list, "1234");
    listFree(&list);
    assert(list == NULL);
}

void testListAppendInsert() {
    List* list = listInitWithDestructor(&nothing);

    pListPos first = listAppend(list, "1234");
    pListPos second = listAppend(list, "5678");
    listInsertAfter(list, "999", listNextNode(list, second));  // pos==NULL
    ASSERT_STR(listPosGetData(first), "1234");
    ASSERT_STR(listPosGetData(second), "5678");

    ASSERT_STR(listPosGetData(listNextNode(list, first)), "5678");
    ASSERT_STR(listPosGetData(listPrevNode(list, second)), "1234");
    ASSERT_STR(listPosGetData(listFirst(list)), "999");
    listInsertAfter(list, "44", first);
    ASSERT_STR(listPosGetData(listNextNode(list, first)), "44");

    listFree(&list);
}

void testListFind() {
    List* list = listInitWithDestructor(&nothing);
    assert(listFindData(list, "1", &compareSTR) == NULL);

    char str[10][2];
    for (int i = 0; i < 10; i++) {
        sprintf(str[i], "%d", i);
        listAppend(list, str[i]);
    }

    ASSERT_STR(listPosGetData(listFindData(list, "0", &compareSTR)), "0");
    ASSERT_STR(listPosGetData(listFindData(list, "9", &compareSTR)), "9");
    ASSERT_STR(listPosGetData(listFindData(list, "4", &compareSTR)), "4");
    assert(listFindData(list, "11", &compareSTR) == NULL);
}

void testListRemoveNode() {
    List* list = listInitWithDestructor(&nothing);

    char str[10][2];
    for (int i = 0; i < 10; i++) {
        sprintf(str[i], "%d", i);
        listAppend(list, str[i]);
    }

    // mid element
    {
        pListPos find = listFindData(list, "5", &compareSTR);
        pListPos prev = listPrevNode(list, find);
        pListPos next = listNextNode(list, find);
        listRemoveNode(list, &find);
        assert(find == NULL);
        assert(listFindData(list, "5", &compareSTR) == NULL);

        // checks that .next pointer was set correctly
        assert(listPrevNode(list, next) == prev);
        assert(listNextNode(list, prev) == next);
    }

    // first element
    {
        pListPos find = listFindData(list, "0", &compareSTR);
        pListPos prev = listPrevNode(list, find);
        pListPos next = listNextNode(list, find);
        listRemoveNode(list, &find);
        assert(find == NULL);
        assert(listFindData(list, "0", &compareSTR) == NULL);

        // checks that .first pointer was set correctly
        assert(listFirst(list) == next);
        assert(listPrevNode(list, next) == prev);
    }

    // last element
    {
        pListPos find = listFindData(list, "9", &compareSTR);
        pListPos prev = listPrevNode(list, find);
        pListPos next = listNextNode(list, find);
        listRemoveNode(list, &find);
        assert(find == NULL);
        assert(listFindData(list, "9", &compareSTR) == NULL);

        // checks that .next and .last pointers were set correctly
        assert(next == NULL);
        assert(listNextNode(list, prev) == next);
        assert(listLast(list) == prev);
    }
}

int main() {
    testListInitFree();
    testListAppendInsert();
    testListFind();
    testListRemoveNode();
}
