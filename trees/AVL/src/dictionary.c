#include "dictionary.h"

#include <assert.h>
#include <stdbool.h>
#include <stdlib.h>
#include <string.h>

typedef struct Dictionary {
    struct Dictionary* left;
    struct Dictionary* right;
    char* data;
    char* key;
    int balance;
} Dictionary;

static size_t treeHeight(Dictionary* root) {
    if (root == NULL)
        return 0;

    int lHeight = treeHeight(root->left);
    int rHeight = treeHeight(root->right);

    return (lHeight > rHeight ? lHeight : rHeight) + 1;
}

static void updateBalance(Dictionary* root) {
    if (root != NULL) {
        root->balance = treeHeight(root->right) - treeHeight(root->left);
    }
}

Dictionary* treeNodeCreate(char* key, char* data) {
    Dictionary* node = calloc(1, sizeof(Dictionary));
    if (node != NULL) {
        node->data = strdup(data);
        if (node->data == NULL) {
            free(node);
            return NULL;
        }

        node->key = strdup(key);
        if (node->key == NULL) {
            free(node);
            return NULL;
        }
    }

    return node;
}

Dictionary* dictionaryCreate(char* key, char* data) {
    return treeNodeCreate(key, data);
}

static void treeNodeFree(Dictionary* node) {
    free(node->data);
    free(node->key);
    free(node);
}

static Dictionary* rightRotate(Dictionary* root) {
    Dictionary* newRoot = root->left;
    root->left = newRoot->right;
    newRoot->right = root;
    updateBalance(newRoot);
    updateBalance(newRoot->left);
    updateBalance(newRoot->right);
    return newRoot;
}

static Dictionary* leftRotate(Dictionary* root) {
    Dictionary* newRoot = root->right;
    root->right = newRoot->left;
    newRoot->left = root;
    updateBalance(newRoot);
    updateBalance(newRoot->left);
    updateBalance(newRoot->right);
    return newRoot;
}

Dictionary* balanceTree(Dictionary* root) {
    if (root == NULL) {
        return NULL;
    }

    updateBalance(root);

    if (root->balance < -1) {  // left heavy
        if (root->left->balance > 0) {
            root->left = leftRotate(root->left);
        }
        return rightRotate(root);
    } else if (root->balance > 1) {  // right heavy
        if (root->right->balance < 0) {
            root->right = rightRotate(root->right);
        }
        return leftRotate(root);
    }

    return root;
}

Dictionary* dictionaryInsertInternals(Dictionary* root, char* key, char* data, int* error) {
    if (root == NULL) {
        Dictionary* new = treeNodeCreate(key, data);
        if (new == NULL) {
            *error = 1;
        }
        return new;
    }
    if (strcmp(key, root->key) == 0) {
        free(root->data);
        root->data = strdup(data);
        if (root->data == NULL) {
            *error = 1;
        }
        return root;
    }
    if (strcmp(key, root->key) < 0) {
        root->left = dictionaryInsertInternals(root->left, key, data, error);
    } else {
        root->right = dictionaryInsertInternals(root->right, key, data, error);
    }

    return balanceTree(root);
}

int dictionaryInsert(Dictionary** root, char* key, char* data) {
    int error = 0;
    *root = dictionaryInsertInternals(*root, key, data, &error);
    return error;
}

static Dictionary* treePullLeftMost(Dictionary* root) {
    if (root->left == NULL) {
        return root;
    }

    Dictionary* node = treePullLeftMost(root->left);
    if (root->left == node) {
        root->left = node->right;
        node->right = NULL;
    }

    return node;
}

Dictionary* dictionaryEntryRemoveInternals(Dictionary* root, char* key) {
    if (root == NULL) {
        return NULL;
    }
    if (strcmp(key, root->key) < 0) {
        root->left = dictionaryEntryRemoveInternals(root->left, key);
    } else if (strcmp(key, root->key) > 0) {
        root->right = dictionaryEntryRemoveInternals(root->right, key);
    } else {
        Dictionary* new = NULL;
        if (root->right == NULL) {
            new = root->left;
            treeNodeFree(root);
            return new;
        }
        if (root->left == NULL) {
            new = root->right;
            treeNodeFree(root);
            return new;
        }
        if (root->left != NULL && root->right != NULL) {
            if (root->right != NULL) {
                new = treePullLeftMost(root->right);
                if (new != root->right) {
                    new->right = root->right;
                }
                new->left = root->left;
            } else {
                new = root->left;
            }

            treeNodeFree(root);
            return balanceTree(new);
        }
    }

    return balanceTree(root);
}

void dictionaryEntryRemove(Dictionary** root, char* key) {
    *root = dictionaryEntryRemoveInternals(*root, key);
}

static Dictionary* dictionaryFindEntry(Dictionary* root, char* key) {
    Dictionary* iter = root;
    while (iter != NULL && strcmp(iter->key, key) != 0) {
        if (strcmp(key, iter->key) < 0) {
            iter = iter->left;
        } else {
            iter = iter->right;
        }
    }
    return iter;
}

bool dictionaryHas(Dictionary* root, char* key) {
    return dictionaryFindEntry(root, key) != NULL;
}

char* dictionaryGet(Dictionary* root, char* key) {
    Dictionary* iter = dictionaryFindEntry(root, key);
    if (iter == NULL) {
        return NULL;
    }
    return iter->data;
}

void treeFree(Dictionary* root) {
    if (root == NULL) {
        return;
    }
    treeFree(root->left);
    treeFree(root->right);
    treeNodeFree(root);
}

void dictionaryFree(Dictionary** root) {
    treeFree(*root);
    *root = NULL;
}