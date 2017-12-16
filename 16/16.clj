(defn spin [s x]
  (vec (concat (take-last x s) (take (- (count s) x) s))))

(defn exchange [s a b]
  (assoc s a (s b) b (s a)))

(defn partner [s a b]
  (let [aa (.indexOf s a)
        bb (.indexOf s b)]
    (assoc s aa (s bb) bb (s aa))))

(def init (vec "abcdefghijklmnop"))

(defn parse [step]
  (let [op (first step)
        args (apply str (rest step))
        [a1 a2] (clojure.string/split args #"/")]
    (condp = op
      \p { :op :p :a1 (first a1) :a2 (first a2) }
      \x { :op :x :a1 (Integer/parseInt a1) :a2 (Integer/parseInt a2) }
      \s { :op :s :a1 (Integer/parseInt a1) })))

(def instructions (map parse (clojure.string/split (slurp "input") #",")))

(defn exec-step [s step]
  (condp = (:op step)
    :p (partner s (:a1 step) (:a2 step))
    :x (exchange s (:a1 step) (:a2 step))
    :s (spin s (:a1 step))))

(defn exec [s] (reduce exec-step s instructions))

(println "Part 1: " (apply str (exec init)))
