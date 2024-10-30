#include "queue.h"

#include <stdbool.h>
#include <stdio.h>
#include <stdlib.h>

#include "../tokens.h"
typedef struct QueueNode {
    Token token;
    struct QueueNode* next;
} QueueNode;

typedef struct Queue {
    QueueNode* first;
    QueueNode* last;
    size_t size;
} Queue;

Queue* queueInit() {
    return calloc(1, sizeof(Queue));
}

bool queueIsEmpty(Queue* queue) {
    return queue->size == 0;
}

size_t queueSize(Queue* queue) {
    return queue->size;
}

int queuePush(Queue* queue, Token token) {
    QueueNode* newNode = calloc(1, sizeof(QueueNode));
    if (!newNode) {
        return QUEUE_NO_MEMORY;
    }
    newNode->token = token;

    if (queueIsEmpty(queue)) {
        queue->first = newNode;
    } else {
        queue->last->next = newNode;
    }
    queue->last = newNode;

    queue->size++;

    return QUEUE_OK;
}

int queuePop(Queue* queue, Token* token) {
    if (queueIsEmpty(queue)) {
        return QUEUE_IS_EMPTY;
    }

    *token = queue->first->token;

    QueueNode* old = queue->first;
    queue->first = old->next;
    free(old);

    queue->size--;

    return 0;
}

void queueFree(Queue** queue) {
    while (!queueIsEmpty(*queue)) {
        Token trash;
        queuePop(*queue, &trash);
    }
    free(*queue);

    *queue = NULL;
}

void queuePrint(Queue* queue) {
    if (queueSize(queue) == 0) {
        return;
    }
    for (QueueNode* ptr = queue->first; ptr != NULL; ptr = ptr->next) {
        if (ptr->token.type == TOKEN_INTEGER) {
            printf("%c ", ptr->token.value + '0');
        } else {
            printf("%c ", ptr->token.value);
        }
    }
    printf("\n");
}