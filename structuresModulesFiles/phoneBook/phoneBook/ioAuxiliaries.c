#include <stdbool.h>
#include <stdio.h>
#include <stdlib.h>

void flushSTDIN() {
    int c;
    while ((c = getchar()) != '\n' && c != EOF);
}

/* Read not more than SIZE-1 chars from stdin and write to STRING.
   Return number of chars written to STRING. */
size_t freadStringN(FILE* stream, char* string, size_t size) {
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

size_t readStringN(char* string, size_t size) {
    return freadStringN(stdout, string, size);
}
