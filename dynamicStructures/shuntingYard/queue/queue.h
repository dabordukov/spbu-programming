#ifndef QUEUE_H
#define QUEUE_H

#include <stdbool.h>

#include "../memoryAuxiliaries.h"
#include "../tokens.h"

typedef struct Queue Queue;

Queue *queueInit();

int queuePush(Queue *queue, Token token);

int queuePop(Queue *queue, Token *token);

bool isEmpty(Queue *queue);

void queueDispose(Queue *queue);

#endif