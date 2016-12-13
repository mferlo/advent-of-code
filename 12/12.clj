(defn parse-line [line]
  (let [[instr arg1 arg2] (clojure.string/split line #" ")]
    (case instr
      "inc" { :instr :inc :register arg1 }
      "dec" { :instr :dec :register arg1 }
      "jnz" { :instr :jnz :register arg1 :jump (Long/parseLong arg2) }
      "cpy" (if (re-matches #"\d+" arg1)
              { :instr :cpv :register arg2 :value (Long/parseLong arg1) }
              { :instr :cpr :register arg2 :from arg1 }))))

(defn parse-code [filename]
  (->> (slurp filename)
       clojure.string/split-lines
       (map parse-line)
       vec))

(defn process-instruction [state instr]
  (let [register (:register instr)
        advance-pointer (assoc state :pointer (inc (:pointer state)))]
    (case (:instr instr)
      :inc (assoc advance-pointer register (inc (state register)))
      :dec (assoc advance-pointer register (dec (state register)))
      :cpv (assoc advance-pointer register (:value instr))
      :cpr (assoc advance-pointer register (state (:from instr)))
      :jnz (if (= 0 (state register))
             advance-pointer
             (assoc state :pointer (+ (:pointer state) (:jump instr)))))))

(let [instructions (parse-code "input.txt")]
  (loop [state { :pointer 0 "a" 0 "b" 0 "c" 1 "d" 0 }]
    (if-let [instr (get instructions (:pointer state))]
      (recur (process-instruction state instr))
      (println state))))
