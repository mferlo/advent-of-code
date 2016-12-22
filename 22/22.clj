(def r #".*-x(\d+)-y(\d+)\s+\d+T\s+(\d+)T\s+(\d+)T.*")

(defn parse-line [line]
  (let [[_ x y used avail] (re-matches r line)]
    { :x (Long/parseLong x)
      :y (Long/parseLong y)
      :used (Long/parseLong used)
      :avail (Long/parseLong avail) }))

(defn parse [filename]
  (->> (slurp filename)
       clojure.string/split-lines
       rest ; df command
       rest ; df header
       (map parse-line)))

(defn viable-pair? [a b]
  (and (pos? (:used a))
       (and (not= (:x a) (:x b)) (not= (:y a) (:y b)))
       (<= (:used a) (:avail b))))

(defn viable-pairs [nodes node]
  (filter (partial viable-pair? node) nodes))

(defn count-viable-pairs [nodes]
  (count (map (partial viable-pairs nodes) nodes)))

(let [nodes (vec (parse "input.txt"))]
  (println (first nodes) (last nodes))
  (println (count-viable-pairs nodes)))
