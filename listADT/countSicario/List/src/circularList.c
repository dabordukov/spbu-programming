#include <stdbool.h>
#include <stdio.h>
#include <stdlib.h>

#include "list.h"

typedef struct ListNode {
    void* data;
    struct ListNode* next;
    void (*dataDestructor)(void*);
} ListNode;

typedef ListNode ListPosition;
typedef ListNode CircularListPosition;

typedef struct List {
    size_t size;
    ListNode* first;
    ListNode* last;
    void (*defaultDestructor)(void*);
} List;

typedef List CircularList;

static ListNode* createNode(void* data, void (*dataDestructor)(void*)) {
    ListNode* newNode = calloc(1, sizeof(ListNode));
    if (!newNode) {
        return NULL;
    }
    newNode->data = data;
    newNode->dataDestructor = dataDestructor;

    return newNode;
}

CircularList* circularListInitWithDestructor(void (*dataDestructor)(void*)) {
    return listInitWithDestructor(dataDestructor);
}

CircularList* circularListInit() {
    return listInitWithDestructor(&free);
}

size_t circularListSize(CircularList* list) {
    return list->size;
}

bool circularListIsEmpty(CircularList* list) {
    return listIsEmpty(list);
}

void* circularListPosGetData(CircularListPosition* pos) {
    return listPosGetData(pos);
}

CircularListPosition* circularListInsertAfterWithDestructor(CircularList* list, void* data, CircularListPosition* pos, void (*dataDestructor)(void*)) {
    ListNode* newNode = createNode(data, dataDestructor);
    if (listIsEmpty(list)) {
        list->first = newNode;
        list->last = newNode;
        newNode->next = list->first;
        list->size++;
        return newNode;
    }

    if (pos == NULL) {  // insert in the beginning
        newNode->next = list->first;
        list->first = newNode;
        list->last->next = newNode;
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

CircularListPosition* circularListInsertAfter(CircularList* list, void* data, CircularListPosition* pos) {
    return circularListInsertAfterWithDestructor(list, data, pos, list->defaultDestructor);
}

CircularListPosition* circularListAppendWithDestructor(CircularList* list, void* data, void (*dataDestructor)(void*)) {
    ListNode* newNode = createNode(data, dataDestructor);

    if (listIsEmpty(list)) {
        list->first = newNode;
        newNode->next = newNode;
    } else {
        list->last->next = newNode;
        newNode->next = list->first;
    }
    list->last = newNode;
    list->size++;
    return newNode;
}

CircularListPosition* circularListAppend(CircularList* list, void* data) {
    return circularListAppendWithDestructor(list, data, list->defaultDestructor);
}

CircularListPosition* circularListFindData(CircularList* list, void* data, bool (*compareData)(void*, void*)) {
    if (listIsEmpty(list)) {
        return NULL;
    }

    ListNode* iter = list->first;
    for (size_t i = 0; i < list->size; i++) {
        if (compareData(iter->data, data)) {
            return (CircularListPosition*)iter;
        }
        iter = iter->next;
    }

    return NULL;
}

CircularListPosition* circularListNextNode(CircularListPosition* pos) {
    return listNextNode(pos);
}

CircularListPosition* circularListPrevNode(CircularList* list, CircularListPosition* pos) {
    if (circularListIsEmpty(list) || pos == NULL) {
        return NULL;
    }
    if (pos == list->first) {
        return list->last;
    }

    CircularListPosition* prev = list->first;
    for (size_t i = 0; i < list->size; i++) {
        if (prev->next == pos) {
            return prev;
        }
        prev = prev->next;
    }

    return NULL;
}

void circularListRemoveNode(CircularList* list, CircularListPosition** pos) {
    if (*pos == NULL) {
        return;
    }

    CircularListPosition* prev = circularListPrevNode(list, *pos);
    if (prev == NULL) {
        return;
    }

    if (prev == *pos) {
        list->first = NULL;
        list->last = NULL;
    } else {
        if (*pos == list->first) {
            list->first = (*pos)->next;
        } else if (*pos == list->last) {
            list->last = prev;
        }
        prev->next = (*pos)->next;
    }

    (*pos)->dataDestructor((*pos)->data);
    free(*pos);
    list->size--;

    *pos = NULL;
}

void circularListPrint(CircularList* list, void (*printData)(void*)) {
    if (circularListIsEmpty(list)) {
        return;
    }

    CircularListPosition* iter = list->first;
    for (size_t i = 0; i < list->size; i++) {
        printData(iter->data);
        iter = iter->next;
    }
}

size_t circularListPosMove(CircularListPosition** pos, size_t steps) {
    if (*pos == NULL) {
        return 0;
    }

    for (size_t countSteps = 0; countSteps < steps; countSteps++) {
        (*pos) = (*pos)->next;
    }

    return steps;
}

CircularListPosition* circularListFirst(CircularList* list) {
    return list->first;
}

CircularListPosition* circularListLast(CircularList* list) {
    return list->last;
}

void circularListFree(CircularList** list) {
    while (!circularListIsEmpty(*list)) {
        ListPosition* last = (*list)->last;
        circularListRemoveNode(*list, &last);
    }

    free(*list);
    *list = NULL;
}
