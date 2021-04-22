(require '[clojure.string :as str])

; parts 1 & 2
(defn possible-triangle [v]
  (let [[x y z] (sort v)]
    (> (+ x y) z)))

(defn to-int [s] (Integer/parseInt s))

(defn parse [s]
  (->> (slurp s)
       str/split-lines
       (map str/trim)
       (map #(str/split % #"\s+"))
       (map #(map to-int %))))

; part 1
(->> (parse "input.txt")
     (filter possible-triangle)
     count
     println)

; part 2
(defn rotate-3x3-squares [three-rows]
  (let [[[x1 x2 x3] [y1 y2 y3] [z1 z2 z3]] three-rows]
    (list (list x1 y1 z1)
          (list x2 y2 z2)
          (list x3 y3 z3))))

(defn take-3-and-rotate [acc cur]
  (if (empty? cur)
    acc
    (take-3-and-rotate (concat acc (rotate-3x3-squares (take 3 cur)))
                       (nthrest cur 3))))

(->> (parse "input.txt")
     (take-3-and-rotate ())
     (filter possible-triangle)
     count
     println)
