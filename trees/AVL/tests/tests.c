#include <assert.h>
#include <stdlib.h>
#include <string.h>

#include "dictionary.h"

typedef struct TreeNode {
    struct TreeNode* left;
    struct TreeNode* right;
    char* data;
    char* key;
    int balance;
} TreeNode;

void assertDictValue(Dictionary* dict, char* key, char* expected) {
    char* value = dictionaryGet(dict, key);
    if (value == NULL) {
        assert(expected == NULL);
        return;
    }
    assert(strcmp(value, expected) == 0);
}

int assertHeight(TreeNode* node) {
    if (node == NULL) {
        return 1;
    }
    int leftHeight = assertHeight(node->left);
    int rightHeight = assertHeight(node->right);
    assert(abs(rightHeight - leftHeight) < 2);
    return (leftHeight > rightHeight) ? leftHeight : rightHeight;
}

void testCaseLeftLeft() {
    Dictionary* testDict = dictionaryCreate("Q", "");
    dictionaryInsert(&testDict, "P", "");
    dictionaryInsert(&testDict, "D", "");
    assertHeight((TreeNode*)testDict);
    dictionaryInsert(&testDict, "L", "");
    dictionaryInsert(&testDict, "C", "");
    dictionaryInsert(&testDict, "B", "");
    assertHeight((TreeNode*)testDict);
    dictionaryFree(&testDict);
}

void testCaseRightRight() {
    Dictionary* testDict = dictionaryCreate("A", "");
    dictionaryInsert(&testDict, "B", "");
    dictionaryInsert(&testDict, "D", "");
    assertHeight((TreeNode*)testDict);
    dictionaryInsert(&testDict, "E", "");
    dictionaryInsert(&testDict, "C", "");
    dictionaryInsert(&testDict, "F", "");
    assertHeight((TreeNode*)testDict);
    dictionaryFree(&testDict);
}

void testCaseLeftRight() {
    Dictionary* testDict = dictionaryCreate("Q", "");
    dictionaryInsert(&testDict, "E", "");
    dictionaryInsert(&testDict, "K", "");
    assertHeight((TreeNode*)testDict);
    dictionaryInsert(&testDict, "C", "");
    dictionaryInsert(&testDict, "F", "");
    dictionaryInsert(&testDict, "G", "");
    assertHeight((TreeNode*)testDict);
    dictionaryFree(&testDict);
}

void testCaseRightLeft() {
    Dictionary* testDict = dictionaryCreate("A", "");
    dictionaryInsert(&testDict, "F", "");
    dictionaryInsert(&testDict, "B", "");
    assertHeight((TreeNode*)testDict);
    dictionaryInsert(&testDict, "G", "");
    dictionaryInsert(&testDict, "E", "");
    dictionaryInsert(&testDict, "D", "");
    assertHeight((TreeNode*)testDict);
    dictionaryFree(&testDict);
}

int main() {
    // test creation and insertion
    Dictionary* testDictionary = dictionaryCreate("0", "first");

    assertDictValue(testDictionary, "0", "first");
    dictionaryInsert(&testDictionary, "1", "1");
    assert(dictionaryHas(testDictionary, "1"));
    assertDictValue(testDictionary, "1", "1");
    dictionaryInsert(&testDictionary, "1", "01");
    assertDictValue(testDictionary, "1", "01");
    dictionaryInsert(&testDictionary, "2", "2");
    assert(dictionaryHas(testDictionary, "2"));
    assertDictValue(testDictionary, "2", "2");
    dictionaryInsert(&testDictionary, "-3", "-3");
    assert(dictionaryHas(testDictionary, "-3"));
    assertDictValue(testDictionary, "-3", "-3");

    // test deletion
    dictionaryEntryRemove(&testDictionary, "1");
    assertDictValue(testDictionary, "1", NULL);
    dictionaryEntryRemove(&testDictionary, "2");
    assertDictValue(testDictionary, "2", NULL);
    // check that dictionary is not broken
    assertDictValue(testDictionary, "-3", "-3");
    assertDictValue(testDictionary, "0", "first");

    dictionaryFree(&testDictionary);

    testCaseLeftLeft();
    testCaseRightRight();
    testCaseLeftRight();
    testCaseRightLeft();
}