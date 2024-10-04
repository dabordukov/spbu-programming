#include "phoneBook.h"

#include "auxiliaries.h"
#include "my_string.h"

typedef struct phoneBookNode {
    string number;
    string name;
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

int phoneBookAddEntry(phoneBook book, string name, string number) {
    return 0;
}