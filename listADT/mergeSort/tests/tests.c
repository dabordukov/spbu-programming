#include <assert.h>
#include <locale.h>
#include <stdio.h>

#include "ioAuxiliaries.h"
#include "list.h"
#include "phonebook.h"

bool compareFiles(char* filename1, char* filename2) {
    FILE* f1 = fopen(filename1, "r");
    if (f1 == NULL) {
        return false;
    }
    FILE* f2 = fopen(filename2, "r");
    if (f2 == NULL) {
        fclose(f1);
        return false;
    }

    char a = EOF;
    char b = EOF;

    while ((a = fgetc(f1)) == (b = fgetc(f2)) && a != EOF);

    fclose(f1);
    fclose(f2);

    return a == b;
}

void testSortByName() {
    List* entries = readEntriesToList(TESTFILE);
    listMergeSort(entries, &phoneBookEntryComparatorName);

    FILE* tmp = fopen("test.tmp", "w");
    listPrintStream(entries, &phoneBookEntryFPrint, tmp);
    fclose(tmp);

    assert(compareFiles("test.tmp", CHECKFILE1));
}

void testSortByNumber() {
    List* entries = readEntriesToList(TESTFILE);
    listMergeSort(entries, &phoneBookEntryComparatorNumber);

    FILE* tmp = fopen("test.tmp", "w");
    listPrintStream(entries, &phoneBookEntryFPrint, tmp);
    fclose(tmp);

    assert(compareFiles("test.tmp", CHECKFILE2));
}

int main() {
    setlocale(LC_CTYPE, "");

    testSortByName();
    testSortByNumber();
}