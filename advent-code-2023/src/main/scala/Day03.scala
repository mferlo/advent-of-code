class Day03 {
  private val input: Seq[String] = FileReader.read(3)
  private val halves: Seq[(String, String)] = input.map(l => (l.substring(0, l.length / 2), l.substring(l.length / 2)))

  private def commonItem(s: Seq[String]): Char = s(0).toSet.intersect(s(1).toSet).intersect(s(2).toSet).head
  private def commonItem(s: (String, String)): Char = s._1.toSet.intersect(s._2.toSet).head

  private def priority(ch: Char): Int = if (ch.isUpper) 27 + ch - 'A' else 1 + ch - 'a'

  def part1(): Int = halves.map(commonItem).map(priority).sum
  def part2(): Int = input.grouped(3).map(commonItem).map(priority).sum
}
