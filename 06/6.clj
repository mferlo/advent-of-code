(->> (slurp "input.txt")
     clojure.string/split-lines
     (apply map str)
     (map frequencies)
     (map #(sort-by val > %)) ; part 1
     ; (map #(sort-by val < %)) ; part 2
     (map first)
     (map first)
     (apply str)
     println)
