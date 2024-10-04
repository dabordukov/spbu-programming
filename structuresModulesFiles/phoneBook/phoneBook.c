#include "phoneBook.h"

#include "auxiliaries.h"

struct phoneNumber {
    char* data;
    int size;
};

struct name {
    char* data;
    int size;
};

typedef struct phoneBookNode {
    struct phoneNumber number;
    struct name name;
} phoneBookNode;

struct phoneBook {
    phoneBookNode* entries[PHONEBOOK_MAX_ENTRIES];
    int size;
};

void* phoneBookInit() {
    phoneBook book = checkedMalloc(sizeof(struct phoneBook));
    book->size = 0;
    return book;
}
