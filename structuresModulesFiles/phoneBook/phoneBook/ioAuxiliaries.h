#ifndef IO_AUX_H
#define IO_AUX_H 1
#include <stdlib.h>

void flushSTDIN();
size_t readLineN(char* string, size_t size);
size_t freadLineN(FILE* stream, char* string, size_t size);
#endif