class Elf(val x: Int, val y: Int) {
  def fullyOverlaps(e: Elf): Boolean =
    (x >= e.x && y <= e.y) || (e.x >= x && e.y <= y)

  def overlaps(e: Elf): Boolean = !values().intersect(e.values()).isEmpty

  private def values(): Set[Int] = (x to y).toSet
}

object Elf {
  def parse(s: String): Elf = {
    val x = s.split('-')
    new Elf(x(0).toInt, x(1).toInt)
  }
}

class Day04 {
  private val elfPairs: Seq[(Elf, Elf)] = FileReader.read(4).map(line => line.split(',')).map(a => (Elf.parse(a(0)), Elf.parse(a(1))))

  def part1(): Int = elfPairs.count(ep => ep._1.fullyOverlaps(ep._2))
  def part2(): Int = elfPairs.count(ep => ep._1.overlaps(ep._2))
}
