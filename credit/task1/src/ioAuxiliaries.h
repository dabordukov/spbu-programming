#ifndef IO_AUXILIARIES_H
#define IO_AUXILIARIES_H
#include <stdio.h>
#include <stdlib.h>

/* Clear stdin until '\n' or EOF*/
void flushSTDIN();

/* read from stdin until newline or EOF
   sets STRING pointer to new string
   returns length of new string*/
size_t readLine(char** string);

/* read from STREAM until newline or EOF
   sets STRING pointer to new string
   returns length of new string*/
size_t fReadLine(FILE* stream, char** string);

#endif