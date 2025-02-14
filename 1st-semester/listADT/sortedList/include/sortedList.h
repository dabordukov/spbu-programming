#pragma once
#include "list.h"

ListPosition* listInsertAfterSorted(List* list, void* data, bool (*comparator)(void*, void*));
bool comparatorInsertDescendingInt(void* dataList, void* dataInsert);
bool comparatorInsertAscendingInt(void* dataList, void* dataInsert);
bool compareInt(void* dataL, void* dataR);
void printListInt(void* data);
void nothing(void*);