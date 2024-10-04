#include "my_string.h"

#include <string.h>

#include "auxiliaries.h"

string newString(char* data) {
    string string_instance;
    string_instance.size = strlen(data);
    string_instance.data = checkedMalloc(sizeof(char) * string_instance.size);
    strncpy(string_instance.data, data, string_instance.size);
    return string_instance;
}