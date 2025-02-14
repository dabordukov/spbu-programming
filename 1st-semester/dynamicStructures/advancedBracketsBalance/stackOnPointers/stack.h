#ifndef STACK_ON_POINTERS
#define STACK_ON_POINTERS
#include <stdbool.h>

typedef struct Stack Stack;

/* sets ERROR to 1 on empty stack
   else return popped value */
int stackPop(Stack* stack, int* error);

/* Return 0 on successfull pushing
   Return 1 on allocation error */
int stackPush(Stack* stack, int value);

/* Return true if stack is empy
   Otherwise, return false*/
bool stackIsEmpty(Stack* stack);

/* sets ERROR to 1 on empty stack
   else return top value */
int stackTop(Stack* stack, int* error);

/* Create stack */
Stack* stackInit();

/* Free stack */
int stackFree(Stack** stackPtr);

#endif