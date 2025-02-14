
#include <stdbool.h>
#include <stdio.h>
#include <stdlib.h>

struct StackElement {
    int value;
    struct StackElement* next;
};

typedef struct Stack {
    size_t size;
    struct StackElement* top;
} Stack;

Stack* stackInit() {
    Stack* stack = calloc(1, sizeof(Stack));
    if (stack == NULL) {
        return NULL;
    }

    stack->size = 0;
    stack->top = NULL;
    return stack;
}

int stackPush(Stack* stack, int value) {
    struct StackElement* element = calloc(1, sizeof(struct StackElement));
    if (element == NULL) {
        return 1;
    }

    element->next = stack->top;
    element->value = value;
    stack->top = element;
    stack->size++;
    return 0;
}

int stackPop(Stack* stack, int* error) {
    *error = 0;
    if (stack->size == 0) {
        *error = 1;
        return 0;
    }

    int popValue = stack->top->value;
    struct StackElement* oldElement = stack->top;
    stack->top = oldElement->next;
    stack->size--;
    free(oldElement);

    return popValue;
}

int stackTop(Stack* stack, int* error) {
    *error = 0;
    if (stack->size == 0) {
        *error = 1;
        return 0;
    }

    return stack->top->value;
}

bool stackIsEmpty(Stack* stack) {
    return stack->size == 0;
}

void stackFree(Stack** stackPtr) {
    if (stackPtr == NULL || *stackPtr == NULL) {
        return;
    }
    Stack* stack = *stackPtr;

    int error;
    int size = stack->size;
    while (size--) {
        stackPop(stack, &error);
    }

    free(stack);
    *stackPtr = NULL;
}