#include <locale.h>
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

    char* graphFilename = NULL;
    if (argc > 1) {
        graphFilename = argv[1];
    } else {
        printf("Введите имя файла, содержащего граф:\n");
        readLine(&graphFilename);
    }

    Graph graph = graphFromFile(graphFilename);

    for (int i = 0; i < graph.size; i++) {
        for (int j = 0; j < graph.size; j++) {
            printf("%d ", (graph.adjMatrix + i * graph.size)[j]);
        }
        printf("\n");
    }
}