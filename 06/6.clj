(->> (slurp "input.txt")
     clojure.string/split-lines
     (apply map str)
     (map #(->> (frequencies %) (sort-by val >) first first))
     (apply str)
     println)
