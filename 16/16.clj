(defn spin [x s] (vec (concat (take-last x s) (take (- (count s) x) s))))
(defn exchange [a b s] (assoc s a (s b) b (s a)))
(defn partner [a b s] (exchange (.indexOf s a) (.indexOf s b) s))

(defn parse [step]
  (let [op (first step)
        [a1 a2] (clojure.string/split (apply str (rest step)) #"/")]
    (condp = op
      \p (partial partner (first a1) (first a2))
      \x (partial exchange (Integer/parseInt a1) (Integer/parseInt a2))
      \s (partial spin (Integer/parseInt a1)))))

(def init "abcdefghijklmnop")
(def instructions (map parse (clojure.string/split (slurp "input") #",")))

(defn dance
  ([s] (apply str (reduce #(%2 %1) (vec s) instructions)))
  ([s n] (if (zero? n) s (recur (dance s) (dec n)))))

(println "Part 1: " (dance init))

(defn cycle-length [s seen step]
  (if (contains? seen s)
    step
    (recur (dance s) (assoc seen s step) (inc step))))

(let [cycle (cycle-length init {} 0)
      steps-to-do (mod 1000000000 cycle)]
  (println "Part 2: " (dance init steps-to-do)))
