#ifndef IO_AUXILIARIES_H
#define IO_AUXILIARIES_H 1
#include <stdio.h>
#include <stdlib.h>

void flushSTDIN();
/* Read from stdin until newline, not more than SIZE-1 chars and write to STRING.
   Return number of chars written to STRING. */
size_t readLineN(char* string, size_t size);

/* Read from STREAM until newline, not more than SIZE-1 chars and write to STRING.
   Return number of chars written to STRING. */
size_t freadLineN(FILE* stream, char* string, size_t size);

/* read from stdin until newline or EOF
   sets STRING pointer to new string
   sets ERROR to 1 on allocation errors
   returns length of new string*/
size_t readLine(char** string, int* error);

/* read from STREAM until newline or EOF
   sets STRING pointer to new string
   sets ERROR to 1 on allocation errors
   returns length of new string*/
size_t freadLine(FILE* stream, char** string, int* error);
#endif