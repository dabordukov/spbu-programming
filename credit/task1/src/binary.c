unsigned int binaryToInteger(char* binaryNumber, int* error) {
    *error = 0;

    char* currChar = binaryNumber;
    while (*currChar != '\0') {
        if (*currChar != '0' && *currChar != '1') {
            *error = 1;
            return 0;
        }
        currChar++;
    }
    currChar--;

    int base = 1;
    unsigned integerNumber = 0;
    while (currChar != binaryNumber - 1) {
        if (*currChar == '1') {
            integerNumber |= base;
        }
        base *= 2;
        currChar--;
    }

    return integerNumber;
}
