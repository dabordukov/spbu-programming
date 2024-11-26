#ifndef DICTIONARY_H
#define DICTIONARY_H

#include <stdbool.h>

typedef void Dictionary;

int dictionaryInsert(Dictionary** root, char* key, char* string);
void dictionaryEntryRemove(Dictionary** root, char* key);
Dictionary* dictionaryCreate(char* key, char* string);
void dictionaryFree(Dictionary** root);
char* dictionaryGet(Dictionary* root, char* key);
bool dictionaryHas(Dictionary* root, char* key);

#endif