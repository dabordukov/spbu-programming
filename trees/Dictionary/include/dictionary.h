#ifndef DICTIONARY_H
#define DICTIONARY_H

#include <stdbool.h>

typedef struct dictionary Dictionary;

int dictionaryInsert(Dictionary** root, int key, char* string);
void dictionaryEntryRemove(Dictionary** root, int key);
Dictionary* dictionaryCreate(int key, char* string);
void dictionaryFree(Dictionary** root);
char* dictionaryGet(Dictionary* root, int key);
bool dictionaryHas(Dictionary* root, int key);

#endif