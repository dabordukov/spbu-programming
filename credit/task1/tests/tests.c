#include <assert.h>

#include "binary.h"

int main() {
    int error = 0;
    assert(binaryToInteger("0101", &error) == 5);
    assert(binaryToInteger("010100", &error) == 20);
    assert(binaryToInteger("1111", &error) == 15);
}