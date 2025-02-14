#include <stdbool.h>
#include <stdlib.h>

#include "postfixCalculatorTokens.h"
#include "stackOnPointers/stack.h"

static Stack* tokenStringToStack(char* string) {
    Stack* stack = stackInit();

    for (int i = 0; string[i] != 0; i++) {
        if (charToTokenType(string[i]) == TOKEN_UNDEFINED) {
            continue;
        }
        stackPush(stack, charToToken(string[i]));
    }

    return stack;
}

Token evaluateExpressionStack(Stack* stack, int* error) {
    Token topElement = stackPop(stack, error);
    if (*error != 0) {
        return NULL_TOKEN;
    }
    if (topElement.type == TOKEN_INTEGER) {
        return topElement;
    }

    Token operation = topElement;
    if (!isTokenOperation(operation)) {
        *error = 1;
        return NULL_TOKEN;
    }

    Token operandRight = evaluateExpressionStack(stack, error);
    if (*error != 0) {
        return NULL_TOKEN;
    }
    Token operandLeft = evaluateExpressionStack(stack, error);
    if (*error != 0) {
        return NULL_TOKEN;
    }

    Token result = NULL_TOKEN;
    switch (operation.type) {
        case TOKEN_PLUS:
            result = tokenPlusToken(operandLeft, operandRight);
            return result;
        case TOKEN_MINUS:
            result = tokenMinusToken(operandLeft, operandRight);
            return result;
        case TOKEN_MUL:
            result = tokenMulToken(operandLeft, operandRight);
            return result;
        case TOKEN_DIV:
            result = tokenDivToken(operandLeft, operandRight);
            return result;
        default:
            *error = 1;
            return NULL_TOKEN;
    }
}

int calculate(char* expression, int* error) {
    *error = 0;

    Stack* stack = tokenStringToStack(expression);

    int result = evaluateExpressionStack(stack, error).value;

    stackFree(&stack);
    return result;
}