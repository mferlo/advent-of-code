class Day06 {
  val input: String = FileReader.read(6).head

  private def allUniqueCharacters(s: String): Boolean = s.toSet.size == s.length

  private def findFirstUniqueN(n: Int, s: String): Int =
    Range.inclusive(n, s.length).find(i => allUniqueCharacters(s.substring(i-n, i))).get

  def part1(): Int = findFirstUniqueN(4, input)
  def part2(): Int = findFirstUniqueN(14, input)
}
