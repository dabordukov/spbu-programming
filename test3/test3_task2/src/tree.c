#include <stdbool.h>
#include <stdlib.h>

typedef struct TreeNode {
    struct TreeNode* left;
    struct TreeNode* right;
    int value;
} TreeNode;

TreeNode* treeNodeCreate(int value) {
    TreeNode* node = calloc(1, sizeof(TreeNode));
    if (node != NULL) {
        node->value = value;
    }

    return node;
}

static TreeNode* treeInsertInternals(TreeNode* root, int value, bool (*compare)(int, int), int* error) {
    if (root == NULL) {
        TreeNode* new = treeNodeCreate(value);
        if (new == NULL) {
            *error = 1;
        }
        return new;
    }

    if (root != NULL) {
        if (compare(value, root->value)) {
            root->left = treeInsertInternals(root->left, value, compare, error);
        } else {
            root->right = treeInsertInternals(root->right, value, compare, error);
        }
    }

    return root;
}

int treeInsert(TreeNode** root, int value, bool (*compare)(int, int)) {
    int error = 0;
    *root = treeInsertInternals(*root, value, compare, &error);
    return error;
}

void treeFree(TreeNode* root) {
    if (root == NULL) {
        return;
    }
    treeFree(root->left);
    treeFree(root->right);
    free(root);
}

static void treeTraverseInorder(TreeNode* root, int* array, size_t* index) {
    if (root == NULL) {
        return;
    }

    treeTraverseInorder(root->left, array, index);
    array[*index] = root->value;
    (*index)++;
    treeTraverseInorder(root->right, array, index);
}

static TreeNode* intArrayToSortedTree(int* array, size_t size, bool (*compare)(int, int)) {
    TreeNode* root = treeNodeCreate(array[0]);
    for (size_t i = 1; i < size; i++) {
        treeInsert(&root, array[i], compare);
    }

    return root;
}

void treeSort(int* array, size_t size, bool (*compare)(int, int)) {
    if (size < 2) {
        return;
    }

    TreeNode* root = intArrayToSortedTree(array, size, compare);
    size_t index = 0;
    treeTraverseInorder(root, array, &index);

    treeFree(root);
}
