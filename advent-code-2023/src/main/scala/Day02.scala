trait Outcome
object Win extends Outcome
object Draw extends Outcome
object Lose extends Outcome

trait Shape {
  def score: Int
  def scoreVersus(shape: Shape): Int
  def forOutcome(outcome: Outcome): Shape
}

object Rock extends Shape {
  override def score: Int = 1
  override def scoreVersus(shape: Shape): Int =
    shape match {
      case Rock => 3
      case Paper => 0
      case Scissors => 6
    }

  override def forOutcome(outcome: Outcome): Shape =
    outcome match {
      case Win => Paper
      case Draw => Rock
      case Lose => Scissors
    }
}

object Paper extends Shape {
  override def score: Int = 2
  override def scoreVersus(shape: Shape): Int =
    shape match {
      case Rock => 6
      case Paper => 3
      case Scissors => 0
    }

  override def forOutcome(outcome: Outcome): Shape =
    outcome match {
      case Win => Scissors
      case Draw => Paper
      case Lose => Rock
    }
}

object Scissors extends Shape {
  override def score: Int = 3
  override def scoreVersus(shape: Shape): Int =
    shape match {
      case Rock => 0
      case Paper => 6
      case Scissors => 3
    }

  override def forOutcome(outcome: Outcome): Shape =
    outcome match {
      case Win => Rock
      case Draw => Scissors
      case Lose => Paper
    }
}

class Day02() {
  private val data : Seq[(String, String)] = FileReader.read(2).map(parseLine)
  private val dataPart1 : Seq[(Shape, Shape)] = data.map(parseLine1)
  private val dataPart2 : Seq[(Shape, Outcome)] = data.map(parseLine2)

  private def toShape : Map[String, Shape] = Map(
    "A" -> Rock, "B" -> Paper, "C" -> Scissors,
    "X" -> Rock, "Y" -> Paper, "Z" -> Scissors
  )

  private def toOutcome : Map[String, Outcome] = Map("X" -> Lose, "Y" -> Draw, "Z" -> Win)

  private def parseLine(line: String): (String, String) = {
    val a = line.split(" ", 2)
    (a(0), a(1))
  }

  private def parseLine1(line: (String, String)): (Shape, Shape) =
    (toShape(line._1), toShape(line._2))

  private def parseLine2(line: (String, String)): (Shape, Outcome) =
    (toShape(line._1), toOutcome(line._2))

  private def scoreRound(round: (Shape, Shape)): Int = round._2.score + round._2.scoreVersus(round._1)
  private def scoreGames(rounds: Seq[(Shape, Shape)]): Int = rounds.map(scoreRound).sum

  def part1(): Int = scoreGames(dataPart1)
  def part2(): Int = scoreGames(dataPart2.map(x => (x._1, x._1.forOutcome(x._2))))
}