
#include <stdbool.h>
#include <stdio.h>
#include <stdlib.h>

static void* checkedCalloc(size_t nmemb, size_t size) {
    void* p = calloc(nmemb, size);
    if (!p) {
        fprintf(stderr, "Can't allocate %lu bytes of memory!\n", size);
        abort();
    }
    return p;
}

struct stackElement {
    int value;
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
}

int stackPush(Stack* stack, int value) {
    struct stackElement* element = checkedCalloc(1, sizeof(struct stackElement));
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
    struct stackElement* oldElement = stack->top;
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