#include "graph.h"

#include <limits.h>
#include <stdbool.h>
#include <stdio.h>
#include <stdlib.h>

Graph graphRead(FILE* stream) {
    Graph graph = {.size = 0, .matrix = NULL, .capitalsSize = 0, .capitals = NULL};
    int cities = 0;
    int roads = 0;
    fscanf(stream, "%d %d", &cities, &roads);

    graph.matrix = calloc(cities * cities, sizeof(int));
    if (graph.matrix == NULL) {
        return graph;
    }
    for (int i = 0; i < cities; i++) {
        for (int j = 0; j < cities; j++) {
            (graph.matrix + i * cities)[j] = -1;
        }
    }

    graph.size = cities;

    for (int i = 0; i < roads; i++) {
        int city1 = -1;
        int city2 = -1;
        int length = -1;
        fscanf(stream, "%d %d %d", &city1, &city2, &length);
        (graph.matrix + city1 * cities)[city2] = length;
        (graph.matrix + city2 * cities)[city1] = length;
    }

    fscanf(stream, "%d", &graph.capitalsSize);

    graph.capitals = calloc(graph.capitalsSize, sizeof(int));
    if (graph.capitals == NULL) {
        free(graph.matrix);
        graph.size = 0;
        return graph;
    }

    for (int i = 0; i < graph.capitalsSize; i++) {
        fscanf(stream, "%d", graph.capitals + i);
    }

    return graph;
}

Graph graphFromFile(char* filename) {
    Graph graph = {.size = 0, .matrix = NULL};

    FILE* graphFile = fopen(filename, "r");
    if (graphFile == NULL) {
        return graph;
    }

    graph = graphRead(graphFile);

    fclose(graphFile);
    return graph;
}

int* conquerCitiesAllocate(Graph graph) {
    int* states = calloc(graph.capitalsSize * (graph.size + 1), sizeof(int));  // last integer in a row for number of cities in the state

    if (states == NULL) {
        return NULL;
    }
    for (int i = 0; i < graph.capitalsSize; i++) {
        for (int j = 0; j < graph.size; j++) {
            (states + i * (graph.size + 1))[j] = -1;
        }
    }

    bool* conquered = calloc(graph.size, sizeof(bool));

    for (int capital = 0; capital < graph.capitalsSize; capital++) {
        conquered[graph.capitals[capital]] = true;

        (states + capital * (graph.size + 1))[graph.size]++;
        (states + capital * (graph.size + 1))[0] = graph.capitals[capital];
    }

    int freeCities = graph.size - graph.capitalsSize;
    for (int state = 0; state < graph.capitalsSize && freeCities > 0; state = (state + 1) % graph.capitalsSize) {
        int minLength = INT_MAX;
        int nearestCity = -1;
        for (int city = 0; (states + state * (graph.size + 1))[city] != -1; city++) {
            int cityID = (states + state * (graph.size + 1))[city];
            for (int neighbour = 0; neighbour < graph.size; neighbour++) {
                int length = (graph.matrix + cityID * graph.size)[neighbour];
                if (length == -1 || conquered[neighbour]) {
                    continue;
                }

                if (length < minLength) {
                    minLength = length;
                    nearestCity = neighbour;
                }
            }
        }
        if (nearestCity == -1) {
            continue;
        }

        conquered[nearestCity] = true;

        int currStateSize = (states + state * (graph.size + 1))[graph.size]++;
        (states + state * (graph.size + 1))[currStateSize] = nearestCity;
        freeCities--;
    }

    free(conquered);
    return states;
}
