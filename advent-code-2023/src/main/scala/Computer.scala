trait Instruction {
  def cycleTime: Int = 1
}

object Noop extends Instruction

case class AddX(val x: Int) extends Instruction {
  override def cycleTime: Int = 2
}

object InstructionParser {
  def read(day: Int): Vector[Instruction] = parse(FileReader.read(day))
  def parse(input: Seq[String]): Vector[Instruction] = input.map(parse).toVector
  def parse(input: String): Instruction =
    input match {
      case "noop" => Noop
      case _ => AddX(input.split(' ')(1).toInt)
    }
}

class Computer(val program: Vector[Instruction],
               val register: Int,
               val instructionPointer: Int,
               val registerHistory: List[Int]) {

  def this(program: Seq[Instruction]) = this(program.toVector, 1, 0, List(0))

  def done(): Boolean = instructionPointer >= program.size

  def execute(): Computer = {
    val (newR, newRH) = program(instructionPointer) match {
      case Noop => (register, registerHistory.appended(register))
      case AddX(x) => (register + x, registerHistory.appendedAll(Seq(register, register)))
      }
    new Computer(program, newR, instructionPointer + 1, newRH)
  }
}
