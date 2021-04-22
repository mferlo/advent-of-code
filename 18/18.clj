(def input ".^^..^...^..^^.^^^.^^^.^^^^^^.^.^^^^.^^.^^^^^^.^...^......^...^^^..^^^.....^^^^^^^^^....^^...^^^^..^")
(def height 400000) ; 40)
(def width (count input))

(defn make-next-tile [row i]
  (let [l (nth row (dec i) nil)
        r (nth row (inc i) nil)]
    (if l (not r) r))) ; (xor l r)

(defn make-next-row [row]
  (map (partial make-next-tile row) (range 0 width)))

; (defn as-str [row]
;  (apply str (map #(if % \^ \.) row)))

(loop [i 0
       safe-tiles 0
       row (map #(= \^ %) input)]
  (if (< i height)
    (recur (inc i)
           (+ safe-tiles (count (filter not row)))
           (vec (make-next-row row))) ; vec for perf; we nth it 2*width times
    (println safe-tiles "are safe")))
