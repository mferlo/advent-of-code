; Not even trying to make this efficient

(def disk-size 35651584) ; 272)
(def input "01000100010010111")

(defn calc-data [s]
  (apply str s "0" (map #(if (= \0 %) \1 \0) (reverse s))))

(defn make-data [s]
  (let [data (calc-data s)]
    (if (< (count data) disk-size)
      (make-data data)
      (subs data 0 disk-size))))

(defn calc-checksum [data]
  (loop [s data checksum []]
    (if-let [s1 (first s)]
      (let [ch (if (= s1 (second s)) "1" "0")]
        (recur (drop 2 s) (conj checksum ch)))
      (apply str checksum))))
      
(defn make-checksum [s]
  (let [checksum (calc-checksum s)]
    (if (even? (count checksum))
      (make-checksum checksum)
      checksum)))

(println (make-checksum (make-data input)))
