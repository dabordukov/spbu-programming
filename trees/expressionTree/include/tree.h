#ifndef TREE_H
#define TREE_H

typedef struct TreeNode TreeNode;

TreeNode* treeNodeCreate(int value);
void treeNodeFree(TreeNode* node);

/*Loads expression tree from STREAM*/
TreeNode* treeBuild(FILE* stream);
void printTree(TreeNode* node);
int evaluateTree(TreeNode* node);
#endif