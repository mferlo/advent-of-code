(->> (slurp "input.txt")
     clojure.string/split-lines
     (apply map str)
     (map (comp first first (partial sort-by val >) frequencies))
     (apply str)
     println)
