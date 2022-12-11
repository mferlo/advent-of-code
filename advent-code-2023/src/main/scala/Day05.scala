import scala.collection.mutable.ListBuffer

class Crane(var stacks: List[List[Char]], val moveOneAtATime: Boolean) {
  def doMoves(moves: Seq[String]): String = {
    moves.foreach(move => doMove(move))
    topCrates()
  }

  private def doMove(move: String): Unit = {
    val Array(_, count, _, from, _, to) = move.split(' ').map(_.toIntOption)
    val (removed, remainder) = stacks(from.get).splitAt(count.get)
    stacks = stacks.updated(from.get, remainder)
    val newStack: List[Char] = if (moveOneAtATime) removed.reverse else removed
    stacks = stacks.updated(to.get, stacks(to.get).prependedAll(newStack))
  }

  private def topCrates(): String = stacks.drop(1).map(_.head).mkString
}

object Crane {
  private def maybeGet(i: Int, line: String): Option[Char] = {
    val ch = line.charAt(4*i - 3)
    if (ch == ' ') None else Some(ch)
  }

  def parse(input: Seq[String], moveOneAtATime: Boolean): Crane = {
    val stacks = List.fill(10)(new ListBuffer[Char]())
    for (line <- input) {
      for (i <- 1 to 9) {
        maybeGet(i, line).foreach(ch => stacks(i).append(ch))
      }
    }
    new Crane(stacks.map(lb => lb.toList), moveOneAtATime)
  }
}

class Day05 {
  val input = FileReader.read(5)
  val crane1 = Crane.parse(input.take(8), moveOneAtATime = true)
  val crane2 = Crane.parse(input.take(8), moveOneAtATime = false)
  val moves = input.drop(10)

  def part1(): String = crane1.doMoves(moves)
  def part2(): String = crane2.doMoves(moves)
}
