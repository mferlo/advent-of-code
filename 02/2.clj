(use '[clojure.string :only (split split-lines)])

(defn get-lines [] (split-lines (slurp "input")))
(defn parse-line [line] (map #(Integer/parseInt %) (split line #"\s+")))

; part 1
(defn max-min-diff [numbers] (- (apply max numbers) (apply min numbers)))

(->> (get-lines)
     (map parse-line)
     (map max-min-diff)
     (reduce +)
     println)

; part 2
(defn divisor-value [x y]
  (if (and (not= x y) (zero? (mod x y)))
    (/ x y)
    0))

(defn divisor-values [xs x]
  (reduce + (map (partial divisor-value x) xs)))

(defn divisor-values-line [xs]
  (reduce + (map (partial divisor-values xs) xs)))

; part 2
(->> (get-lines)
     (map parse-line)
     (map divisor-values-line)
     (reduce +)
     println)

