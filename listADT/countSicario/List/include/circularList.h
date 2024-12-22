#ifndef CIRCULAR_LIST_H
#define CIRCULAR_LIST_H
#include <stdbool.h>
#include <stddef.h>
#include <stdio.h>

typedef struct List CircularList;

typedef struct CircularListPosition CircularListPosition;

/*pointer to the list node*/
typedef CircularListPosition* pCircularListPos;

/* free() is set to default destructor for deleting data from circular list nodes*/
CircularList* circularListInit();

/* dataDestructor is set to default for deleting data from circular list nodes */
CircularList* circularListInitWithDestructor(void (*dataDestructor)(void*));

/* Delete circular list and its nodes
   dataDestructor is applied for every data in nodes*/
void circularListFree(CircularList** list);

/* Returns the number of elements in the circular list*/
size_t circularListSize(CircularList* list);

/* Return true if the circular list is empty, otherwise return false*/
bool circularListIsEmpty(CircularList* list);

/* Return data from node*/
void* circularListPosGetData(CircularListPosition* pos);

/* Return position of the first node
   If the circular list is empty returns NULL*/
CircularListPosition* circularListFirst(CircularList* list);

/* Return position of the last node
   If the circular list is empty returns NULL*/
CircularListPosition* circularListLast(CircularList* list);

/* Returns NULL if there is no POS node in circular list, last node if POS node is the first node
   Otherwise return position of the node previous to POS node*/
CircularListPosition* circularListPrevNode(CircularList* list, CircularListPosition* pos);

/*Returns NULL if POS node is NULL, first node if POS node is the last node
Otherwise return position of the node next to POS node */
CircularListPosition* circularListNextNode(CircularListPosition* pos);

/* Search node with data equals DATA. Compare equality with compareData function.
   Return position of the first such node
   If not found return NULL */
CircularListPosition* circularListFindData(CircularList* list, void* data, bool (*compareData)(void*, void*));

/* Move POS pointer next STEPS times
   Return number of successful steps,
   return value is less than passed STEPS parameter if the circular list end is encountered*/
size_t circularListPosMove(CircularListPosition** pos, size_t steps);

/* Insert node with DATA after the POS node
   If pos==NULL, insert node in the beginning of the circular list*/
CircularListPosition* circularListInsertAfter(CircularList* list, void* data, CircularListPosition* pos);

/* Insert node with DATA in the end of the circular lists*/
CircularListPosition* circularListAppend(CircularList* list, void* data);

/* Insert node with DATA and custom dataDestructor after the POS node*/
CircularListPosition* circularListInsertAfterWithDestructor(CircularList* list, void* data, CircularListPosition* pos, void (*dataDestructor)(void*));

/* Insert node with DATA and custom dataDestructor in the end of the circular list*/
CircularListPosition* circularListAppendWithDestructor(CircularList* list, void* data, void (*dataDestructor)(void*));

/* Delete node from circular list*/
void circularListRemoveNode(CircularList* list, CircularListPosition** pos);

/* Applies printData(void*) to data in every node*/
void circularListPrint(CircularList* list, void (*printData)(void*));

#endif