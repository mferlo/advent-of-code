(def keypad-1
  {1 { \L 1 \U 1 \R 2 \D 4 }
   2 { \L 1 \U 2 \R 3 \D 5 }
   3 { \L 2 \U 3 \R 3 \D 6 }
   4 { \L 4 \U 1 \R 5 \D 7 }
   5 { \L 4 \U 2 \R 6 \D 8 }
   6 { \L 5 \U 3 \R 6 \D 9 }
   7 { \L 7 \U 4 \R 8 \D 7 }
   8 { \L 7 \U 5 \R 9 \D 8 }
   9 { \L 8 \U 6 \R 9 \D 9 } })

(defn move-keypad-1 [digit direction]
  ((keypad-1 digit) direction))

(defn dial-1 [prev s]
  "Given the result of dialing the previous line
  and the string representing the current line,
  dials the current line, returning the number."
  (reduce move-keypad-1 prev (seq s)))

; Test input: "ULL\nRRDDD\nLURDL\nUUUUD" => 1985
(->> (slurp "input.txt")
     clojure.string/split-lines
     (reductions dial-1 5) ; starting at 5, get & store the value of each line
     rest                  ; drop spurious leading 5
     clojure.string/join
     println)

(def keypad-2-non-edges
  "The state transitions that don't go off the edge.
  Note that everything is chars, unlike keypad-1.
      1
    2 3 4
  5 6 7 8 9
    A B C
      D"
  { \1 { \D \3 }
    \2 { \R \3 \D \6 }
    \3 { \L \2 \U \1 \R \4 \D \7 }
    \4 { \L \3 \D \8 }
    \5 { \R \6 }
    \6 { \L \5 \U \2 \R \7 \D \A }
    \7 { \L \6 \U \3 \R \8 \D \B }
    \8 { \L \7 \U \4 \R \9 \D \C }
    \9 { \L \8 }
    \A { \U \6 \R \B }
    \B { \L \A \U \7 \R \C \D \D }
    \C { \L \B \U \8 }
    \D { \U \B } })

(defn move-keypad-2 [digit direction]
  "If we find an edge, follow it, otherwise return input"
  (let [digit-edges (keypad-2-non-edges digit)]
    (or (digit-edges direction) digit)))

(defn dial-2 [prev s]
  (reduce move-keypad-2 prev (seq s)))

; Test input: "ULL\nRRDDD\nLURDL\nUUUUD" => 5DB3 (2)
(->> (slurp "input.txt")
     clojure.string/split-lines
     (reductions dial-2 \5)
     rest
     clojure.string/join
     println)
