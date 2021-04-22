(defn parse [regex-match]
  (let [[_ direction steps] regex-match]
    { :turn (if (= direction "L") :left :right) :steps (Integer. steps) } ))

(defn turn [direction left-or-right]
  (case direction
    :north (if (= left-or-right :left) :west :east)
    :south (if (= left-or-right :left) :east :west)
    :east (if (= left-or-right :left) :north :south)
    :west (if (= left-or-right :left) :south :north)))

(defn move [cur direction steps]
  (case direction
    :north { :x (:x cur) :y (+ (:y cur) steps) :direction direction }
    :south { :x (:x cur) :y (- (:y cur) steps) :direction direction }
    :east { :x (+ (:x cur) steps) :y (:y cur) :direction direction }
    :west { :x (- (:x cur) steps) :y (:y cur) :direction direction }))

(defn walk [pos turn-and-step]
  (let [new-dir (turn (:direction pos) (:turn turn-and-step))]
    (move pos new-dir (:steps turn-and-step))))

; 1a
(->> (slurp "input.txt")
     (re-seq #"([LR])(\d+)")
     (map parse)
     (reduce walk { :x 0 :y 0 :direction :north })
     println) ; Answer is x + y

; 1b helpers

(defn unroll [turn-and-step]
  (cons (:turn turn-and-step)
        (repeat (:steps turn-and-step) :forward)))

(defn walk-step-by-step [cur action]
  (if (= action :forward)
    (let [new-position (move cur (:direction cur) 1)]
      (assoc new-position :move true))
    (let [new-dir (turn (:direction cur) action)]
      (assoc cur :direction new-dir :move false))))

(defn stop-on-first-duplicate [places-visited new-place]
  (if (contains? places-visited new-place)
    (reduced new-place)
    (conj places-visited new-place)))

; 1b
(->> (slurp "input.txt")
     (re-seq #"([LR])(\d+)")
     (map parse)
     (map unroll)
     flatten
     (reductions walk-step-by-step { :x 0 :y 0 :direction :north })
     (filter #(:move %))
     (map #(dissoc % :direction :move))
     (reduce stop-on-first-duplicate #{})
     println) ; Answer is x + y
