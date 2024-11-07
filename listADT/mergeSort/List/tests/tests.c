#include <assert.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#include "list.h"

void printStr(void* data) {
    puts((const char*)data);
}

bool equalStr(void* data1, void* data2) {
    return strcmp((const char*)data1, (const char*)data2) == 0;
}

bool compareStrAscending(void* dataL, void* dataR) {
    return strcmp((const char*)dataL, (const char*)dataR) <= 0;
}

void nothing(void* data) {
    return;
}

void testCaseSortedList() {
    List* list = listInitWithDestructor(&nothing);

    listAppend(list, "1");
    listMergeSort(list, &compareStrAscending);
    assert(listIsSorted(list, compareStrAscending));

    listAppend(list, "2");
    listMergeSort(list, &compareStrAscending);
    assert(listIsSorted(list, compareStrAscending));

    listAppend(list, "3");
    listMergeSort(list, &compareStrAscending);
    assert(listIsSorted(list, compareStrAscending));

    char str[10][3];
    for (int i = 4; i < 14; i++) {
        sprintf(str[i - 4], "%d", i);
        listAppend(list, str[i - 4]);
    }

    listMergeSort(list, &compareStrAscending);

    assert(listIsSorted(list, compareStrAscending));

    listFree(&list);
}

void testCaseReverseSortedList() {
    List* list = listInitWithDestructor(&nothing);

    char str[100][3];
    for (int i = 99; i >= 0; i--) {
        sprintf(str[i], "%d", i);
        listAppend(list, str[i]);
    }

    listMergeSort(list, &compareStrAscending);
    assert(listIsSorted(list, compareStrAscending));
    listFree(&list);
}

void testCaseRandomData() {
    List* list = listInitWithDestructor(&nothing);
    FILE* testFile = fopen(RANDOM_DATA_PATH, "r");
    assert(testFile != NULL);
    char numbers[1000][5];
    for (int i = 0; i < 1000; i++) {
        assert(fscanf(testFile, "%4s", numbers[i]) > 0);
        listAppend(list, &(numbers[i]));
    }

    fclose(testFile);

    listMergeSort(list, &compareStrAscending);
    assert(listIsSorted(list, &compareStrAscending));
    listFree(&list);
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
    listInsertAfter(list, "999", listNextNode(second));  // pos==NULL
    ASSERT_STR(listPosGetData(first), "1234");
    ASSERT_STR(listPosGetData(second), "5678");

    ASSERT_STR(listPosGetData(listNextNode(first)), "5678");
    ASSERT_STR(listPosGetData(listPrevNode(list, second)), "1234");
    ASSERT_STR(listPosGetData(listFirst(list)), "999");
    listInsertAfter(list, "44", first);
    ASSERT_STR(listPosGetData(listNextNode(first)), "44");

    listFree(&list);

    list = listInit();
    listInsertAfterWithDestructor(list, "1", NULL, &nothing);
    ASSERT_STR(listPosGetData(listFirst(list)), "1");

    listInsertAfterWithDestructor(list, "1337", listLast(list), &nothing);
    ASSERT_STR(listPosGetData(listLast(list)), "1337");
    listFree(&list);
}

void testListFind() {
    List* list = listInitWithDestructor(&nothing);
    assert(listFindData(list, "1", &equalStr) == NULL);

    char str[10][2];
    for (int i = 0; i < 10; i++) {
        sprintf(str[i], "%d", i);
        listAppend(list, str[i]);
    }

    ASSERT_STR(listPosGetData(listFindData(list, "0", &equalStr)), "0");
    ASSERT_STR(listPosGetData(listFindData(list, "9", &equalStr)), "9");
    ASSERT_STR(listPosGetData(listFindData(list, "4", &equalStr)), "4");
    assert(listFindData(list, "11", &equalStr) == NULL);

    listFree(&list);
}

void testListMisc() {
    List* list = listInitWithDestructor(&nothing);

    char str[10][2];
    for (int i = 0; i < 10; i++) {
        sprintf(str[i], "%d", i);
        listAppend(list, str[i]);
    }

    // lisPosMove test
    {
        ListPosition* node = listFindData(list, "0", &equalStr);
        ListPosition* step1 = listFindData(list, "1", &equalStr);
        ListPosition* step2more = listFindData(list, "3", &equalStr);
        ListPosition* lastNode = listFindData(list, "9", &equalStr);

        assert(listPosMove(&node, 1) == 1);
        assert(step1 == node);
        assert(listPosMove(&node, 2) == 2);
        assert(step2more == node);
        assert(listPosMove(&node, 15) == 6);
        assert(lastNode == node);

        node = NULL;
        assert(listPosMove(&node, 1) == 0);
        assert(node == NULL);
    }

    // listNextNode listPrevNode tests
    {
        assert(listNextNode(NULL) == NULL);
        assert(listPrevNode(list, NULL) == NULL);

        assert(listNextNode(listLast(list)) == NULL);
        assert(listPrevNode(list, listFirst(list)) == NULL);

        assert(listNextNode(listFindData(list, "0", &equalStr)) == listFindData(list, "1", &equalStr));
        assert(listPrevNode(list, listFindData(list, "6", &equalStr)) == listFindData(list, "5", &equalStr));
    }

    // listGetPosData tests
    {
        assert(listPosGetData(NULL) == NULL);
        assert(listPosGetData(listFirst(list)) == str[0]);
        assert(listPosGetData(listLast(list)) == str[9]);
        assert(listPosGetData(listPrevNode(list, listLast(list))) == str[8]);
    }

    listFree(&list);
}

void testListRemoveNode() {
    List* list = listInitWithDestructor(&nothing);

    char str[10][2];
    for (int i = 0; i < 10; i++) {
        sprintf(str[i], "%d", i);
        listAppend(list, str[i]);
    }

    {
        ListPosition* nullNode = NULL;
        size_t listSizeTmp = listSize(list);
        listRemoveNode(list, &nullNode);
        assert(listSize(list) == listSizeTmp);
    }

    // mid element
    {
        pListPos find = listFindData(list, "5", &equalStr);
        pListPos prev = listPrevNode(list, find);
        pListPos next = listNextNode(find);
        listRemoveNode(list, &find);
        assert(find == NULL);
        assert(listFindData(list, "5", &equalStr) == NULL);

        // checks that .next pointer was set correctly
        assert(listPrevNode(list, next) == prev);
        assert(listNextNode(prev) == next);
    }

    // first element
    {
        pListPos find = listFindData(list, "0", &equalStr);
        pListPos prev = listPrevNode(list, find);
        pListPos next = listNextNode(find);
        listRemoveNode(list, &find);
        assert(find == NULL);
        assert(listFindData(list, "0", &equalStr) == NULL);

        // checks that .first pointer was set correctly
        assert(listFirst(list) == next);
        assert(listPrevNode(list, next) == prev);
    }

    // last element
    {
        pListPos find = listFindData(list, "9", &equalStr);
        pListPos prev = listPrevNode(list, find);
        pListPos next = listNextNode(find);
        listRemoveNode(list, &find);
        assert(find == NULL);
        assert(listFindData(list, "9", &equalStr) == NULL);

        // checks that .next and .last pointers were set correctly
        assert(next == NULL);
        assert(listNextNode(prev) == next);
        assert(listLast(list) == prev);
    }

    listFree(&list);
}

int main() {
    testListInitFree();
    testListAppendInsert();
    testListFind();
    testListMisc();
    testListRemoveNode();

    testCaseSortedList();
    testCaseReverseSortedList();
    testCaseRandomData();
}
