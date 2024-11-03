
#include <assert.h>
#include <stdarg.h>
#include <stdbool.h>
#include <stdio.h>
#include <stdlib.h>

typedef struct ListNode {
    void* data;
    struct ListNode* next;
    void (*dataDestructor)(void*);
} ListNode;
typedef ListNode ListPosition;

typedef struct List {
    size_t size;
    ListNode* first;
    ListNode* last;
    void (*defaultDestructor)(void*);
} List;

static ListNode* createNode(void* data, void (*dataDestructor)(void*)) {
    ListNode* newNode = calloc(1, sizeof(ListNode));
    if (!newNode) {
        return NULL;
    }
    newNode->data = data;
    newNode->dataDestructor = dataDestructor;

    return newNode;
}

/* dataDestructor is set to default for deleting data from list nodes */
List* listInitWithDestructor(void (*dataDestructor)(void*)) {
    List* list = calloc(1, sizeof(List));
    list->defaultDestructor = dataDestructor;
    return list;
}

List* listInit() {
    return listInitWithDestructor(&free);
}

size_t listSize(List* list) {
    return list->size;
}

bool listIsEmpty(List* list) {
    return list->size == 0;
}

void* listPosGetData(ListPosition* pos) {
    if (!pos) {
        return NULL;
    }

    return pos->data;
}

ListPosition* listInsertAfterWithDestructor(List* list, void* data, ListPosition* pos, void (*dataDestructor)(void*)) {
    ListNode* newNode = createNode(data, dataDestructor);
    if (listIsEmpty(list)) {
        list->first = newNode;
        list->last = newNode;
        list->size++;
        return newNode;
    }

    if (pos == NULL) {  // insert in the beginning
        newNode->next = list->first;
        list->first = newNode;
    } else {
        newNode->next = pos->next;
        pos->next = newNode;
        if (list->last == pos) {
            list->last = newNode;
        }
    }

    list->size++;
    return newNode;
}

/* Insert node with DATA after the POS node*/
ListPosition* listInsertAfter(List* list, void* data, ListPosition* pos) {
    return listInsertAfterWithDestructor(list, data, pos, list->defaultDestructor);
}

ListPosition* listAppendWithDestructor(List* list, void* data, void (*dataDestructor)(void*)) {
    ListNode* newNode = createNode(data, dataDestructor);

    if (listIsEmpty(list)) {
        list->first = newNode;
    } else {
        list->last->next = newNode;
    }
    list->last = newNode;
    list->size++;
    return newNode;
}

/* Insert node with DATA in the end of the lists*/
ListPosition* listAppend(List* list, void* data) {
    return listAppendWithDestructor(list, data, list->defaultDestructor);
}

/* Return NULL on empty list or if there is no any node with such DATA
   Otherwise return position of the node with DATA*/
ListPosition* listFindData(List* list, void* data, bool (*compareData)(void*, void*)) {
    if (listIsEmpty(list)) {
        return NULL;
    }

    ListNode* iter = list->first;
    for (; iter->next != NULL && !compareData(iter->data, data); iter = iter->next);
    if (!compareData(iter->data, data)) {
        return NULL;
    }

    return (ListPosition*)iter;
}

/*Returns NULL if there is no POS node in list or POS node is the last node
  Otherwise return position of the node next to POS node*/
ListPosition* listNextNode(List* list, ListPosition* pos) {
    if (pos == NULL) {
        return NULL;
    }

    return pos->next;
}

/*Returns NULL if there is no POS node in list or POS node is the first node
  Otherwise return position of the node previous to POS node*/
ListPosition* listPrevNode(List* list, ListPosition* pos) {
    if (listIsEmpty(list) || pos == NULL) {
        return NULL;
    }

    ListPosition* prev = list->first;
    for (; prev->next != pos && prev->next != NULL; prev = prev->next);
    if (prev->next == pos) {
        return prev;
    }

    return NULL;
}

/* Delete node from list*/
void listRemoveNode(List* list, ListPosition** pos) {
    if (*pos == NULL) {
        return;
    }

    ListPosition* prev = listPrevNode(list, *pos);
    if (prev == NULL) {
        list->first = (*pos)->next;
    } else {
        prev->next = (*pos)->next;
    }

    if ((*pos)->next == NULL) {
        assert(list->last == *pos);
        list->last = prev;
    }

    (*pos)->dataDestructor((*pos)->data);
    free(*pos);
    list->size--;

    *pos = NULL;
}

void listPrint(List* list, void (*printData)(void*)) {
    if (listIsEmpty(list)) {
        return;
    }

    for (ListPosition* iter = list->first; iter != NULL; iter = iter->next) {
        printData(iter->data);
    }
}

void listFree(List** list) {
    while (!listIsEmpty(*list)) {
        ListPosition* last = (*list)->last;
        listRemoveNode(*list, &last);
    }

    free(*list);
    *list = NULL;
}

ListPosition* listFirst(List* list) {
    return list->first;
}

ListPosition* listLast(List* list) {
    return list->last;
}

size_t listPosMove(ListPosition** pos, size_t steps) {
    if (*pos == NULL) {
        return 0;
    }

    int countSteps = 0;
    for (; countSteps < steps; countSteps++) {
        if ((*pos)->next != NULL) {
            *pos = (*pos)->next;
        } else {
            break;
        }
    }

    return countSteps;
}

static void swapNodes(List* list, ListPosition* a, ListPosition* b) {
    if (a == b) {
        return;
    }

    assert(a != NULL && b != NULL);

    void* dataTemp = a->data;
    a->data = b->data;
    b->data = dataTemp;

    void (*destructorTemp)(void*) = a->dataDestructor;
    a->dataDestructor = b->dataDestructor;
    b->dataDestructor = destructorTemp;
}

static void mergeSort(List* list, bool (*compareData)(void*, void*), ListPosition* pos, size_t subListSize) {
    if (subListSize <= 1) {
        return;
    }
    size_t secondSubListSize = subListSize - subListSize / 2;
    mergeSort(list, compareData, pos, subListSize / 2);
    ListPosition* subListMid = pos;
    assert(listPosMove(&subListMid, subListSize / 2) == subListSize / 2);
    mergeSort(list, compareData, subListMid, secondSubListSize);

    ListPosition* ptr1 = pos;
    ListPosition* ptr2 = subListMid;
    ListPosition* end = subListMid;
    if (listPosMove(&end, secondSubListSize) < secondSubListSize) {
        end = NULL;
    }
    if (subListSize == 13) {
        printf("13");
    }
    while (ptr1 != ptr2 && ptr2 != end) {
        if (!compareData(ptr1->data, ptr2->data)) {
            swapNodes(list, ptr1, ptr2);
            listPrint(list, &puts);
            puts("");
        }
        ptr1 = ptr1->next;
    }
}

void listMergeSort(List* list, bool (*compareData)(void*, void*)) {
    if (list != NULL) {
        mergeSort(list, compareData, list->first, list->size);
    }
}

bool listIsSorted(List* list, bool (*compareData)(void*, void*)) {
    if (list->size < 2) {
        return true;
    }

    for (ListPosition* pos = list->first; pos->next != NULL; pos = pos->next) {
        if (!compareData(pos->data, pos->next->data)) {
            return false;
        }
    }

    return true;
}