(require '[clojure.string :as str])

(defn parse [line]
  (let [tokens (str/split line #" " 3)
        connections (str/split (last tokens) #", ")]
    [(first tokens) connections]))

(def graph (into {} (map parse (str/split-lines (slurp "input")))))

(defn filter-already-seen [seen s] (remove (partial contains? seen) s))

(defn find-all-connected [to-examine seen graph]
  (if-let [current (first to-examine)]
    (let [unseen-neighbors (filter-already-seen seen (graph current))]
      (recur (concat (rest to-examine) unseen-neighbors) (conj seen current) graph))
    seen))

(defn count-groups [num-groups seen graph]
  (if-let [unvisited (not-empty (filter-already-seen seen (keys graph)))]
    (let [visited (find-all-connected (list (first unvisited)) seen graph)]
      (recur (inc num-groups) (into seen visited) graph))
    num-groups))

(println "Part 1: " (count (find-all-connected '("0") #{} graph)))
(println "Part 2: " (count-groups 0 #{} graph))
