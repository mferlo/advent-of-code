#include <stdio.h>
#include <stdlib.h>
#include <stdbool.h>

typedef enum Dir { UP, LEFT, DOWN, RIGHT } Dir;
typedef struct GridState { int x, y; Dir d; bool justMovedOut;} GridState;

void step(GridState* s) {
    switch (s->d) {
        case RIGHT: s->x += 1; break;
        case LEFT:  s->x -= 1; break;
        case UP:    s->y += 1; break;
        case DOWN:  s->y -= 1; break;
    }

    if (s->justMovedOut) {
        s->d = UP;
        s->justMovedOut = false;
    } else if (abs(s->x) == abs(s->y)) {
        if (s->d == RIGHT) { // Just finished the square; don't turn yet
            s->justMovedOut = true;
        } else { // Finished a side; turn the corner to do next
            s->d = (s->d + 1) % 4;
        }
    }
}

int part1(int input) {
    GridState s = { 0, 0, RIGHT, true };
    for (int i = 1; i < input; i += 1) {
        step(&s);
    }

    return abs(s.x) + abs(s.y);
}

// Fake a 2d array with indexes from -50 to 50, from one that's 0 to 100
const int origin = 50;
const int DIM = 101;
void set(int b[DIM][DIM], int x, int y, int value) {
    b[x+origin][y+origin] = value;
}

int sumNeighbors(int b[DIM][DIM], int xIn, int yIn) {
    int x = xIn + origin;
    int y = yIn + origin;
    return b[x-1][y+1] + b[x][y+1] + b[x+1][y+1]
         + b[x-1][y] /*+ b[x][y] */+ b[x+1][y]
         + b[x-1][y-1] + b[x][y-1] + b[x+1][y-1];
}


int part2(int input) {
    int board[DIM][DIM];
    for (int i = 0; i < DIM; i++) {
        for (int j = 0; j < DIM; j++) {
            board[i][j] = 0;
        }
    }

    GridState s = { 0, 0, RIGHT, true };
    int value = 1;
    set(board, s.x, s.y, value);
    while (value <= input) {
        step(&s);
        value = sumNeighbors(board, s.x, s.y);
        set(board, s.x, s.y, value);
    }
    return value;
}

int main() {
    printf("%d\n%d\n", part1(368078), part2(368078));
}
