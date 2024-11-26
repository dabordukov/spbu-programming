#include <assert.h>
#include <stdlib.h>
#include <string.h>

#include "dictionary.h"

void assertDictValue(Dictionary* dict, int key, char* expected) {
    char* value = dictionaryGet(dict, key);
    if (value == NULL) {
        assert(expected == NULL);
        return;
    }
    assert(strcmp(value, expected) == 0);
}
int main() {
    // test creation and insertion
    Dictionary* testDictionary = dictionaryCreate(0, "first");
    assertDictValue(testDictionary, 0, "first");
    dictionaryInsert(&testDictionary, 1, "1");
    assert(dictionaryHas(testDictionary, 1));
    assertDictValue(testDictionary, 1, "1");
    dictionaryInsert(&testDictionary, 1, "01");
    assertDictValue(testDictionary, 1, "01");
    dictionaryInsert(&testDictionary, 2, "2");
    assert(dictionaryHas(testDictionary, 2));
    assertDictValue(testDictionary, 2, "2");
    dictionaryInsert(&testDictionary, -3, "-3");
    assert(dictionaryHas(testDictionary, -3));
    assertDictValue(testDictionary, -3, "-3");

    // test deletion
    dictionaryEntryRemove(&testDictionary, 0);
    assertDictValue(testDictionary, 0, NULL);
    dictionaryEntryRemove(&testDictionary, 2);
    assertDictValue(testDictionary, 2, NULL);
    // check that dictionary is not broken
    assertDictValue(testDictionary, -3, "-3");
    assertDictValue(testDictionary, 1, "01");

    dictionaryFree(&testDictionary);
}