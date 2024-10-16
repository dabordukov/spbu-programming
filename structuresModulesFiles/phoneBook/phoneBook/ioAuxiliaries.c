#include <stdbool.h>
#include <stdio.h>
#include <stdlib.h>

void flushSTDIN() {
    int c;
    while ((c = getchar()) != '\n' && c != EOF);
}

/* Read from STREAM until newline, not more than SIZE-1 chars and write to STRING.
   Return number of chars written to STRING. */
size_t freadLineN(FILE* stream, char* string, size_t size) {
    size_t count = 0;
    size--;  // leave one byte for terminator
    int c = EOF;

    while (size-- && (c = fgetc(stream)) != '\n' && c != EOF) {
        string[count++] = c;
    }
    string[count] = 0;  // terminator

    if (c != '\n' && c != EOF) {
        flushSTDIN();
    }

    return count;
}

size_t readLineN(char* string, size_t size) {
    return freadLineN(stdout, string, size);
}
