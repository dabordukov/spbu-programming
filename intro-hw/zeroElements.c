/*
Нулевые элементы
Напишите программу, считающую количество нулевых элементов в массиве
*/

#include <stdio.h>
#include <stdlib.h>
#include <time.h>

int count_zeros(int array[], int array_size){
    int count = 0;

    for (int i = 0; i < array_size; i++) {
        if (array[i] == 0) {
            count++;
        }
    }

    return count;
}

int main() {
    int array_size = 20;
    int array[array_size];
    srand(time(NULL));
    for(int i = 0; i < array_size; i++){
        array[i] = rand()%10;
        printf("%d ", array[i]);
    }
    puts("");
    
    int zeros = count_zeros(array, array_size);
    printf("%d zeros\n", zeros);
    
}