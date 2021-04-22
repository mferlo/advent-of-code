(defn get-program [] 
  (vec (map #(Integer/parseInt %) (clojure.string/split-lines (slurp "input")))))

(defn done? [program inst]
  (or (neg? inst) (>= inst (count program))))

(defn run-program [program inst steps]
  (if (done? program inst)
    steps
    (recur (update program inst inc)
           (+ inst (program inst))
           (inc steps))))

(defn run-program-2 [program inst steps]
  (if (done? program inst)
    steps
    (recur (update program inst (if (>= (program inst) 3) dec inc))
           (+ inst (program inst))
           (inc steps))))

; part 1
(println (run-program (get-program) 0 0))
(println (run-program-2 (get-program) 0 0))
