#ifndef ORIENTEDGRAPH_H_
#define ORIENTEDGRAPH_H_

#include <stdbool.h>
#include <stdio.h>

typedef struct Graph Graph;

/* Read number of vertices and adjacency matrix from file */
Graph* graphFromFile(char* filename);

/* Print array of pointers to array of bool*/
void print2DArray(FILE* stream, bool** array, size_t size);

/* Print adjacency matrix in stream */
void graphPrint(Graph* graph, FILE* stream);

/* Calculate reachability matrix of the graph
   Return NULL on allocation error or NULL graph*/
bool** reachabilityMatrixCalculate(Graph* graph);

/* Return number of vertices */
size_t graphSize(Graph* graph);

/* 2D Array free*/
void free2DArray(bool** array, size_t size);

/* Free graph */
void freeGraph(Graph* graph);

#endif
