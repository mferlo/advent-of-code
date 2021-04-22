#!python

part1 = 0
part2 = 0

for line in open('input').read().splitlines():
    words = set()
    sortedWords = set()
    dup1 = False
    dup2 = False

    for word in line.split():
        if word in words:
            dup1 = True
        words.add(word)

        sortedWord = ''.join(sorted(word))
        if sortedWord in sortedWords:
            dup2 = True
        sortedWords.add(sortedWord)

    if not dup1:
        part1 += 1
    if not dup2:
        part2 += 1

print part1
print part2
                
