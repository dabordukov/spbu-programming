#ifndef STACK_ON_POINTERS
#define STACK_ON_POINTERS
#include <stdbool.h>

#include "../postfixCalculatorTokens.h"

typedef struct Stack* Stack;

/* sets ERROR to 1 on empty stack
   else return popped value */
Token stackPop(Stack* stack, int* error);

int stackPush(Stack* stack, Token token);
bool stackIsEmpty(Stack* stack);

/* sets ERROR to 1 on empty stack
   else return top value */
Token stackTop(Stack* stack, int* error);
int stackFree(Stack** stackPtr);
Stack* stackInit();
#endif