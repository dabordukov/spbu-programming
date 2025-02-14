#include "tokens.h"

#include <assert.h>
#include <stdbool.h>
TokenType charToTokenType(char c) {
    switch (c) {
        case '+':
            return TOKEN_PLUS;
        case '-':
            return TOKEN_MINUS;
        case '*':
            return TOKEN_MUL;
        case '/':
            return TOKEN_DIV;
        case '(':
            return TOKEN_LEFT_PARENTHESIS;
        case ')':
            return TOKEN_RIGHT_PARENTHESIS;
        default:
            if ('0' <= c && c <= '9') {
                return TOKEN_INTEGER;
            }
            return TOKEN_UNDEFINED;
    }
}

int charOperationTokenPriority(char c) {
    switch (c) {
        case '+':
        case '-':
            return 1;
        case '*':
        case '/':
            return 2;
        default:
            return 0;
    }
}

bool isTokenOperation(Token token) {
    return token.type < 4;
}

bool isEqualToken(Token a, Token b) {
    return a.value == b.value && a.type == b.type;
}

Token charToToken(char c) {
    Token token = NULL_TOKEN;
    switch (c) {
        case '+':
            token.type = TOKEN_PLUS;
            token.value = c;
            break;
        case '-':
            token.type = TOKEN_MINUS;
            token.value = c;
            break;
        case '*':
            token.type = TOKEN_MUL;
            token.value = c;
            break;
        case '/':
            token.type = TOKEN_DIV;
            token.value = c;
            break;
        case '(':
            token.type = TOKEN_LEFT_PARENTHESIS;
            token.value = c;
            break;
        case ')':
            token.type = TOKEN_RIGHT_PARENTHESIS;
            token.value = c;
            break;
        default:
            if ('0' <= c && c <= '9') {
                token.type = TOKEN_INTEGER;
                token.value = c - '0';
                break;
            }
            token.type = TOKEN_UNDEFINED;
    }
    token.priority = charOperationTokenPriority(c);

    return token;
}

char tokenToChar(Token token) {
    switch (token.type) {
        case TOKEN_INTEGER:
            assert(token.value < 10 && token.value >= 0);
            return token.value + '0';
        case TOKEN_PLUS:
        case TOKEN_MINUS:
        case TOKEN_MUL:
        case TOKEN_DIV:
        case TOKEN_LEFT_PARENTHESIS:
        case TOKEN_RIGHT_PARENTHESIS:
            return token.value;
        default:
            return 255;
    }
}

Token tokenPlusToken(Token a, Token b) {
    if (a.type == b.type && a.type == TOKEN_INTEGER) {
        a.value += b.value;
        return a;
    }
    return NULL_TOKEN;
}
Token tokenMulToken(Token a, Token b) {
    if (a.type == b.type && a.type == TOKEN_INTEGER) {
        a.value *= b.value;
        return a;
    }
    return NULL_TOKEN;
}
Token tokenDivToken(Token a, Token b) {
    if (a.type == b.type && a.type == TOKEN_INTEGER) {
        a.value /= b.value;
        return a;
    }
    return NULL_TOKEN;
}
Token tokenMinusToken(Token a, Token b) {
    if (a.type == b.type && a.type == TOKEN_INTEGER) {
        a.value -= b.value;
        return a;
    }
    return NULL_TOKEN;
}