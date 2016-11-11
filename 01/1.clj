(defn toInt [c]
  (if (= c \() 1 -1))

; 1a
(->> (slurp "input.txt")
     (map toInt)
     (reduce +)
     println)

; 1b
(defn stop-at-basement [acc index cur]
  (if (neg? acc)
    (reduced index)
    (+ acc (toInt cur))))

(->> (slurp "input.txt")
     vec
     (reduce-kv stop-at-basement 0)
     println)
