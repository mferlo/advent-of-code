(defn dial [cur dir]
  ; FIXME logic here
  5)

(defn decode [s]
  (->> (seq s)
       (reduce dial 5)))

(->> "ULL\nRRDDD\nLURDL\nUUUUD" ; Test input, answer 1985
     clojure.string/split-lines
     (map decode)
     clojure.string/join
     println)
