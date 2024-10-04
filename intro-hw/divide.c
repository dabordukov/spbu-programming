#include <stdio.h>
#include <stdlib.h>

int divide(int a, int b) {
    while (b == 0) {
        printf("Error: division by zero\nEnter another b = ");
        scanf("%d", &b);
    }

    int q = 0;
    int inverse_a = (a < 0) ? -1 : 1;
    int inverse_b = (b < 0) ? -1 : 1;
    
    while (inverse_b * (q + 1) * b <= a * inverse_a){
        q++;
    }
    
    if (a < 0 && abs(q * b) != abs(a)) {
        q++;
    }

    return inverse_a * inverse_b * q;
}

int main() {
    int a = 0;
    int b = 0;
    printf("Enter two space-separated numbers:\n");
    while (scanf("%d %d", &a, &b) == 2) {
        printf("a/b: %d \n", divide(a, b));
    }
}