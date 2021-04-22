(defn in-range? [upper-left cur lower-right]
  (and (<= (:x upper-left) (:x cur) (:x lower-right))
       (<= (:y upper-left) (:y cur) (:y lower-right))))

(defn as-2d
  ([i] {:x (quot i 1000) :y (rem i 1000)})
  ([x y] {:x (Integer/parseInt x) :y (Integer/parseInt y)}))
  
(defn get-func-for-line-a [s]
  (let [[_ cmd x1 y1 x2 y2] (re-find #"^(.*) (\d+),(\d+) through (\d+),(\d+)$" (str s))]
    (fn[index state]
      (if (in-range? (as-2d x1 y1) (as-2d index) (as-2d x2 y2)) 
        (case cmd
          "toggle" (if (= 0 state) 1 0)
          "turn on" 1
          "turn off" 0)
        state))))

(defn get-func-for-line-b [s]
  (let [[_ cmd x1 y1 x2 y2] (re-find #"^(.*) (\d+),(\d+) through (\d+),(\d+)$" (str s))]
    (fn[index state]
      (if (in-range? (as-2d x1 y1) (as-2d index) (as-2d x2 y2)) 
        (case cmd
          "toggle" (+ state 2)
          "turn on" (inc state)
          "turn off" (max 0 (dec state)))
        state))))

(defn make-next-lights [state f]
  (map-indexed f state))

; Correct, but naive (inefficient)
(->> (slurp "input.txt")
     clojure.string/split-lines
     (map get-func-for-line-b)
     (reduce make-next-lights (repeat 1000000 0))
     (reduce +)
     println)
