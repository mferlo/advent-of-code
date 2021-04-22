(use '[clojure.string :only (split)])

; 2a
(defn calc-paper [sorted-dimensions]
  (let [[x y z] sorted-dimensions]
    (+ (* 3 x y) (* 2 y z) (* 2 x z))))

; 2b
(defn calc-ribbon [sorted-dimensions]
  (let [[x y z] sorted-dimensions]
    (+ x x y y (* x y z))))

(defn process-line [line calc]
  (->> (split line #"x")
       (map #(Integer/parseInt %))
       sort
       calc))

(with-open [rdr (clojure.java.io/reader "input.txt")]
  (->> (line-seq rdr)
       (map #(process-line % calc-paper))
       (reduce +)
       println))

(with-open [rdr (clojure.java.io/reader "input.txt")]
  (->> (line-seq rdr)
       (map #(process-line % calc-ribbon))
       (reduce +)
       println))
