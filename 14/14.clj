; https://rosettacode.org/wiki/MD5#Clojure
(defn md5-impl [input]
  (apply str
         (map (partial format "%02x")
              (.digest (doto (java.security.MessageDigest/getInstance "MD5")
                         (.update (.getBytes input)))))))

; Part 1 - Always call memoized
;(def md5 (memoize md5-impl))

; Part 2 - Don't memoize intermediate results
(defn md5-with-key-stretching [input]
  (loop [i 0 s input]
    (if (= i 2017)
      s
      (recur (inc i) (md5-impl s)))))
(def md5 (memoize md5-with-key-stretching))

;(def salt "abc")
(def salt "ihaygndm")

(defn quintuple-search [start stop-after pattern]
  (loop [i start]
    (if (< stop-after i)
      false
      (if (re-find pattern (md5 (str salt i)))
        true
        (recur (inc i))))))

(defn quintuple-in-next-1000? [digit i]
  (let [pattern (re-pattern (apply str (repeat 5 digit)))]
    (quintuple-search (inc i) (+ i 1001) pattern)))

(defn is-key? [i]
  (if-let [m (re-find #"(.)\1\1" (md5 (str salt i)))]
    (quintuple-in-next-1000? (second m) i)))

(loop [i 0 found 0]
  (if (= found 64)
    (println (dec i))
    (recur (inc i)
           (if (is-key? i)
             (inc found)
             found))))
