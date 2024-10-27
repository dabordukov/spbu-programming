#include <assert.h>
#include <stdbool.h>

#include "../stack.h"
bool testStackPush() {
    bool passed = true;
    Stack* stack = stackInit();
    stackPush(stack, 42);
    int error = 0;

    if (stackTop(stack, &error) != 42 || error != 0) {
        passed = false;
    }
    stackPush(stack, 888);
    if (stackTop(stack, &error) != 888 || error != 0) {
        passed = false;
    }
    stackFree(&stack);
    return passed;
}

bool testStackPop() {
    bool passed = true;
    Stack* stack = stackInit();
    stackPush(stack, 42);
    stackPush(stack, 888);

    int error = 0;
    if (stackPop(stack, &error) != 888 || error != 0) {
        passed = false;
    }
    if (stackPop(stack, &error) != 42 || error != 0) {
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

    stackPush(stack, 42);
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

bool testStackAll() {
    assert(testStackIsEmpty());
    assert(testStackPop());
    assert(testStackPush());
}