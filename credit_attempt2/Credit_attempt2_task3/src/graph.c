#include "graph.h"

#include <stdbool.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

typedef struct Graph {
    int size;
    bool** adjacencyMatrix;
} Graph;

size_t graphSize(Graph* graph) {
    if (graph == NULL) {
        return 0;
    }
    return graph->size;
}

void freeGraph(Graph* graph) {
    free2DArray(graph->adjacencyMatrix, graph->size);
    free(graph);
}

static bool** allocate2DArray(size_t size) {
    bool** array = calloc(size, sizeof(bool*));
    if (array == NULL) {
        return NULL;
    }
    for (size_t i = 0; i < size; i++) {
        array[i] = calloc(size, sizeof(bool));
        if (array[i] == NULL) {
            // free already allocated rows
            while (i--) {
                free(array[i]);
            }

            free(array);
            return NULL;
        }
    }

    return array;
}

void free2DArray(bool** array, size_t size) {
    if (array == NULL) {
        return;
    }

    for (size_t i = 0; i < size; i++) {
        free(array[i]);
    }
    free(array);
}

Graph* graphFromFile(char* filename) {
    Graph* graph = calloc(1, sizeof(Graph));
    if (graph == NULL) {
        return NULL;
    }

    FILE* graphFile = fopen(filename, "r");
    if (graphFile == NULL) {
        free(graph);
        return NULL;
    }

    int size = 0;
    fscanf(graphFile, "%d\n", &size);
    if (size < 1) {
        free(graph);
        fclose(graphFile);
        return NULL;
    }

    graph->adjacencyMatrix = allocate2DArray(size);
    if (graph->adjacencyMatrix == NULL) {
        fclose(graphFile);
        free(graph);
        return NULL;
    }

    graph->size = size;

    for (size_t i = 0; i < size; i++) {
        for (size_t j = 0; j < size; j++) {
            int number = 0;
            if (fscanf(graphFile, "%d ", &number) != 1) {
                free2DArray(graph->adjacencyMatrix, graph->size);
                free(graph);
                fclose(graphFile);
                return NULL;
            }

            if (number == 1) {
                graph->adjacencyMatrix[i][j] = true;
            }
        }
    }

    fclose(graphFile);
    return graph;
}

void print2DArray(FILE* stream, bool** array, size_t size) {
    for (size_t i = 0; i < size; i++) {
        for (size_t j = 0; j < size; j++) {
            if (array[i][j]) {
                fprintf(stream, "1 ");
            } else {
                fprintf(stream, "0 ");
            }
        }
        fprintf(stream, "\n");
    }
}

void graphPrint(Graph* graph, FILE* stream) {
    if (graph == NULL) {
        return;
    }

    print2DArray(stream, graph->adjacencyMatrix, graph->size);
}

static bool** matrixMuliplicate(bool** matrixA, bool** matrixB, size_t size) {
    bool** result = allocate2DArray(size);
    if (result == NULL) {
        return NULL;
    }

    for (size_t i = 0; i < size; i++) {
        for (size_t j = 0; j < size; j++) {
            for (size_t k = 0; k < size; k++) {
                if (matrixA[i][k] && matrixB[k][j]) {
                    result[i][j] = true;
                    break;
                }
            }
        }
    }

    return result;
}

bool** reachabilityMatrixCalculate(Graph* graph) {
    if (graph == NULL) {
        return NULL;
    }

    size_t size = graph->size;

    bool** reachabilityMatrix = allocate2DArray(size);
    if (reachabilityMatrix == NULL) {
        return NULL;
    }
    for (size_t i = 0; i < size; i++) {
        reachabilityMatrix[i][i] = true;
    }

    bool** baseMatrix = allocate2DArray(size);
    if (baseMatrix == NULL) {
        free2DArray(reachabilityMatrix, size);
        return NULL;
    }

    for (size_t i = 0; i < size; i++) {
        memcpy(baseMatrix[i], graph->adjacencyMatrix[i], size);
    }

    for (size_t power = 0; power < size; power++) {
        // Unite matrices
        for (size_t i = 0; i < size; i++) {
            for (size_t j = 0; j < size; j++) {
                reachabilityMatrix[i][j] = reachabilityMatrix[i][j] || baseMatrix[i][j];
            }
        }

        if (power == size - 1) {
            break;
        }

        bool** newMatrix = matrixMuliplicate(baseMatrix, graph->adjacencyMatrix, size);
        if (newMatrix == NULL) {
            free2DArray(reachabilityMatrix, size);
            free2DArray(baseMatrix, size);
            return NULL;
        }
        free2DArray(baseMatrix, size);
        baseMatrix = newMatrix;
    }
    free2DArray(baseMatrix, size);

    return reachabilityMatrix;
}