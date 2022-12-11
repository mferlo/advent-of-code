import scala.io.Source

object FileReader {
  private def fileName(day: Int) = f"c:/Users/Matt/Desktop/dev/advent-code/advent-code-2023/src/main/scala/$day%02d.txt"
  def read(day: Int): Seq[String] = Source.fromFile(fileName(day)).getLines().toSeq
  def readAsOptionInts(day: Int): Seq[Option[Int]] = read(day).map(_.toIntOption)
  def readAsInts(day: Int): Seq[Int] = read(day).map(_.toInt)
}