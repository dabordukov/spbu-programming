#ifndef GRAPH_H
#define GRAPH_H

#include <stdbool.h>
#include <stdio.h>

typedef struct {
    int size;
    int* matrix;
    int capitalsSize;
    int* capitals;
} Graph;

Graph graphRead(FILE* stream);
Graph graphFromFile(char* filename);

int* conquerCitiesAllocate(Graph graph);
#endif