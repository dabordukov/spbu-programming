#include <stdbool.h>
#include <stdlib.h>
#include <string.h>

typedef struct TreeNode {
    struct TreeNode* left;
    struct TreeNode* right;
    char* string;
    int key;
} TreeNode;

TreeNode* treeNodeCreate(int key, char* string) {
    TreeNode* node = calloc(1, sizeof(TreeNode));
    if (node != NULL) {
        node->string = malloc((strlen(string) + 1) * sizeof(char));
        if (node->string == NULL) {
            free(node);
            return NULL;
        }
        node->key = key;

        strcpy(node->string, string);
    }

    return node;
}

TreeNode* dictionaryCreate(int key, char* string) {
    return treeNodeCreate(key, string);
}

static void treeNodeFree(TreeNode* node) {
    free(node->string);
    free(node);
}

bool inDictionary(TreeNode* root, int key) {
    TreeNode* iter = root;
    while (iter != NULL && iter->key != key) {
        if (key < iter->key) {
            iter = iter->left;
        } else {
            iter = iter->right;
        }
    }

    if (iter == NULL) {
        return false;
    }
    return true;
}

static TreeNode* dictionaryInsertInternals(TreeNode* root, int key, char* string, int* error) {
    if (root == NULL) {
        TreeNode* new = treeNodeCreate(key, string);
        if (new == NULL) {
            *error = 1;
        }
        return new;
    }

    if (root != NULL) {
        if (key < root->key) {
            root->left = dictionaryInsertInternals(root->left, key, string, error);
        } else if (key > root->key) {
            root->right = dictionaryInsertInternals(root->right, key, string, error);
        } else {
            free(root->string);
            root->string = malloc((strlen(string) + 1) * sizeof(char));
            strcpy(root->string, string);
        }
    }

    return root;
}

int dictionaryInsert(TreeNode** root, int key, char* string) {
    int error = 0;
    *root = dictionaryInsertInternals(*root, key, string, &error);
    return error;
}

static TreeNode* treePullLeftMost(TreeNode* root) {
    if (root->left == NULL) {
        return root;
    }

    TreeNode* node = treePullLeftMost(root->left);
    if (root->left == node) {
        root->left = node->right;
        node->right = NULL;
    }

    return node;
}

TreeNode* dictionaryEntryRemoveInternals(TreeNode* root, int key) {
    if (root == NULL) {
        return NULL;
    }
    if (key == root->key) {
        TreeNode* old = root;
        TreeNode* new = NULL;
        if (root->right != NULL) {
            new = treePullLeftMost(root->right);
            if (new != root->right) {
                new->right = root->right;
            }
            new->left = root->left;
        } else {
            new = root->left;
        }

        treeNodeFree(old);
        return new;
    }

    if (key < root->key) {
        root->left = dictionaryEntryRemoveInternals(root->left, key);
    } else {
        root->right = dictionaryEntryRemoveInternals(root->right, key);
    }
    return root;
}

void dictionaryEntryRemove(TreeNode** root, int key) {
    *root = dictionaryEntryRemoveInternals(*root, key);
}

static TreeNode* dictionaryFindEntry(TreeNode* root, int key) {
    TreeNode* iter = root;
    while (iter != NULL && iter->key != key) {
        if (key < iter->key) {
            iter = iter->left;
        } else {
            iter = iter->right;
        }
    }
    return iter;
}

bool dictionaryHas(TreeNode* root, int key) {
    return dictionaryFindEntry(root, key) != NULL;
}

char* dictionaryGet(TreeNode* root, int key) {
    TreeNode* iter = dictionaryFindEntry(root, key);
    if (iter == NULL) {
        return NULL;
    }
    return iter->string;
}

void treeFree(TreeNode* root) {
    if (root == NULL) {
        return;
    }
    treeFree(root->left);
    treeFree(root->right);
    treeNodeFree(root);
}

void dictionaryFree(TreeNode** root) {
    treeFree(*root);
    *root = NULL;
}