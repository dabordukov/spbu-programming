#include <assert.h>
#include <stdbool.h>
#include <stdio.h>

#include "stack.h"
#include "tokens.h"

bool testStackPush() {
    bool passed = true;
    Stack* stack = stackInit();
    Token testToken = (Token){TOKEN_INTEGER, 42};
    stackPush(stack, testToken);
    int error = 0;

    if (!isEqualToken(stackPop(stack, &error), testToken) || error != 0) {
        passed = false;
    }
    testToken = (Token){TOKEN_PLUS, '+'};
    stackPush(stack, testToken);
    if (!isEqualToken(stackTop(stack, &error), testToken) || error != 0) {
        passed = false;
    }
    stackFree(&stack);
    return passed;
}

bool testStackPop() {
    bool passed = true;
    Stack* stack = stackInit();
    Token testToken = (Token){TOKEN_INTEGER, 42};
    Token testToken2 = (Token){TOKEN_PLUS, '+'};

    stackPush(stack, testToken);
    stackPush(stack, testToken2);

    int error = 0;
    if (!isEqualToken(stackPop(stack, &error), testToken2) || error != 0) {
        passed = false;
    }

    if (!isEqualToken(stackPop(stack, &error), testToken) || error != 0) {
        passed = false;
    }

    if (passed && (!isEqualToken(stackPop(stack, &error), NULL_TOKEN) || error != 1)) {
        passed = false;
    }

    stackFree(&stack);
    return passed;
}

bool testStackIsEmpty() {
    bool passed = true;
    Stack* stack = stackInit();

    if (stackIsEmpty(stack) == false) {
        passed = false;
    }
    Token testToken = (Token){TOKEN_INTEGER, 42};

    stackPush(stack, testToken);
    if (stackIsEmpty(stack) == true) {
        passed = false;
    }
    int error;
    stackPop(stack, &error);
    if (stackIsEmpty(stack) == false) {
        passed = false;
    }

    stackFree(&stack);
    return passed;
}

int main() {
    assert(testStackPush());
    assert(testStackPop());
    assert(testStackIsEmpty());
}
