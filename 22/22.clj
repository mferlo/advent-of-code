(def r #"^/dev/grid/node-x(\d+)-y(\d+)\s+\d+T\s+(\d+)T\s+(\d+)T\s+\d+%$")

(defn parse-line [line]
  (let [[x y used avail] (map #(Long/parseLong %) (rest (re-matches r line)))]
    { :x x :y y :used used :avail avail }))

(defn parse [filename]
  (->> (slurp filename)
       clojure.string/split-lines
       rest ; df command
       rest ; df header
       (map parse-line)))

(defn has-data? [a] (pos? (:used a)))
(defn not-same? [a b] (not (and (= (:x a) (:x b)) (= (:y a) (:y b)))))
(defn has-space? [a b] (<= (:used a) (:avail b)))

(defn viable-pair? [a b] (and (has-data? a) (not-same? a b) (has-space? a b)))

(defn viable-pairs [nodes node]
  (filter (partial viable-pair? node) nodes))

(let [nodes (vec (parse "input.txt"))]
  (->> (map (partial viable-pairs nodes) nodes)
       flatten
       count
       println))
