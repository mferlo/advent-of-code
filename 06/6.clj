(defn in-range? [upper-left cur lower-right]
  (and (<= (:x upper-left) (:x cur) (:x lower-right))
       (<= (:y upper-left) (:y cur) (:y lower-right))))

(defn to-2d [i]
  {:x (quot i 1000) :y (rem i 1000)})

(defn from-2d [x y]
  {:x x :y y})
  
(defn get-func [s]
  (case s
    "toggle" #(if (= % 0) 1 0)
    "turn on" 1
    "turn off" 0))

(defn parse-line [s]
  (let [regex (re-find #"^(.*) (\d+),(\d+) through (\d+),(\d+)" (str s))]
    {:func (get-func regex[1])
      :upper-left (from-2d regex[2] regex[3])
      :lower-right (from-2d regex[4] regex[5])}))
  
(defn make-next-lights [state index input]
  (if (in-range? (:upper-left input) (to-2d index) (:lower-right input))
    ((:func input) state)
    (state)))

(with-open [rdr (clojure.java.io/reader "input.txt")]
  (->> (line-seq rdr)
       parse-line
;       count
       println))

       ;; 
       ;; (reduce-kv make-next-lights (repeat 1000000 0))
       ;; (reduce +)
       ;; println))
