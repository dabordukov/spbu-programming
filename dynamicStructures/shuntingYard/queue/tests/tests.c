#include <assert.h>
#include <stdbool.h>
#include <stdlib.h>

#include "queue.h"
#include "tokens.h"

Queue* testQueueInit() {
    Queue* queue = queueInit();
    assert(queue != NULL);
    return queue;
}

void testQueueFree(Queue** queue) {
    queueFree(queue);
    assert(*queue == NULL);
}

void testQueuePushPop() {
    Queue* queue = testQueueInit();
    Token token = charToToken('5');
    Token token2 = charToToken('/');
    Token token3 = charToToken('(');

    assert(queuePush(queue, token) == 0);
    assert(queueSize(queue) == 1);
    assert(queuePush(queue, token2) == 0);
    assert(queueSize(queue) == 2);
    assert(queuePush(queue, token3) == 0);
    assert(queueSize(queue) == 3);

    Token pop;
    assert(queuePop(queue, &pop) == 0);
    assert(pop.type == token.type && pop.value == token.value);
    assert(queueSize(queue) == 2);

    assert(queuePop(queue, &pop) == 0);
    assert(pop.type == token2.type && pop.value == token2.value);
    assert(queueSize(queue) == 1);

    assert(queuePop(queue, &pop) == 0);
    assert(pop.type == token3.type && pop.value == token3.value);
    assert(queueSize(queue) == 0);

    testQueueFree(&queue);
}

void testQueuePopEmpty(Queue* queue) {
    Token token;
    if (queuePop(queue, &token) != 1) {
        queuePush(queue, token);
        assert(false);
    }
}

void testQueueIsEmpty() {
    Queue* queue = testQueueInit();
    assert(queueIsEmpty(queue));
    testQueuePopEmpty(queue);
    assert(queueIsEmpty(queue));
    Token token = NULL_TOKEN;
    queuePush(queue, token);
    assert(!queueIsEmpty(queue));
    queuePop(queue, &token);
    assert(queueIsEmpty(queue));
    queueFree(&queue);
}

int main() {
    testQueueIsEmpty();
    testQueuePushPop();
}
