#include "tokens.h"

#include <stdbool.h>
TokenType charToTokenType(int c) {
    switch (c) {
        case '+':
            return TOKEN_PLUS;
        case '-':
            return TOKEN_MINUS;
        case '*':
            return TOKEN_MUL;
        case '/':
            return TOKEN_DIV;
        default:
            if ('0' <= c && c <= '9') {
                return TOKEN_INTEGER;
            }
            return TOKEN_UNDEFINED;
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
            break;
        case '-':
            token.type = TOKEN_MINUS;
            break;
        case '*':
            token.type = TOKEN_MUL;
            break;
        case '/':
            token.type = TOKEN_DIV;
            break;
        case '(':
            token.type = TOKEN_LEFT_PARENTHESIS;
            break;
        case ')':
            token.type = TOKEN_RIGHT_PARENTHESIS;
            break;
        default:
            if ('0' <= c && c <= '9') {
                token.type = TOKEN_INTEGER;
                token.value = c - '0';
                break;
            }
            token.type = TOKEN_UNDEFINED;
    }
    return token;
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