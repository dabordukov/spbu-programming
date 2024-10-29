#include <stdbool.h>
#include <string.h>

#include "stack.h"

char oppositeBracket(char bracket) {
    switch (bracket) {
        case '(':
            return ')';
        case ')':
            return '(';
        case '{':
            return '}';
        case '}':
            return '{';
        case '[':
            return ']';
        case ']':
            return '[';
        default:
            return 0;
    }
}

bool isOpeningBracket(char symbol) {
    return symbol == '(' || symbol == '{' || symbol == '[';
}

bool isBracketBalanced(char* string) {
    bool balanced = true;
    Stack* stack = stackInit();
    int errorStack = 0;
    size_t stringLen = strlen(string);

    for (int i = 0; i < stringLen; i++) {
        char opposite = oppositeBracket(string[i]);

        if (opposite != 0) {
            if (isOpeningBracket(string[i])) {
                stackPush(stack, string[i]);

            } else {
                if (stackPop(stack, &errorStack) != opposite) {
                    balanced = false;
                    break;
                }
            }
        }
    }

    if (balanced && !stackIsEmpty(stack)) {
        balanced = false;
    }

    stackFree(&stack);
    return balanced;
}