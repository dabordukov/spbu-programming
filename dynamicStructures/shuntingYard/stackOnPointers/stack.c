
#include <stdbool.h>
#include <stdio.h>
#include <stdlib.h>

#include "../memoryAuxiliaries.h"
#include "../tokens.h"
struct stackElement {
    Token data;
    struct stackElement* next;
};

typedef struct Stack {
    size_t size;
    struct stackElement* top;
} Stack;

Stack* stackInit() {
    Stack* stack = checkedCalloc(1, sizeof(Stack));
    stack->size = 0;
    stack->top = NULL;
    return stack;
}

int stackPush(Stack* stack, Token token) {
    struct stackElement* element = checkedCalloc(1, sizeof(struct stackElement));
    element->next = stack->top;
    element->data = token;
    stack->top = element;
    stack->size++;

    return 0;
}

Token stackPop(Stack* stack, int* error) {
    *error = 0;
    if (stack->size == 0) {
        *error = 1;
        return NULL_TOKEN;
    }

    Token popValue = stack->top->data;
    struct stackElement* oldElement = stack->top;
    stack->top = oldElement->next;
    stack->size--;
    free(oldElement);

    return popValue;
}

Token stackTop(Stack* stack, int* error) {
    *error = 0;
    if (stack->size == 0) {
        *error = 1;
        return NULL_TOKEN;
    }

    return stack->top->data;
}

bool stackIsEmpty(Stack* stack) {
    return stack->size == 0;
}

int stackFree(Stack** stackPtr) {
    if (stackPtr == NULL || *stackPtr == NULL) {
        return 0;
    }
    Stack* stack = *stackPtr;

    int error;
    int size = stack->size;
    while (size--) {
        stackPop(stack, &error);
    }

    free(stack);
    *stackPtr = NULL;
    return 0;
}