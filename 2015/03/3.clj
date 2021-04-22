(defn deliver-presents [acc cur]
  (case cur
    \v {:x (:x acc) :y (dec (:y acc))}
    \^ {:x (:x acc) :y (inc (:y acc))}
    \< {:x (dec (:x acc)) :y (:y acc)}
    \> {:x (inc (:x acc)) :y (:y acc)}))

(defn get-houses [input]
  (reductions deliver-presents {:x 0 :y 0} input))

(defn get-house-count [input]
  (->> (get-houses input)
       distinct
       count))

; 3a
(->> (slurp "input.txt")
     get-house-count
     println)

;3b
(let [input (slurp "input.txt")]
  (->> (mapcat get-houses (list (take-nth 2 input) (take-nth 2 (rest input))))
       distinct
       count
       println))
