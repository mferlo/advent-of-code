; part 1
(defn parse []
  (->> (slurp "input.txt")
       (re-seq #"(.*)-(\d+)\[(.*)\]")
       (map rest)))

(defn calculate-checksum [s]
  (->> (clojure.string/replace s "-" "")
       sort
       frequencies
       (sort-by val >)
       (take 5)
       keys
       clojure.string/join))

(defn room-value [room-info]
  (let [[full-name id given-checksum] room-info
        expected-checksum (calculate-checksum full-name)]
    (if (= given-checksum expected-checksum) (Integer. id) 0)))

(->> (parse)
     (map room-value)
     (reduce +)
     println)

; part 2
(defn rotate-char [ch n]
  (let [c (- (int ch) (int \a))]
    (char (+ (int \a) (mod (+ c n) 26)))))

(defn decode-char [ch n]
  (if (= ch \-) \  (rotate-char ch n)))

(defn decode-name [encoded-name n]
  (clojure.string/join (map #(decode-char % n) encoded-name)))

(defn decode [room-info]
  (let [[encoded-name id _] room-info]
    { :name (decode-name encoded-name (Integer. id))
      :id id }))

(->> (parse)
     (map decode)
     (filter #(= (:name %) "northpole object storage"))
     println)
