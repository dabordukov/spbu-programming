#include <locale.h>
#include <stdbool.h>
#include <stddef.h>
#include <stdio.h>

#include "graph.h"
#include "ioAuxiliaries.h"

int main(int argc, char* argv[]) {
#ifdef _WIN32
    system("chcp 65001");
    setlocale(LC_CTYPE, "Ru.UTF-8");
#else
    setlocale(LC_CTYPE, "");
#endif

    Graph* graph = NULL;
    char* graphFileName = NULL;
    size_t read = 0;
    if (argc > 1) {
        graph = graphFromFile(argv[1]);
    } else {
        printf("Введите имя файла, содержащего граф:\n");
        read = readLine(&graphFileName);
    }
    if (graph == NULL) {
        printf("Не удалось прочитать граф\n");
        if (read) {
            free(graphFileName);
        }
        return 2;
    }

    graphPrint(graph, stdout);
    printf("\n");
    bool** reachabilityMatrix = reachabilityMatrixCalculate(graph);
    if (reachabilityMatrix == NULL) {
        fprintf(stderr, "Can't allocate enough memory\n");
        if (read) {
            free(graphFileName);
        }
        return 1;
    }

    printf("Вершины, из которых достижимы все остальные\n");
    for (size_t vertex = 0; vertex < graphSize(graph); vertex++) {
        bool allReachable = true;
        for (size_t i = 0; i < graphSize(graph); i++) {
            if (!reachabilityMatrix[vertex][i]) {
                allReachable = false;
                break;
            }
        }
        if (allReachable) {
            printf("%lu ", vertex);
        }
    }
    printf("\n");

    free2DArray(reachabilityMatrix, graphSize(graph));
    freeGraph(graph);
    if (read) {
        free(graphFileName);
    }
}
