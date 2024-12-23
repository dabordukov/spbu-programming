#include <assert.h>

#include "graph.h"

void testCase1() {
    Graph* graph = graphFromFile(TEST_FILENAME);
    assert(graph != NULL);

    bool** reachabilityMatrix = reachabilityMatrixCalculate(graph);
    assert(reachabilityMatrix != NULL);

    // vertex 5 can reach every other vertex
    for (size_t i = 0; i < graphSize(graph); i++) {
        assert(reachabilityMatrix[5][i]);
    }

    // no more vertices that can reach every other vertex
    for (size_t vertex = 0; vertex < graphSize(graph) - 1; vertex++) {
        bool allReachable = true;
        for (size_t i = 0; i < graphSize(graph); i++) {
            if (!reachabilityMatrix[vertex][i]) {
                allReachable = false;
                break;
            }
        }
        assert(!allReachable);
    }
}

int main(void) {
    testCase1();
}
