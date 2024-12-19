#ifndef STACK_ON_POINTERS
#define STACK_ON_POINTERS
#include <stdbool.h>

typedef struct Stack Stack;

/* sets ERROR to 1 on empty stack
   else return popped value */
int stackPop(Stack* stack, int* error);

int stackPush(Stack* stack, int value);
bool stackIsEmpty(Stack* stack);

/* sets ERROR to 1 on empty stack
   else return top value */
int stackTop(Stack* stack, int* error);
int stackFree(Stack** stackPtr);
Stack* stackInit();
#endif