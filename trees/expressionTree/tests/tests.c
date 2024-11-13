#include <assert.h>
#include <stdbool.h>
#include <stdio.h>

#include "tree.h"
static FILE* stringToStream(char* string) {
    FILE* tmp = tmpfile();
    fprintf(tmp, "%s", string);
    rewind(tmp);
    return tmp;
}

bool assert_expression(char* expr, int result) {
    FILE* f = stringToStream(expr);
    bool equal = evaluateTree(treeBuild(f)) == result;
    fclose(f);
    return equal;
}

int main() {
    assert(assert_expression("(+ (* (+ 44 1) 5) (* (+ 1 1) 2))", 229));
    assert(assert_expression("(/ (* (+ 44 1) 5) (+ (+ 2 1) 2))", 45));
    assert(assert_expression("(/ (* (+ 44 1) 5) (- (+ 2 1) 8))", -45));
    assert(assert_expression("(/ (* (+ -44 1) 5) (- (+ 2 1) 8))", 43));
}