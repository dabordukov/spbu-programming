#include <list.h>
#include <stdlib.h>
#include <string.h>

#include "ioAuxiliaries.h"

#define HT_INIT_CAPACITY 16

struct htEntry {
    char* key;
    size_t value;
};

void htEntryDestructor(void* entry) {
    free(((struct htEntry*)entry)->key);
    free(entry);
}

typedef struct {
    size_t capacity;
    List** bucket;
    size_t size;
} Hashtable;

// FNV hash function
static size_t hash(const char* key) {
    size_t hash = 14695981039346656037UL;
    for (const char* p = key; *p; p++) {
        hash ^= (size_t)(unsigned char)(*p);
        hash *= 1099511628211UL;
    }
    return hash;
}

Hashtable* hashtableInit() {
    Hashtable* ht = calloc(1, sizeof(Hashtable));
    if (ht == NULL) {
        return NULL;
    }

    ht->capacity = HT_INIT_CAPACITY;
    ht->bucket = calloc(ht->capacity, sizeof(List*));
    if (ht->bucket == NULL) {
        free(ht);
        return NULL;
    }

    return ht;
}

bool findKeyComparator(void* entry, void* string) {
    return strcmp((char*)string, ((struct htEntry*)entry)->key) == 0;
}

int hashtableExtend(Hashtable* ht);

int hashtableUpdate(Hashtable* ht, const char* key, size_t value) {
    size_t index = hash(key) % ht->capacity;

    if (ht->bucket[index] == NULL) {
        ht->bucket[index] = listInit();
    }

    ListPosition* pos = listFindData(ht->bucket[index], (void*)key, &findKeyComparator);
    if (pos == NULL) {
        struct htEntry* data = malloc(sizeof(struct htEntry));
        if (data == NULL) {
            return 1;
        }

        data->key = malloc((strlen(key) + 1) * sizeof(char));
        if (data->key == NULL) {
            free(data);
            return 1;
        }

        strcpy(data->key, key);
        data->value = value;
        listAppendWithDestructor(ht->bucket[index], data, &htEntryDestructor);
        ht->size++;
    } else {
        ((struct htEntry*)(listPosGetData(pos)))->value = value;
    }

    if ((float)ht->size / (float)ht->capacity > 1) {
        hashtableExtend(ht);
    }

    return 0;
}

int hashtableExtend(Hashtable* ht) {
    Hashtable oldHT = *ht;
    ht->bucket = calloc(ht->capacity * 2, sizeof(List*));
    if (ht->bucket == NULL) {
        ht->bucket = oldHT.bucket;
        return 1;
    }

    // reset hashtable
    ht->capacity = ht->capacity * 2;
    ht->size = 0;

    // copy old entries
    for (size_t bucket = 0; bucket < oldHT.capacity; bucket++) {
        if (oldHT.bucket[bucket] == NULL) {
            continue;
        }

        ListPosition* iter = listFirst(oldHT.bucket[bucket]);
        while (iter != NULL) {
            struct htEntry* entry = (struct htEntry*)(listPosGetData(iter));
            hashtableUpdate(ht, entry->key, entry->value);
            iter = listNextNode(iter);
        }
    }

    // free old buckets
    for (size_t i = 0; i < oldHT.capacity; i++) {
        listFree(&(oldHT.bucket[i]));
    }
    free(oldHT.bucket);

    return 0;
}

size_t hashtableGet(Hashtable* ht, const char* key, int* error) {
    *error = 0;

    size_t index = hash(key) % ht->capacity;
    ListPosition* pos = listFindData(ht->bucket[index], (void*)key, &findKeyComparator);

    if (pos == NULL) {
        *error = 1;
        return 0;
    }

    return ((struct htEntry*)(listPosGetData(pos)))->value;
}

void hashtableFree(Hashtable** pHashtable) {
    Hashtable* ht = *pHashtable;
    for (size_t i = 0; i < ht->capacity; i++) {
        listFree(&(ht->bucket[i]));
    }

    free(ht->bucket);
    free(ht);

    *pHashtable = NULL;
}

size_t meanBucketLength(Hashtable* ht) {
    size_t meanLength = 0;
    size_t nonEmptyBuckets = 0;
    for (size_t i = 0; i < ht->capacity; i++) {
        if (ht->bucket[i] != NULL) {
            nonEmptyBuckets++;
            meanLength += listSize(ht->bucket[i]);
        }
    }

    return meanLength / nonEmptyBuckets;
}

size_t maxBucketLength(Hashtable* ht) {
    size_t maxLength = 0;
    for (size_t i = 0; i < ht->capacity; i++) {
        if (ht->bucket[i] != NULL && maxLength < listSize(ht->bucket[i])) {
            maxLength = listSize(ht->bucket[i]);
        }
    }

    return maxLength;
}

float fillFactor(Hashtable* ht) {
    return (float)ht->size / (float)ht->capacity;
}

char** hashtableKeys(Hashtable* ht) {
    char** keys = calloc(ht->size, sizeof(char*));
    if (keys == NULL) {
        return NULL;
    }

    size_t keyIndex = 0;
    for (size_t i = 0; i < ht->capacity; i++) {
        ListPosition* ptr = listFirst(ht->bucket[i]);
        while (ptr != NULL) {
            keys[keyIndex] = ((struct htEntry*)listPosGetData(ptr))->key;
            ptr = listNextNode(ptr);
            keyIndex++;
        }
    }

    return keys;
}

size_t hashtableSize(Hashtable* ht) {
    if (ht == NULL) {
        return 0;
    }

    return ht->size;
}

Hashtable* countWordFrequencies(FILE* stream) {
    Hashtable* ht = hashtableInit();

    int error = 0;
    char* line = NULL;
    size_t lineSize = 0;
    while ((lineSize = fReadLine(stream, &line, &error)) > 0 && error == 0) {
        char* word = malloc(lineSize * sizeof(char));
        char* currChar = line;

        while (*currChar != '\0') {
            size_t wordLength = 0;
            while (*currChar != ' ') {
                word[wordLength] = *currChar;
                wordLength++;
                if (*currChar == '\0') {
                    break;
                }
                currChar++;
            }
            word[wordLength] = '\0';
            int errorHT = 0;
            if (wordLength > 0) {
                hashtableUpdate(ht, word, hashtableGet(ht, word, &errorHT) + 1);
            }

            if (*currChar == '\0') {
                break;
            }
            currChar++;
        }
        free(word);
    }

    return ht;
}