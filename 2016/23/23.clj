(defn maybe-to-long [s]
  (if (re-matches #"-?\d+" s) (Long/parseLong s) s))

(defn parse-line [line]
  (let [[instr arg1 arg2] (clojure.string/split line #" ")]
    (case instr
      "inc" { :instr :inc :arg1 arg1 }
      "dec" { :instr :dec :arg1 arg1 }
      "tgl" { :instr :tgl :arg1 arg1 }
      "jnz" { :instr :jnz :arg1 (maybe-to-long arg1) :arg2 (maybe-to-long arg2) }
      "cpy" { :instr :cpy :arg1 (maybe-to-long arg1) :arg2 arg2 })))

(defn toggle-instruction [i toggle-index] ; for debugging
  (case (:instr i)
    :inc (assoc i :instr :dec)
    :dec (assoc i :instr :inc)
    :tgl (assoc i :instr :inc)
    :jnz (assoc i :instr :cpy)
    :cpy (assoc i :instr :jnz)))

(defn parse-code [filename]
  (->> (slurp filename)
       clojure.string/split-lines
       (map parse-line)
       vec))

(defn value? [x] (integer? x))
(defn register? [x] (not (value? x)))

(defn valid? [i]
  (case (:instr i)
    :cpy (register? (:arg2 i))
    true))

(defn advance-pointer [state]
  (assoc state :pointer (inc (:pointer state))))

(defn get-value [arg state]
  (if (value? arg) arg (state arg)))

(defn get-new-state [state instr]
  (let [arg1 (:arg1 instr)
        arg2 (:arg2 instr)
        next-state (advance-pointer state)]
    (case (:instr instr)
      :inc (assoc next-state arg1 (inc (state arg1)))
      :dec (assoc next-state arg1 (dec (state arg1)))
      :cpy (assoc next-state arg2 (get-value arg1 state))
      :jnz (if (zero? (get-value arg1 state))
             next-state
             (assoc state :pointer (+ (:pointer state) (get-value arg2 state)))))))

(defn toggle [state instructions instr]
  (let [toggle-index (+ (:pointer state) (state (:arg1 instr)))]
    (if-let [toggle-instr (get instructions toggle-index)]
      (assoc instructions toggle-index (toggle-instruction toggle-instr toggle-index))
      instructions)))

(defn process-instruction [state instructions instr]
  (if (valid? instr)
    (if (= :tgl (:instr instr))
      (list (advance-pointer state) (toggle state instructions instr))
      (list (get-new-state state instr) instructions))
    (list (advance-pointer state) instructions)))

(loop [state { :pointer 0 "a" 12 "b" 0 "c" 0 "d" 0 }
       instructions (parse-code "input.txt")]
  (if-let [instr (get instructions (:pointer state))] ; stop when oob
    (let [[new-state new-instructions] (process-instruction state instructions instr)]
      (recur new-state new-instructions))
    (println state)))
