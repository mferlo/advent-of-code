import scala.collection.mutable.ListBuffer

class Day01() {
  private val data : Seq[Int] = parse(FileReader.readAsOptionInts(1)).map(l => l.sum).sorted.reverse

  private def parse(input: Seq[Option[Int]]) : Seq[Seq[Int]] = {
    val result = new ListBuffer[Seq[Int]]
    val current = new ListBuffer[Int]
    for (n <- input) {
      if (n.isEmpty) {
        result += current.toSeq
        current.clear()
      } else {
        current += n.get
      }
    }
    result.toSeq
  }

  def part1 : Int = data(0)
  def part2 : Int = data.take(3).sum
}