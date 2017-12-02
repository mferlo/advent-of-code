(use '[clojure.string :only (split split-lines)])

(defn get-lines []
  (split-lines (slurp "input")))

(defn parse-line [line]
  (map #(Integer/parseInt %) (split line #"\s+")))

(defn max-min-diff [numbers]
  (- (apply max numbers) (apply min numbers)))

(->> (get-lines)
     (map parse-line)
     (map max-min-diff)
     (reduce +)
     println)
