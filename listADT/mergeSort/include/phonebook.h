#ifndef PHONEBOOK_H
#define PHONEBOOK_H
#include "list.h"

void phoneBookEntryDestructor(void* data);
List* readEntriesToList(const char* filename);
bool phoneBookEntryComparatorNumber(void* dataL, void* dataR);
bool phoneBookEntryComparatorName(void* dataL, void* dataR);
void phoneBookEntryPrint(void* entry);
void phoneBookEntryFPrint(FILE* stream, void* entry);
#endif