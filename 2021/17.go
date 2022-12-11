package main

/*

import (
	"fmt"
)

type Target struct {
	MinX int
	MaxX int
	MinY int
	MaxY int
}

type Hit struct {
	Start, End int
}

func overlap(h1, h2 Hit) bool {
	return h1.Start <= h2.End && h2.Start <= h1.End
}

func xHit(target Target, dx int) (bool, Hit) {
	t := 0
	x := 0

	isHit := false
	var start, end int

	for x <= target.MaxX {
		if dx == 0 {
			if target.MinX <= x && x <= target.MaxX {
				end = 999
			}

			break
		}

		t++
		x += dx
		dx--

		if target.MinX <= x && x <= target.MaxX {
			if !isHit {
				isHit = true
				start = t
			}
			end = t
		}
	}

	return isHit, Hit{start, end}

}

func xHits(target Target) map[int]Hit {
	result := make(map[int]Hit)

	xMin := 0
	xMinDist := 0
	for xMinDist < target.MinX {
		xMin++
		xMinDist += xMin
	}

	for dx := xMin; dx <= target.MaxX; dx++ {
		isHit, hit := xHit(target, dx)
		if isHit {
			result[dx] = hit
		}
	}

	return result
}

func yHit(target Target, dy int) (bool, Hit) {
	t := 0
	y := 0

	isHit := false
	var start, end int

	for y >= target.MinY {
		t++
		y += dy
		dy--

		if target.MinY <= y && y <= target.MaxY {
			if !isHit {
				isHit = true
				start = t
			}
			end = t
		}
	}

	return isHit, Hit{start, end}
}

func yHits(target Target, dyMax int) map[int]Hit {
	result := make(map[int]Hit)

	for dy := target.MinY; dy <= dyMax; dy++ {
		isHit, hit := yHit(target, dy)
		if isHit {
			result[dy] = hit
		}
	}

	return result
}

func numSolutions(target Target, dyMax int) int {
	xHits := xHits(target)
	yHits := yHits(target, dyMax)

	count := 0
	for _, xHit := range xHits {
		for _, yHit := range yHits {
			if overlap(xHit, yHit) {
				count++
			}
		}
	}

	return count
}

func apex(target Target) (maxHeight, dyMax int) {
	// Assume that you can reach the target horizontally in much less time than your vertical movement takes
	// Assume the target is below you (this is the case for both the example and the input)

	// Shoot up at some dy
	// At t = dy, it will be at its apex (also at t = dy+1). The height will be triangle(dy)
	// At t = 2dy + 1, it will be at y=0 with velocity -(dy+1)
	// And then at the next step, we want it to intercept the very bottom of the target zone
	// So we want -(dy+1) == target.MinY => dy = -target.MinY - 1
	dyMax = -target.MinY - 1

	// What's the max of this? The triangular number for it
	maxHeight = 0
	for i := 0; i <= dyMax; i++ {
		maxHeight += i
	}
	return
}

func main() {
	testTarget := Target{20, 30, -10, -5}
	target := Target{29, 73, -248, -194}

	testMaxHeight, testdyMax := apex(testTarget)
	maxHeight, dyMax := apex(target)

	fmt.Println(testMaxHeight, maxHeight)

	fmt.Println(numSolutions(testTarget, testdyMax), numSolutions(target, dyMax))
}
*/
