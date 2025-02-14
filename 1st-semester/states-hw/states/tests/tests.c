#include <assert.h>

#include "graph.h"

void testCase1() {
    int correctResult[2][5] = {{0, 2, 3, -1, -1}, {4, 1, -1, -1, -1}};
    Graph g = graphFromFile(TEST_FILE1);
    int* states = conquerCitiesAllocate(g);

    for (int i = 0; i < g.capitalsSize; i++) {
        for (int j = 0; j < g.size; j++) {
            assert(correctResult[i][j] == (states + i * (g.size + 1))[j]);
        }
    }
}

void testCase2() {
    int correctResult[3][15] = {{14, 12, 0, 2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
                                {6, 1, 3, 7, 8, 4, 5, -1, -1, -1, -1, -1, -1, -1, -1},
                                {9, 10, 13, 11, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1}};
    Graph g = graphFromFile(TEST_FILE2);
    int* states = conquerCitiesAllocate(g);

    for (int i = 0; i < g.capitalsSize; i++) {
        for (int j = 0; j < g.size; j++) {
            assert(correctResult[i][j] == (states + i * (g.size + 1))[j]);
        }
    }
}
int main(int argc, char* argv[]) {
    testCase1();
    testCase2();
}