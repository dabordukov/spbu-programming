#include "list.h"

#include <stddef.h>
#include <stdlib.h>
#include <string.h>

typedef struct ListNode {
    char* string;
    struct ListNode* next;
} ListNode;

typedef struct List {
    struct ListNode* first;
    struct ListNode* last;
    size_t size;
} List;

List* listInit() {
    return calloc(1, sizeof(List));
}

static ListNode* createListNode(char* string) {
    ListNode* newNode = calloc(1, sizeof(ListNode));
    if (newNode == NULL) {
        return NULL;
    }

    size_t stringLength = strlen(string);
    char* newString = calloc(stringLength + 1, sizeof(char));
    if (newString == NULL) {
        free(newNode);
        return NULL;
    }

    strcpy(newString, string);

    newNode->string = newString;

    return newNode;
}

int listAppend(List* list, char* string) {
    if (list == NULL || string == NULL) {
        return 1;
    }

    ListNode* newNode = createListNode(string);
    if (newNode == NULL) {
        return 2;
    }

    if (list->last == NULL) {
        list->last = newNode;
    } else {
        list->last->next = newNode;
        list->last = newNode;
    }

    if (list->size == 0) {
        list->first = newNode;
    }

    list->size++;

    return 0;
}

static void freeNodeList(ListNode* node) {
    free(node->string);
    free(node);
}

int listRemoveLast(List* list) {
    ListNode* iter = list->first;
    if (iter == NULL) {
        return 1;
    }

    ListNode* end = list->last;
    while (iter->next != end) {
        iter = iter->next;
    }
    freeNodeList(end);

    list->last = iter;
    iter->next = NULL;
    list->size--;

    return 0;
}

ListNode* listNodeNext(ListNode* node) {
    if (node == NULL) {
        return NULL;
    }

    return node->next;
}

ListNode* listFirst(List* list) {
    if (list == NULL) {
        return NULL;
    }

    return list->first;
}

ListNode* listLast(List* list) {
    if (list == NULL) {
        return NULL;
    }

    return list->last;
}

char* listNodeGetData(ListNode* node) {
    if (node == NULL) {
        return NULL;
    }

    return node->string;
}

size_t listSize(List* list) {
    if (list == NULL) {
        return 0;
    }

    return list->size;
}

void freeList(List* list) {
    if (list == NULL) {
        return;
    }

    while (list->last != NULL) {
        freeNodeList(list->last);
    }
    free(list);
}