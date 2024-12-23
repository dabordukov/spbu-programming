#ifndef SINGLE_LINKED_LIST_H_
#define SINGLE_LINKED_LIST_H_

#include <stddef.h>

typedef struct ListNode ListNode;

typedef struct List List;

/* Create list */
List* listInit();

/* Add element in the end of the list
   Return 1 on invalid list or string
   Return 2 on allocation error */
int listAppend(List* list, char* string);

/* Remove last element of the list */
int listRemoveLast(List* list);

/* Return 0 on success
   Return 1 on empty list */
int listRemoveLast(List* list);

/* Return NULL if the node is NULL
   Return NULL if the node is the last element
   Otherwise, return pointer to thenext node */
ListNode* listNodeNext(ListNode* node);

/* Return NULL if the list is NULL
   Return NULL if the list is empty
   Otherwise, return pointer to the first node */
ListNode* listFirst(List* list);

/* Return NULL if the list is NULL
   Return NULL if the list is empty
   Otherwise, return pointer to the last node */
ListNode* listLast(List* list);

/* Return NULL if the node is NULL
   Otherwise, return pointer to the string in the node */
char* listNodeGetData(ListNode* node);

/* Return size of list
   Return 0 if the list is NULL */
size_t listSize(List* list);

/* Free list */
void freeList(List* list);

#endif
