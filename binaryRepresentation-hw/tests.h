#ifndef TESTS_BINARY_REPRESENTATION_H
#define TESTS_BINARY_REPRESENTATION_H 1
void testBinaryAddOne(void (*binaryAddOne)(char *, int));
void testInvertBits(void (*invertBits)(char *, int));
void testNegativeBinary(void (*negativeBinary)(char *, int));
void testConvertDecimalTo2Complement(char *(*convertDecimalTo2Complement)(int, int));
void testBinaryAdd(void (*binaryAdd)(char *, char *, int));
void testConvert2ComplementToDecimal(int (*convert2ComplementToDecimal)(char *, int));
#endif