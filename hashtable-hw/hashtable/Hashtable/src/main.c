#include <hashtable.h>
#include <stdio.h>

int main(int argc, char* argv[]) {
    Hashtable* ht = hashtableInit();
    for (int i = 65; i < 95; i++) {
        char key[5] = {};
        sprintf(key, "%c", i);
        hashtableUpdate(ht, key, i);
    }

    printf("%f ", fillFactor(ht));
    printf("%ld ", meanBucketLength(ht));
    printf("%ld\n", maxBucketLength(ht));

    hashtableFree(&ht);
}