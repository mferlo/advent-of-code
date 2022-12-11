class Day10 {
  val testProgram = 
  val program = InstructionParser.read(10)

  val computer = new Computer(program)

  var c = computer
  while (!c.done()) {
    c = c.execute()
  }
  val rh = c.registerHistory

  def part1(): Int = Range.inclusive(20, 220, 20).map(i => i * rh(i)).sum
  def part2(): Int = 0

}
