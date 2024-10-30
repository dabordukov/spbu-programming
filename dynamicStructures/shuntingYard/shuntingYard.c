#include <assert.h>

#include "queue/queue.h"
#include "stackOnPointers/stack.h"
#include "tokens.h"

static Queue* charStringToTokenQueue(char* string) {
    Queue* queue = queueInit();

    for (int i = 0; string[i] != 0; i++) {
        Token token = charToToken(string[i]);
        if (token.type == TOKEN_UNDEFINED) {
            continue;
        }
        if (queuePush(queue, token) != 0) {
            queueFree(&queue);
            return NULL;
        }
    }

    return queue;
}

static char* tokenQueueToCharString(Queue* queue) {
    char* expression = calloc(queueSize(queue) * 2, sizeof(char));
    if (expression == NULL) {
        return NULL;
    }

    int i = 0;
    for (; !queueIsEmpty(queue); i++) {
        Token token;
        queuePop(queue, &token);
        expression[i] = tokenToChar(token);
        expression[++i] = ' ';
    }

    expression[--i] = 0;
    return expression;
}

int shuntingYardAlgorithm(char* expressionInfix, char** expressionPostfix) {
    Queue* tokensInput = charStringToTokenQueue(expressionInfix);
    if (tokensInput == NULL) {
        return 1;
    }
    Stack* stack = stackInit();
    Queue* tokensOutput = queueInit();
    if (tokensOutput == NULL) {
        return 1;
    }

    bool error = false;
    while (!queueIsEmpty(tokensInput)) {
        Token token;
        queuePop(tokensInput, &token);
        int err = 0;
        switch (token.type) {
            case TOKEN_INTEGER:
                if (queuePush(tokensOutput, token) != 0) {
                    error = true;
                }
                break;
            case TOKEN_PLUS:
            case TOKEN_MINUS:
            case TOKEN_MUL:
            case TOKEN_DIV:
                assert(token.priority > 0);
                while (stackTop(stack, &err).priority >= token.priority) {  // in case of stack is empty it returns NULL_TOKEN, which has priority=0
                    queuePush(tokensOutput, stackPop(stack, &err));
                    assert(err == 0);  ///////////////////////
                }
                stackPush(stack, token);
                break;
            case TOKEN_LEFT_PARENTHESIS:
                stackPush(stack, token);
                break;
            case TOKEN_RIGHT_PARENTHESIS:
                while (stackTop(stack, &err).type != TOKEN_LEFT_PARENTHESIS) {
                    assert(err == 0);           ///////////
                    if (stackIsEmpty(stack)) {  // invalid expression
                        error = true;
                        break;
                    }
                    queuePush(tokensOutput, stackPop(stack, &err));
                    assert(err == 0);  ///////////////
                }
                stackPop(stack, &err);
                assert(err == 0);  /////////////
                break;
            default:
                assert(false);  /////////
                error = true;
                break;
        }
        if (error) {
            break;
        }
    }

    if (!error) {
        while (!stackIsEmpty(stack)) {
            int err = 0;
            if (stackTop(stack, &err).type == TOKEN_LEFT_PARENTHESIS) {
                error = true;
                break;
            }
            queuePush(tokensOutput, stackPop(stack, &err));
            assert(err == 0);  //////////////
        }
    }

    if (!error) {
        *expressionPostfix = tokenQueueToCharString(tokensOutput);
    }

    queueFree(&tokensOutput);
    queueFree(&tokensInput);
    stackFree(&stack);

    return (int)error;
}