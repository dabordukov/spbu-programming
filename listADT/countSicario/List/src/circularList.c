#define INTERNALS_CIRCULAR_LIST_H
#include <stdbool.h>
#include <stdio.h>
#include <stdlib.h>

typedef struct ListNode {
    void* data;
    struct ListNode* next;
    void (*dataDestructor)(void*);
} ListNode;
typedef ListNode ListPosition;
typedef ListPosition CircListPosition;
#include "list.h"

typedef struct List {
    size_t size;
    ListNode* first;
    ListNode* last;
    void (*defaultDestructor)(void*);
} List;
typedef List CircList;

static ListNode* createNode(void* data, void (*dataDestructor)(void*)) {
    ListNode* newNode = calloc(1, sizeof(ListNode));
    if (!newNode) {
        return NULL;
    }
    newNode->data = data;
    newNode->dataDestructor = dataDestructor;

    return newNode;
}

CircList* circListInitWithDestructor(void (*dataDestructor)(void*)) {
    return listInitWithDestructor(dataDestructor);
}

CircList* circListInit() {
    return listInitWithDestructor(&free);
}

size_t circListSize(CircList* list) {
    return list->size;
}

bool circListIsEmpty(CircList* list) {
    return listIsEmpty(list);
}

void* circListPosGetData(CircListPosition* pos) {
    return listPosGetData(pos);
}

CircListPosition* circListInsertAfterWithDestructor(CircList* list, void* data, CircListPosition* pos, void (*dataDestructor)(void*)) {
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

CircListPosition* circListInsertAfter(CircList* list, void* data, CircListPosition* pos) {
    return circListInsertAfterWithDestructor(list, data, pos, list->defaultDestructor);
}

CircListPosition* circListAppendWithDestructor(CircList* list, void* data, void (*dataDestructor)(void*)) {
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

CircListPosition* circListAppend(CircList* list, void* data) {
    return circListAppendWithDestructor(list, data, list->defaultDestructor);
}

CircListPosition* circListFindData(CircList* list, void* data, bool (*compareData)(void*, void*)) {
    if (listIsEmpty(list)) {
        return NULL;
    }

    ListNode* iter = list->first;
    for (size_t i = 0; i < list->size; i++) {
        if (compareData(iter->data, data)) {
            return (CircListPosition*)iter;
        }
        iter = iter->next;
    }

    return NULL;
}

CircListPosition* circListNextNode(CircListPosition* pos) {
    return listNextNode(pos);
}

CircListPosition* circListPrevNode(CircList* list, CircListPosition* pos) {
    if (circListIsEmpty(list) || pos == NULL) {
        return NULL;
    }
    if (pos == list->first) {
        return list->last;
    }

    CircListPosition* prev = list->first;
    for (size_t i = 0; i < list->size; i++) {
        if (prev->next == pos) {
            return prev;
        }
        prev = prev->next;
    }

    return NULL;
}

void circListRemoveNode(CircList* list, CircListPosition** pos) {
    if (*pos == NULL) {
        return;
    }

    CircListPosition* prev = circListPrevNode(list, *pos);
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

void circListPrint(CircList* list, void (*printData)(void*)) {
    if (circListIsEmpty(list)) {
        return;
    }

    CircListPosition* iter = list->first;
    for (size_t i = 0; i < list->size; i++) {
        printData(iter->data);
        iter = iter->next;
    }
}

size_t circListPosMove(CircListPosition** pos, size_t steps) {
    if (*pos == NULL) {
        return 0;
    }

    for (int countSteps = 0; countSteps < steps; countSteps++) {
        (*pos) = (*pos)->next;
    }

    return steps;
}

CircListPosition* circListFirst(CircList* list) {
    return list->first;
}

CircListPosition* circListLast(CircList* list) {
    return list->last;
}

void circListFree(CircList** list) {
    while (!circListIsEmpty(*list)) {
        ListPosition* last = (*list)->last;
        circListRemoveNode(*list, &last);
    }

    free(*list);
    *list = NULL;
}
