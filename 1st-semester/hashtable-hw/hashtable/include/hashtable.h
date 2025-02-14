#ifndef HASHTABLE_H
#define HASHTABLE_H

#include <stdlib.h>

typedef struct Hashtable Hashtable;

/* Initialize hashtable */
Hashtable* hashtableInit();

/* Add new pair {key, value} or update key with new value */
int hashtableUpdate(Hashtable* ht, const char* string, size_t value);

/*Returns value by key
  Assign error = 1 on non-existing key and return 0 */
size_t hashtableGet(Hashtable* ht, const char* key, int* error);

/* Frees every key and the hashtable */
void hashtableFree(Hashtable** pHashtable);

/* Return fill factor of the hashtable (size/capacity) */
float fillFactor(Hashtable* ht);

/* Return max among bucket lengths */
size_t maxBucketLength(Hashtable* ht);

/* Return mean bucket length */
size_t meanBucketLength(Hashtable* ht);

/* Return array of pointers to keys */
char const** hashtableKeys(Hashtable* ht);

/* Return number of entries in the hashtable */
size_t hashtableSize(Hashtable* ht);

/* Return hashtable with words as keys and its frequencies as values*/
Hashtable* countWordFrequencies(FILE* stream);

#endif