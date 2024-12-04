#ifndef TREE_H
#define TREE_H

typedef struct TreeNode TreeNode;

/* Insert element in the tree depending on comparator */
int treeInsert(TreeNode** root, int value, bool (*compare)(int, int));

/* Sort the tree depending on comparator */
void treeSort(int* array, size_t size, bool (*compare)(int, int));

/* Create tree*/
TreeNode* treeNodeCreate(int value);
#endif