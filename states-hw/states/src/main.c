#include <stdio.h>

#include "graph.h"
#include "ioAuxiliaries.h"

int main(int argc, char* argv[]) {
    char* filename = NULL;
    int readBytes = 0;
    if (argc > 1) {
        filename = argv[1];
    } else {
        while (readBytes == 0) {
            printf("Введите имя файла с графом:\n");
            readBytes = readLine(&filename);
        }
    }

    Graph g = graphFromFile(filename);
    int* states;
    states = conquerCitiesAllocate(g);

    for (int state = 0; state < g.capitalsSize; state++) {
        printf("[State%d]: ", g.capitals[state]);
        int* currState = (states + state * (g.size + 1));
        for (int j = 0; currState[j] != -1 && j < g.size; j++) {
            printf("%d ", (states + state * (g.size + 1))[j]);
        }
        printf("\n");
    }

    if (readBytes > 0) {
        free(filename);
    }
    free(g.capitals);
    free(g.matrix);
    free(states);
}