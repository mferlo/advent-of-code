class Day08 {
  val testInput = Seq("30373", "25512", "65332", "33549", "35390")
  val realInput = FileReader.read(8)

  val grid: List[List[Int]] = realInput.map(parseLine).toList
  val rows = grid.size
  val cols = grid.head.size
  val coords: List[(Int, Int)] = (0 until rows).flatMap(row => (0 until cols).map(col => (row, col))).toList

  def parseLine(line: String): List[Int] = {
    line.map(_.asDigit).toList
  }

  def visibleFromTop(r: Int, c: Int): Boolean = (0 until r).map(grid(_)(c)).forall(_ < grid(r)(c))
  def visibleFromBottom(r: Int, c: Int): Boolean = (r + 1 until rows).map(grid(_)(c)).forall(_ < grid(r)(c))
  def visibleFromLeft(r: Int, c: Int): Boolean = (0 until c).map(grid(r)(_)).forall(_ < grid(r)(c))
  def visibleFromRight(r: Int, c: Int): Boolean = (c + 1 until cols).map(grid(r)(_)).forall(_ < grid(r)(c))
  def visible(r: Int, c: Int) = visibleFromTop(r, c) || visibleFromLeft(r, c) || visibleFromBottom(r, c) || visibleFromRight(r, c)
  def part1: Int = coords.count(pos => visible(pos._1, pos._2))

  def countUntilNotLessThan(n: Int, s: Seq[Int]): Int =
    s.span(_ < n) match {
      case (shorter, Nil) => shorter.size
      case (shorter, _) => shorter.size + 1
    }
  def scenicScoreUp(r: Int, c: Int, h: Int): Int = countUntilNotLessThan(h, Range.inclusive(r - 1, 0, -1).map(grid(_)(c)))
  def scenicScoreDown(r: Int, c: Int, h: Int): Int = countUntilNotLessThan(h, (r + 1 until rows).map(grid(_)(c)))
  def scenicScoreLeft(r: Int, c: Int, h: Int): Int = countUntilNotLessThan(h, Range.inclusive(c - 1, 0, -1).map(grid(r)(_)))
  def scenicScoreRight(r: Int, c: Int, h: Int): Int = countUntilNotLessThan(h, (c + 1 until cols).map(grid(r)(_)))
  def scenicScore(r: Int, c: Int): Int =
    scenicScoreUp(r, c, grid(r)(c)) *
      scenicScoreDown(r, c, grid(r)(c)) *
      scenicScoreLeft(r, c, grid(r)(c)) *
      scenicScoreRight(r, c, grid(r)(c))
  def part2: Int = coords.map(pos => scenicScore(pos._1, pos._2)).max
}
