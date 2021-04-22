(def regex
  #"^Disc #(\d+) has (\d+) positions; at time=0, it is at position (\d+)\.$")

(def disc0 { :fake-disc "Just filling up [0]" })
(def part2-disc { :disc 7 :size 11 :start 0 })

(defn parse-line [result line]
  (let [[disc size start] (map #(Long/parseLong %) (rest (re-find regex line)))]
    (conj result { :disc disc :size size :start start })))

(defn disc-vec-from-file [filename]
  (->> (slurp filename)
       clojure.string/split-lines
       (reduce parse-line [ disc0 ])))

(defn disc-open-at-time? [disc time]
  (= 0 
     (mod (+ time (:start disc))
          (:size disc))))

(defn good-time-to-drop? [discs time]
  (loop [i 1] ; No Disc #0
    (if-let [disc (get discs i)]
      (if (disc-open-at-time? disc (+ time i))
        (recur (inc i)))
      true))) ; We made it through all the discs.

(defn time-to-drop [discs]
  (loop [time 0]
    (if (good-time-to-drop? discs time)
      time
      (recur (inc time)))))

(let [discs (disc-vec-from-file "input.txt")]
  (println (time-to-drop discs))
  (println (time-to-drop (conj discs part2-disc))))
