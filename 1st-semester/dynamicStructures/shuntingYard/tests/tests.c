#include <assert.h>
#include <stdlib.h>
#include <string.h>

#include "shuntingYard.h"

void ASSERT_SY(char* infix, char* postfix) {
    char* str = NULL;
    shuntingYardAlgorithm(infix, &str);
    assert(strcmp(postfix, str) == 0);
}

int main() {
    ASSERT_SY("(1 + 2) * 3", "1 2 + 3 *");
    ASSERT_SY("3 * (1 + 2)", "3 1 2 + *");
    ASSERT_SY("1 / 2 / 3 / 4", "1 2 / 3 / 4 /");
    ASSERT_SY("1 - 2 - 3 - 4", "1 2 - 3 - 4 -");
    ASSERT_SY("1 + 2 / 3 - 4", "1 2 3 / + 4 -");
    ASSERT_SY("1 * 2 / 3 + 7 / 8", "1 2 * 3 / 7 8 / +");
}