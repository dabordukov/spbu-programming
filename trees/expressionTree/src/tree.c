#include <assert.h>
#include <stdbool.h>
#include <stdio.h>
#include <stdlib.h>

#include "ioAuxiliaries.h"
#include "stack.h"

typedef enum TokenType {
    TREE,
    NUMBER,
    END
} Token;

typedef struct TreeNode {
    struct TreeNode* left;
    struct TreeNode* right;
    int value;
} TreeNode;

TreeNode* treeNodeCreate(char value) {
    TreeNode* node = calloc(1, sizeof(TreeNode));
    node->value = value;
    return node;
}

static Token checkToken(FILE* expression) {
    int ch = getc(expression);
    ungetc(ch, expression);

    if (ch == '(') {
        return TREE;
    } else if (ch == ')') {
        return END;
    } else {
        return NUMBER;
    }
}

TreeNode* treeBuild(FILE* expression) {
    clearWhitespaces(expression);
    Token token = checkToken(expression);
    TreeNode* node;
    if (token == TREE) {
        assert(getc(expression) == '(');
        clearWhitespaces(expression);
        int operation = getc(expression);
        node = treeNodeCreate(operation);
        node->left = treeBuild(expression);
        node->right = treeBuild(expression);
        clearWhitespaces(expression);
        assert(getc(expression) == ')');
    } else if (token == NUMBER) {
        int number = 0;
        fscanf(expression, "%d", &number);
        clearWhitespaces(expression);
        node = treeNodeCreate(number);
    }

    return node;
}

static Token nodeToToken(TreeNode* node) {
    if (node->left == NULL && node->right == NULL) {
        return NUMBER;
    } else {
        return TREE;
    }
}

int evaluateTree(TreeNode* node) {
    if (nodeToToken(node) == TREE) {
        switch (node->value) {
            case '+':
                return evaluateTree(node->left) + evaluateTree(node->right);
            case '-':
                return evaluateTree(node->left) - evaluateTree(node->right);
            case '*':
                return evaluateTree(node->left) * evaluateTree(node->right);
            case '/':
                return evaluateTree(node->left) / evaluateTree(node->right);
        }
    }

    return node->value;
}

static void printTreeInternals(TreeNode* node) {
    if (node->left != NULL && node->right != NULL) {
        printf("(%c ", node->value);
        printTreeInternals(node->left);
        printf(" ");
        printTreeInternals(node->right);
        printf(")");
    } else {
        assert(node->left == node->right);
        printf("%d", node->value);
    }
}

void printTree(TreeNode* node) {
    printTreeInternals(node);
    printf("\n");
}

void treeNodeFree(TreeNode* node) {
    if (node == NULL) {
        return;
    }
    treeNodeFree(node->left);
    treeNodeFree(node->right);
    free(node);
}