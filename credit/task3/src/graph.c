#include <assert.h>
#include <stdio.h>
#include <stdlib.h>

typedef struct graph {
    int size;
    int* adjMatrix;
} Graph;

// Graph graphInit(int size) {
//     Graph graph = {.size = 0, .adjMatrix = NULL};
//     graph.adjMatrix = calloc(size * size, sizeof(int));
//     if (graph.adjMatrix != NULL) {
//         graph.size = size;
//     }

//     return graph;
// }

Graph graphFromFile(char* filename) {
    Graph graph = {.size = 0, .adjMatrix = NULL};

    FILE* graphFile = fopen(filename, "r");
    if (graphFile == NULL) {
        return graph;
    }

    int size = 0;
    fscanf(graphFile, "%d\n", &size);
    if (size < 1) {
        return graph;
    }
    graph.adjMatrix = calloc(size * size, sizeof(int));
    if (graph.adjMatrix == NULL) {
        return graph;
    }
    graph.size = size;

    for (int i = 0; i < size; i++) {
        for (int j = 0; j < size; j++) {
            assert(fscanf(graphFile, "%d ", graph.adjMatrix + graph.size * i + j) == 1);
        }
    }

    return graph;
}

int* graphBFSArrayAllocate(Graph graph) {
    int* visited = calloc(graph.size, sizeof(int));
    if (visited == NULL) {
        return NULL;
    }

    for (int visit = 0; visit < graph.size; visit++) {
        if (visited[visit] == 1) {
            continue;
        }
    }
}