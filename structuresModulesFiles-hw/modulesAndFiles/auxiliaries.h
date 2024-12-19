#ifndef AUX_h
#define AUX_H 1
#include <stdbool.h>
#include <stdio.h>

void swap(int* a, int* b);
void printArray(int* array, int size);
void fillArrayRand(int* array, int size, int rangeMax);
bool addIntOverflow(int a, int b);
void* checkedMalloc(size_t size);
int* parseArrayFromFileAllocate(char* filename, int* arraySze);
#endif