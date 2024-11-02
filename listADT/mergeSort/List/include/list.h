#ifndef LIST_H
#define LIST_H
#include <stdbool.h>
#include <stddef.h>

typedef struct List List;

typedef void ListPosition;
/*pointer to the list node*/
typedef ListPosition* pListPos;

/* free() is set to default destructor for deleting data from list nodes*/
List* listInit();

/* dataDestructor is set to default for deleting data from list nodes */
List* listInitWithDestructor(void (*dataDestructor)(void*));

/* Delete list and its nodes
   dataDestructor is applied for every data in nodes*/
void listFree(List** list);

/* Returns the number of elements in the list*/
size_t listSize(List* list);

bool listIsEmpty(List* list);

void* listPosGetData(ListPosition* pos);

/* Return position of the first node
   If the list is empty returns NULL*/
ListPosition* listFirst(List* list);

/* Return position of the last node
   If the list is empty returns NULL*/
ListPosition* listLast(List* list);

/* Returns NULL if there is no POS node in list or POS node is the first node
   Otherwise return position of the node previous to POS node*/
ListPosition* listPrevNode(List* list, ListPosition* pos);

/*Returns NULL if there is no POS node in list or POS node is the last node
  Otherwise return position of the node next to POS node*/
ListPosition* listNextNode(List* list, ListPosition* pos);

/* Search node with data equals DATA. Compare equality with compareData function.
   Return position of the first such node
   If not found return NULL */
ListPosition* listFindData(List* list, void* data, bool (*compareData)(void*, void*));

/* Insert node with DATA after the POS node
   If pos==NULL, insert node in the beginning of the list*/
ListPosition* listInsertAfter(List* list, void* data, ListPosition* pos);
/* Insert node with DATA in the end of the lists*/
ListPosition* listAppend(List* list, void* data);

/* Insert node with DATA and custom dataDestructor after the POS node*/
ListPosition* listInsertAfterWithDestructor(List* list, void* data, ListPosition* pos, void (*dataDestructor)(void*));
/* Insert node with DATA and custom dataDestructor in the end of the lists*/
ListPosition* listAppendWithDestructor(List* list, void* data, void (*dataDestructor)(void*));

/* Returns NULL if deletion is successful
   Otherwise return given POS
   dataDestructor is applied to every data*/
void listRemoveNode(List* list, ListPosition** pos);

void listPrint(List* list, void (*printData)(void*));

#endif