#include <stdio.h>

#include "ioAuxiliaries.h"
#include "tree.h"

int main(int argc, char* argv[]) {
    FILE* inputFile;
    if (argc == 2) {
        inputFile = fopen(argv[1], "r");
        if (inputFile == NULL) {
            fprintf(stderr, "Can't open %s for read\n", argv[1]);
            return 1;
        }
    } else {
        printf("Введите имя файла, содержащего дерево: ");
        char* filename = NULL;
        int error = 0;
        fReadLine(stdin, &filename, &error);
        if (error != 0) {
            fprintf(stderr, "Can't read filename\n");
            return 2;
        }

        inputFile = fopen(filename, "r");
        if (inputFile == NULL) {
            fprintf(stderr, "Can't open %s for read\n", argv[1]);
            return 1;
        }
    }

    TreeNode* expr = treeBuild(inputFile);
    printTree(expr);
    printf("Результат: %d\n", evaluateTree(expr));

    fclose(inputFile);
    treeNodeFree(expr);
}