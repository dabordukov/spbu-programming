#include <assert.h>
#include <stdbool.h>
#include <stdio.h>

#include "processFile.h"

bool compareFiles(char* filename1, char* filename2) {
    FILE* file1 = fopen(filename1, "r");
    if (file1 == NULL) {
        return false;
    }

    FILE* file2 = fopen(filename2, "r");
    if (file2 == NULL) {
        fclose(file1);
        return false;
    }

    int ch = 0;
    bool equal = false;
    while ((ch = getc(file1)) == getc(file2)) {
        if (ch == EOF) {
            equal = true;
            break;
        }
    }

    fclose(file1);
    fclose(file2);

    return equal;
}

int main() {
    processFile(TEST_IN, "out.tmp", 5, 5);
    assert(compareFiles(TEST_OUT, "out.tmp") == true);
}