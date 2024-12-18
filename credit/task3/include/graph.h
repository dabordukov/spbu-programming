#ifndef GRAPH_H
#define GRAPH_H

typedef struct graph {
    int size;
    int* adjMatrix;
} Graph;

// Read graph from file
Graph graphFromFile(char* filename);

#endif
