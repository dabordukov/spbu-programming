#ifndef TOKENS_H
#define TOKENS_H
#include <stdbool.h>

#define NULL_TOKEN ((Token){.type = TOKEN_UNDEFINED, .value = 0, .priority = 0})

typedef enum {
    TOKEN_PLUS,
    TOKEN_MINUS,
    TOKEN_MUL,
    TOKEN_DIV,
    TOKEN_INTEGER,
    TOKEN_LEFT_PARENTHESIS,
    TOKEN_RIGHT_PARENTHESIS,
    TOKEN_UNDEFINED
} TokenType;

typedef struct {
    TokenType type;
    int value;
    int priority;
} Token;

// TokenType charToTokenType(int c);
Token charToToken(char c);
char tokenToChar(Token token);
bool isTokenOperation(Token token);
bool isEqualToken(Token a, Token b);
Token tokenMinusToken(Token a, Token b);
Token tokenPlusToken(Token a, Token b);
Token tokenDivToken(Token a, Token b);
Token tokenMulToken(Token a, Token b);
int charOperationTokenPriority(char c);
#endif