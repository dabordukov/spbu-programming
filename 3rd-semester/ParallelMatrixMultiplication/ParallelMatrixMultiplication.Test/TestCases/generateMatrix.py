import sys
import random

if(len(sys.argv) < 3):
	print(f"Usage:\npython {sys.argv[0]} ROWS COLUMNS")

ROWS = int(sys.argv[1])
COLUMNS = int(sys.argv[2])
RANGE = 20

for i in range(ROWS):
	for j in range(COLUMNS):
		print(random.randint(-RANGE, RANGE), end='')
		if j < COLUMNS - 1:
			print(end=' ')
	print()


