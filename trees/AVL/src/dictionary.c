#include <assert.h>
#include <stdbool.h>
#include <stdlib.h>
#include <string.h>

typedef struct TreeNode {
    struct TreeNode* left;
    struct TreeNode* right;
    char* data;
    char* key;
    int balance;
} TreeNode;

static size_t treeHeight(TreeNode* root) {
    TreeNode* iter = root;
    size_t height = 0;

    while (iter != NULL) {
        height++;
        if (root->balance < 0) {
            iter = iter->left;
        } else {
            iter = iter->right;
        }
    }
    return height;
}

static void updateBalance(TreeNode* root) {
    if (root != NULL) {
        root->balance = treeHeight(root->right) - treeHeight(root->left);
    }
}

TreeNode* treeNodeCreate(char* key, char* data) {
    TreeNode* node = calloc(1, sizeof(TreeNode));
    if (node != NULL) {
        node->data = malloc((strlen(data) + 1) * sizeof(char));
        if (node->data == NULL) {
            free(node);
            return NULL;
        }
        strcpy(node->data, data);

        node->key = malloc((strlen(data) + 1) * sizeof(char));
        if (node->key == NULL) {
            free(node);
            return NULL;
        }
        strcpy(node->key, key);
    }

    return node;
}

TreeNode* dictionaryCreate(char* key, char* data) {
    return treeNodeCreate(key, data);
}

static void treeNodeFree(TreeNode* node) {
    free(node->data);
    free(node->key);
    free(node);
}

typedef struct {
    TreeNode* node;
    bool imbalanceIncreased;
} NodeBalance;

static TreeNode* rightRotate(TreeNode* root) {
    TreeNode* newRoot = root->left;
    root->left = newRoot->right;
    newRoot->right = root;
    updateBalance(newRoot->left);
    updateBalance(newRoot->right);
    updateBalance(newRoot);

    return newRoot;
}

static TreeNode* leftRotate(TreeNode* root) {
    TreeNode* newRoot = root->right;
    root->right = newRoot->left;
    newRoot->left = root;
    updateBalance(newRoot->left);
    updateBalance(newRoot->right);
    updateBalance(newRoot);

    return newRoot;
}

TreeNode* balanceTree(TreeNode* root) {
    if (root == NULL) {
        return NULL;
    }
    TreeNode* newRoot = root;
    if (root->balance < -1) {
        if (root->left->balance < 0) {
            newRoot = rightRotate(root);
        } else if (root->left->balance > 0) {
            root->left = leftRotate(root->left);
            newRoot = rightRotate(root);
        }
    } else if (root->balance > 1) {
        if (root->right->balance > 0) {
            newRoot = leftRotate(root);
        } else if (root->right->balance < 0) {
            root->right = rightRotate(root->right);
            newRoot = leftRotate(root);
        }
    }

    return newRoot;
}

static NodeBalance dictionaryInsertInternals(TreeNode* root, char* key, char* data, int* error) {
    NodeBalance node = {.imbalanceIncreased = false, .node = NULL};
    if (root == NULL) {
        TreeNode* new = treeNodeCreate(key, data);
        if (new == NULL) {
            *error = 1;
        }
        node.node = new;
        node.imbalanceIncreased = true;
        return node;
    }

    if (strcmp(key, root->key) < 0) {
        node = dictionaryInsertInternals(root->left, key, data, error);
        if (node.imbalanceIncreased) {
            if (root->balance > 0) {
                node.imbalanceIncreased = false;
            }
            root->balance--;
        }
        root->left = node.node;
    } else if (strcmp(key, root->key) > 0) {
        node = dictionaryInsertInternals(root->right, key, data, error);
        if (node.imbalanceIncreased) {
            if (root->balance < 0) {
                node.imbalanceIncreased = false;
            }
            root->balance++;
        }
        root->right = node.node;
    } else {
        free(root->data);
        root->data = malloc((strlen(data) + 1) * sizeof(char));
        strcpy(root->data, data);
        return (NodeBalance){.imbalanceIncreased = false, .node = root};
    }

    root = balanceTree(root);

    node.node = root;
    return node;
}

int dictionaryInsert(TreeNode** root, char* key, char* data) {
    int error = 0;
    *root = dictionaryInsertInternals(*root, key, data, &error).node;
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

NodeBalance dictionaryEntryRemoveInternals(TreeNode* root, char* key) {
    NodeBalance node = {.node = NULL, .imbalanceIncreased = false};
    if (root == NULL) {
        return node;
    }

    if (strcmp(key, root->key) == 0) {
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
        updateBalance(new);
        new = balanceTree(new);
        node.imbalanceIncreased = true;
        node.node = new;
        return node;

    } else if (strcmp(key, root->key) < 0) {
        node = dictionaryEntryRemoveInternals(root->left, key);
        root->left = node.node;
        if (node.imbalanceIncreased) {
            if (root->balance > 0) {
                node.imbalanceIncreased = false;
            }
            root->balance++;
        }
    } else {
        node = dictionaryEntryRemoveInternals(root->right, key);
        root->right = node.node;
        if (node.imbalanceIncreased) {
            if (root->balance < 0) {
                node.imbalanceIncreased = false;
            }
            root->balance--;
        }
    }

    root = balanceTree(root);

    node.node = root;
    return node;
}

void dictionaryEntryRemove(TreeNode** root, char* key) {
    *root = dictionaryEntryRemoveInternals(*root, key).node;
}

static TreeNode* dictionaryFindEntry(TreeNode* root, char* key) {
    TreeNode* iter = root;
    while (iter != NULL && strcmp(iter->key, key) != 0) {
        if (strcmp(key, iter->key) < 0) {
            iter = iter->left;
        } else {
            iter = iter->right;
        }
    }
    return iter;
}

bool dictionaryHas(TreeNode* root, char* key) {
    return dictionaryFindEntry(root, key) != NULL;
}

char* dictionaryGet(TreeNode* root, char* key) {
    TreeNode* iter = dictionaryFindEntry(root, key);
    if (iter == NULL) {
        return NULL;
    }
    return iter->data;
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