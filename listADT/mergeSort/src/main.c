#include <locale.h>
#include <stdio.h>

#include "list.h"
#include "phonebook.h"

void printUsage(char* filename) {
    printf("Usage: %s [sort mode] [path]\nSort mode:\n\t1 - sort by name\n\t2 - sort by number\nExample: %s 1 phonebook.txt\n", filename, filename);
}

int main(int argc, char* argv[]) {
    setlocale(LC_CTYPE, "");
    List* entries = NULL;
    if (argc == 3) {
        entries = readEntriesToList(argv[2]);
        if (entries == NULL) {
            fprintf(stderr, "Can't read file %s\n", argv[2]);
            return 1;
        }
        switch (argv[1][0]) {
            case '1':
                listMergeSort(entries, &phoneBookEntryComparatorName);
                break;
            case '2':
                listMergeSort(entries, &phoneBookEntryComparatorNumber);
                break;
            default:
                printUsage(argv[0]);
                break;
        }

        listPrint(entries, &phoneBookEntryPrint);
    } else {
        printUsage(argv[0]);
    }

    listFree(&entries);
}