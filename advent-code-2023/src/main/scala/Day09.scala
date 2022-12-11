import scala.collection.mutable.ListBuffer

class Coordinate(val r: Int, val c: Int) {
  def move(dir: String): Coordinate =
    dir match {
      case "U" => new Coordinate(r - 1, c)
      case "D" => new Coordinate(r + 1, c)
      case "L" => new Coordinate(r, c - 1)
      case "R" => new Coordinate(r, c + 1)
    }

  def isTouching(other: Coordinate) =
    Math.abs(r - other.r) <= 1 && Math.abs(c - other.c) <= 1

  def trailTo(head: Coordinate): Coordinate =
    if (isTouching(head))
      this
    else
      new Coordinate(r + head.r.compareTo(r), c + head.c.compareTo(c))

  def asTuple(): (Int, Int) = (r, c)
  override def toString: String = s"($r, $c)"
}

object Coordinate {
  def Origin: Coordinate = new Coordinate(0, 0)
}

trait Rope {
  def move(dir: String): Rope
  def tail(): Coordinate
}

class Part1Rope(val head: Coordinate, val tail: Coordinate) extends Rope {
  def this() = this(Coordinate.Origin, Coordinate.Origin)

  override def move(dir: String): Rope = {
    val newHead = head.move(dir)
    val newTail = tail.trailTo(newHead)
    new Part1Rope(newHead, newTail)
  }

  override def toString: String = s"$head, $tail"
}

class Part2Rope(val knots: List[Coordinate]) extends Rope {
  def this() = this(List.fill(10)(Coordinate.Origin))

  override def tail(): Coordinate = knots.last

  def move(dir: String): Rope = {
    val newKnots = new ListBuffer[Coordinate]
    var current = knots.head.move(dir)
    newKnots += current
    for (i <- 1 until knots.size) {
      val next = knots(i).trailTo(current)
      current = next
      newKnots += current
    }
    new Part2Rope(newKnots.toList)
  }
}

class Day09 {
  val testInput = Seq("R 4", "U 4", "L 3", "D 1", "R 4", "D 1", "L 5", "R 2")
  val testInput2 = Seq("R 5", "U 8", "L 8", "D 3", "R 17", "D 10", "L 25", "U 20")
  val realInput = FileReader.read(9)
  val moves = realInput.map(_.split(' ')).map(a => (a(0), a(1).toInt))

  def moveRope(rope: Rope): Int = {
    val tailPositions = new ListBuffer[Coordinate]
    tailPositions += new Coordinate(0, 0)

    var r: Rope = rope
    for ((dir, count) <- moves) {
      for (_ <- 1 to count) {
        r = r.move(dir)
        tailPositions += r.tail()
      }
    }
    tailPositions.toList.map(_.asTuple()).distinct.size
  }

  def part1(): Int = moveRope(new Part1Rope())
  def part2(): Int = moveRope(new Part2Rope())
}
