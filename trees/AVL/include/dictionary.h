#ifndef DICTIONARY_H
#define DICTIONARY_H

#include <stdbool.h>

typedef struct Dictionary Dictionary;

/* Insert new key:string pair to the dictionary */
int dictionaryInsert(Dictionary** root, char* key, char* string);

/* Remove the key:string pair from the dictionary */
void dictionaryEntryRemove(Dictionary** root, char* key);

/* Initialize dictionary with key:string pair */
Dictionary* dictionaryCreate(char* key, char* string);

/* Free dictionary */
void dictionaryFree(Dictionary** root);

/* Get string by key from the dictionary */
char* dictionaryGet(Dictionary* root, char* key);

/* Check key:string pair existence in the dictionary */
bool dictionaryHas(Dictionary* root, char* key);

#endif