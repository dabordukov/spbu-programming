#ifndef QUEUE_H
#define QUEUE_H

#include <stdbool.h>

#include "memoryAuxiliaries.h"
#include "tokens.h"

typedef struct Queue Queue;

typedef enum QueueError {
    QUEUE_OK,
    QUEUE_IS_EMPTY,
    QUEUE_NO_MEMORY
} QueueError;

Queue *queueInit();

int queuePush(Queue *queue, Token token);

int queuePop(Queue *queue, Token *token);

bool queueIsEmpty(Queue *queue);

void queueFree(Queue **queue);

size_t queueSize(Queue *queue);
void queuePrint(Queue *queue);
#endif