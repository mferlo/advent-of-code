(require '[clojure.string :as str])

(defn parse [line] (vec (map #(Integer/parseInt %) (str/split line #": "))))
(def scanners (into {} (map parse (str/split-lines (slurp "input")))))
(def t-max (inc (apply max (keys scanners)))) ; "inc" because range is exclusive

(defn scan-pos [t scan-range]
  (let [period (* 2 (dec scan-range))
        t-mod (mod t period)]
    (if (< t-mod scan-range) t-mod (- period t-mod))))

(defn caught? [t scan-range] (zero? (scan-pos t scan-range)))

(defn calc-severity [t]
  (if-let [scan-range (scanners t)]
    (if (caught? t scan-range) (* t scan-range) 0)
    0))

(println "Part 1:" (reduce + (map calc-severity (range 0 t-max))))

(defn caught-with-delay? [initial-delay t]
  (if-let [scan-range (scanners t)]
    (caught? (+ initial-delay t) scan-range)))

(defn successful-with-delay? [delay]
  (not-any? (partial caught-with-delay? delay) (range 0 t-max)))

(println "Part 2:" (first (filter successful-with-delay? (range))))
