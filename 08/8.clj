; 8a
(defn get-ascii-value [s]
  (-> (subs s 2) ; strip the \x
      (Integer/parseInt 16)
      char
      str))

(defn unescape [s]
  (-> (clojure.string/replace s #"\\x[0-9a-f]{2}" get-ascii-value)
      (clojure.string/replace "\\\\" "\\")      ; \\   -> \
      (clojure.string/replace "\\\"" "\"")))    ; \"   -> "

(defn size-in-memory [s]
  (->
   (subs s 0 (dec (count s))) ; remove trailing quote
   (subs 1)                   ; remove leading quote
   unescape
   count))

(->> (slurp "input.txt")
     clojure.string/split-lines
     (map #(- (count %) (size-in-memory %)))
     (reduce +)
     println)

; 8b
(defn encoded-char-size [c]
  (case c
    \" 2
    \\ 2
    1))

(defn encoded-str-size [s]
  (+ 2 (reduce + (map encoded-char-size s))))

(defn encoded-expansion-size [s]
  (- (encoded-str-size s) (count s)))

(->> (slurp "input.txt")
     clojure.string/split-lines
     (map encoded-expansion-size)
     (reduce +)
     println)
