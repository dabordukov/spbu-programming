#ifndef AUX_H
#define AUX_H 1
void* checkedMalloc(size_t size);
void* checkedCalloc(size_t nmemb, size_t size);
void swapBits(char* a, char* b);
void printBits(char* bits, int size);
void reverseBits(char* bits, int size);
#endif