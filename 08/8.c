#include <string.h>
#include <stdio.h>
#include <stdlib.h>

#define RowSize 6
#define ColSize 50

void init(char grid[RowSize][ColSize]) {
    for (int r = 0; r < RowSize; r++) {
        for (int c = 0; c < ColSize; c++) {
            grid[r][c] = '.';
        }
    }
}

void print(char grid[RowSize][ColSize]) {
    for (int r = 0; r < RowSize; r++) {
        for (int c = 0; c < ColSize; c++) {
            putchar(grid[r][c]);
        }
        printf("\n");
    }
}

void rectangle(char grid[RowSize][ColSize], int cRect, int rRect) {
    for (int r = 0; r < rRect; r++) {
        for (int c = 0; c < cRect; c++) {
            grid[r][c] = '#';
        }
    }
}

void rotateRow(char grid[RowSize][ColSize], int row, int distance) {
    int splitPoint = ColSize - distance;
    char wrapAround[distance];

    for (int c = 0; c < distance; c++) {
        wrapAround[c] = grid[row][c + splitPoint];
    }

    for (int c = ColSize - 1; c >= distance; c--) {
        grid[row][c] = grid[row][c - distance];
    }

    for (int c = 0; c < distance; c++) {
        grid[row][c] = wrapAround[c];
    }
}

void rotateCol(char grid[RowSize][ColSize], int col, int distance) {
    int splitPoint = RowSize - distance;
    char wrapAround[distance];

    for (int r = 0; r < distance; r++) {
        wrapAround[r] = grid[r + splitPoint][col];
    }

    for (int r = RowSize - 1; r >= distance; r--) {
        grid[r][col] = grid[r - distance][col];
    }

    for (int r = 0; r < distance; r++) {
        grid[r][col] = wrapAround[r];
    }
}

// Note: Input was preprocessed such that the format is always:
//   c,x,y
// Where c is one of 'r', 'x', or 'y' (rect, rotate x, rotate y),
// and x & y are numbers
int main() {
    char grid[RowSize][ColSize];
    init(grid);

    FILE* input = fopen("preprocessed-input.txt", "r");
    char command;
    int i1, i2;
    while (3 == fscanf(input, "%c,%i,%i\n", &command, &i1, &i2)) {
        switch (command) {
        case 'r': rectangle(grid, i1, i2); break;
        case 'x': rotateCol(grid, i1, i2); break;
        case 'y': rotateRow(grid, i1, i2); break;
        }
    }

    int count = 0;
    for (int r = 0; r < RowSize; r++) for (int c = 0; c < ColSize; c++) if (grid[r][c] == '#') count++;
    printf("Count is %i\n", count);
    print(grid);
}
