#ifndef EXPRESSION_OKENS_H
#define EXPRESSION_TOKENS_H
#include <stdbool.h>

#define NULL_TOKEN ((Token){TOKEN_UNDEFINED, 0})

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
} Token;

TokenType charToTokenType(int c);
Token charToToken(char c);
bool isTokenOperation(Token token);
bool isEqualToken(Token a, Token b);
Token tokenMinusToken(Token a, Token b);
Token tokenPlusToken(Token a, Token b);
Token tokenDivToken(Token a, Token b);
Token tokenMulToken(Token a, Token b);
#endif