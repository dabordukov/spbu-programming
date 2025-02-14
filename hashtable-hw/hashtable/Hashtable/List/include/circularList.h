#ifndef CIRCULAR_LIST_H
#define CIRCULAR_LIST_H
#include <stdbool.h>
#include <stddef.h>
#include <stdio.h>

typedef struct List CircList;

typedef void CircListPosition;
/*pointer to the list node*/
typedef CircListPosition* pCircListPos;

/* free() is set to default destructor for deleting data from circular list nodes*/
CircList* circListInit();

/* dataDestructor is set to default for deleting data from circular list nodes */
CircList* circListInitWithDestructor(void (*dataDestructor)(void*));

/* Delete circular list and its nodes
   dataDestructor is applied for every data in nodes*/
void circListFree(CircList** list);

/* Returns the number of elements in the circular list*/
size_t circListSize(CircList* list);

bool circListIsEmpty(CircList* list);

void* circListPosGetData(CircListPosition* pos);

/* Return position of the first node
   If the circular list is empty returns NULL*/
CircListPosition* circListFirst(CircList* list);

/* Return position of the last node
   If the circular list is empty returns NULL*/
CircListPosition* circListLast(CircList* list);

/* Returns NULL if there is no POS node in list or POS node is the first node
   Otherwise return position of the node previous to POS node*/
CircListPosition* circListPrevNode(CircList* list, CircListPosition* pos);

/*Returns NULL if POS node is NULL or POS node is the last node
Otherwise return position of the node next to POS node */
CircListPosition* circListNextNode(CircListPosition* pos);

/* Search node with data equals DATA. Compare equality with compareData function.
   Return position of the first such node
   If not found return NULL */
CircListPosition* circListFindData(CircList* list, void* data, bool (*compareData)(void*, void*));

/* Move POS pointer next STEPS times
   Return number of successful steps,
   return value is less than passed STEPS parameter if the circular list end is encountered*/
size_t circListPosMove(CircListPosition** pos, size_t steps);

/* Insert node with DATA after the POS node
   If pos==NULL, insert node in the beginning of the circular list*/
CircListPosition* circListInsertAfter(CircList* list, void* data, CircListPosition* pos);
/* Insert node with DATA in the end of the circular lists*/
CircListPosition* circListAppend(CircList* list, void* data);

/* Insert node with DATA and custom dataDestructor after the POS node*/
CircListPosition* circListInsertAfterWithDestructor(CircList* list, void* data, CircListPosition* pos, void (*dataDestructor)(void*));
/* Insert node with DATA and custom dataDestructor in the end of the circular list*/
CircListPosition* circListAppendWithDestructor(CircList* list, void* data, void (*dataDestructor)(void*));

/* Delete node from circular list*/
void circListRemoveNode(CircList* list, CircListPosition** pos);

/* Applies printData(void*) to data in every node*/
void circListPrint(CircList* list, void (*printData)(void*));

#endif