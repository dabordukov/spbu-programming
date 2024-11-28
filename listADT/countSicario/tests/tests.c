#include <assert.h>
#include <stdint.h>

#include "sicarioKilling.h"

#define ASSERT_SICARIO(x, y, z) assert(sicarioKilling(x, y) == z)

int main() {
    ASSERT_SICARIO(1, 1, 1);
    ASSERT_SICARIO(5, 5, 2);
    ASSERT_SICARIO(41, 3, 31);
    ASSERT_SICARIO(55, 55, 26);
}