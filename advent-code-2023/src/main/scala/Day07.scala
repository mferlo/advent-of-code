import scala.collection.mutable.ListBuffer

trait Node {
  def name: String
  def size(): Int
  def asString(depth: Int): String
  def getChildren(): Seq[Node]
}

class File(val name: String, val fileSize: Int) extends Node {
  override def size(): Int = fileSize

  override def asString(depth: Int): String =
    (" " * 2 * depth) + s"- $name (file, size=$fileSize)"

  override def getChildren(): Seq[Node] = Seq.empty[Node]
}

class Directory(val name: String, val parent: Directory) extends Node {
  override def asString(depth: Int): String =
    (" " * 2 * depth) + s"- $name (dir)"

  override def getChildren(): Seq[Node] = children.keys.toSeq.sorted.map(c => children(c))

  private var children: Map[String, Node] = Map.empty[String, Node]

  def addChild(child: Node): Unit = children = children + (child.name -> child)

  def getChild(childName: String): Node = children(childName)

  def getChildDirectory(childDirectoryName: String): Directory = {
    val child = getChild(childDirectoryName)
    child match {
      case d: Directory => d
      case f: File => throw new Exception(s"$childDirectoryName is a file")
    }
  }

  override def size(): Int = children.values.map(_.size()).sum
}

class Day07 {
  val realInput = FileReader.read(7)
  val testInput = Seq("$ cd /", "$ ls", "dir a", "14848514 b.txt", "8504156 c.dat", "dir d", "$ cd a", "$ ls", "dir e", "29116 f", "2557 g", "62596 h.lst", "$ cd e", "$ ls", "584 i", "$ cd ..", "$ cd ..", "$ cd d", "$ ls", "4060174 j", "8033020 d.log", "5626152 d.ext", "7214296 k")

  val root = traverseFilesystem(realInput)
  val directories = allDirectories(root)
  val directorySizes = directories.map(_.size())

  def traverseFilesystem(input: Seq[String]): Directory = {
    val lines = input.toVector

    // Create root directory and skip first line of input
    if (input.head != "$ cd /") throw new Exception("Unexpected first input")
    val root = new Directory("/", null)
    var i = 1
    var path = List("/")

    var currentDirectory = root

    while (i < lines.size) {
      val line = lines(i)
      i += 1
      if (line.startsWith("$ cd")) {
        val relativePath = line.substring("$ cd ".length)
        if (relativePath == "..") {
          path = path.dropRight(1)
          currentDirectory = currentDirectory.parent
        } else {
          path = path.appended(relativePath)
          currentDirectory = currentDirectory.getChildDirectory(relativePath)
        }
      } else if (line == "$ ls") {
        while (i < lines.size && !lines(i).startsWith("$")) {
          val Array(p1, p2) = lines(i).split(' ')
          i += 1
          val newChild =
            if (p1 == "dir")
              new Directory(p2, currentDirectory)
            else
              new File(p2, p1.toInt)
          currentDirectory.addChild(newChild)
        }
      } else {
        throw new Exception("Unexpected input: " + line)
      }
    }

    root
  }

  def recursivePrint(current: Node, depth: Int): Unit = {
    println(current.asString(depth))
    for (child <- current.getChildren()) {
      recursivePrint(child, depth + 1)
    }
  }

  def allDirectories(current: Directory, found: ListBuffer[Directory]): Unit = {
    current.getChildren().foreach(c => {
      c match {
        case _: File =>
        case d: Directory => {
          found += d
          allDirectories(d, found)
        }
      }
    }
    )
  }

  def allDirectories(root: Directory): List[Directory] = {
    val temp = new ListBuffer[Directory]
    allDirectories(root, temp)
    temp.toList
  }

  // find all of the directories with a total size of at most 100000, then calculate the sum of their total sizes.
  def part1(): Int = {
    directorySizes.filter(_ <= 100000).sum
  }

  def part2(): Int = {
    val availableSpace = 70000000 - root.size()
    val minimumSizeToDelete = 30000000 - availableSpace
    directorySizes.sorted.find(_ >= minimumSizeToDelete).get
  }
}
