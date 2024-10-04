#ifndef BINARY_REPRESENTATION_H
#define BINARY_REPRESENTATION_H 1

#define BINARY_SIZE (sizeof(int) * 8)
char* convertDecimalTo2Complement(int x, int size);
void binaryAdd(char* a, char* b, int size);
void negativeBinary(char* bits, int size);
void binaryAddOne(char* bits, int size);
void invertBits(char* bits, int size);
int convert2ComplementToDecimal(char* bits, int size);
#endif