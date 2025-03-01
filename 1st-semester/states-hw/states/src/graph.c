#include "graph.h"

#include <limits.h>
#include <stdbool.h>
#include <stdio.h>
#include <stdlib.h>

Graph graphRead(FILE* stream) {
    Graph graph = {.size = 0, .matrix = NULL, .capitalsSize = 0, .capitals = NULL};
    int cities = 0;
    int roads = 0;
    if (fscanf(stream, "%d %d", &cities, &roads) != 2) {
        return graph;
    }

    int* matrix = calloc(cities * cities, sizeof(int));
    if (matrix == NULL) {
        return graph;
    }
    for (int i = 0; i < cities; i++) {
        for (int j = 0; j < cities; j++) {
            (matrix + i * cities)[j] = -1;
        }
    }

    for (int i = 0; i < roads; i++) {
        int city1 = -1;
        int city2 = -1;
        int length = -1;
        if (fscanf(stream, "%d %d %d", &city1, &city2, &length) != 3) {
            return graph;
        }

        (matrix + city1 * cities)[city2] = length;
        (matrix + city2 * cities)[city1] = length;
    }

    if (fscanf(stream, "%d", &graph.capitalsSize) != 1) {
        return graph;
    }

    int* capitals = calloc(graph.capitalsSize, sizeof(int));
    if (capitals == NULL) {
        return graph;
    }

    for (int i = 0; i < graph.capitalsSize; i++) {
        if (fscanf(stream, "%d", capitals + i) != 1) {
            return graph;
        }
    }

    graph.matrix = matrix;
    graph.capitals = capitals;
    graph.size = cities;
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

    // initialize states with capitals
    for (int capital = 0; capital < graph.capitalsSize; capital++) {
        conquered[graph.capitals[capital]] = true;

        (states + capital * (graph.size + 1))[graph.size] = 1;
        (states + capital * (graph.size + 1))[0] = graph.capitals[capital];
    }

    int freeCities = graph.size - graph.capitalsSize;
    for (int state = 0; state < graph.capitalsSize && freeCities > 0; state = (state + 1) % graph.capitalsSize) {
        int* currState = (states + state * (graph.size + 1));
        int minLength = INT_MAX;
        int nearestCity = -1;

        // iterate over the state's cities
        for (int city = 0; currState[city] != -1; city++) {
            int* currCity = (graph.matrix + currState[city] * graph.size);

            // check cities neighbour to the current state's city
            for (int neighbour = 0; neighbour < graph.size; neighbour++) {
                if (conquered[neighbour]) {
                    continue;
                }

                int length = currCity[neighbour];
                if (length == -1) {
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

        int currStateSize = currState[graph.size]++;
        currState[currStateSize] = nearestCity;
        freeCities--;
    }

    free(conquered);
    return states;
}
