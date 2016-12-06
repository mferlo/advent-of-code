; https://rosettacode.org/wiki/MD5#Clojure
(defn md5-as-hex [input]
  (apply str
         (map (partial format "%02x")
              (.digest (doto (java.security.MessageDigest/getInstance "MD5")
                         (.update (.getBytes input)))))))

(defn find-starting-with [input-prefix output-prefix]
  (->> (range)
       (map #(str input-prefix %))
       (map md5-as-hex)
       (filter #(clojure.string/starts-with? % output-prefix))))

; part1: "abc" -> "18f47a30"
(comment
(->> (find-starting-with "ugkcyxxp" "00000")
     (take 8)
     (map #(nth % 5))
     (println))
) ;comment

(defn valid-position? [md5-info]
  (contains? #{ \0 \1 \2 \3 \4 \5 \6 \7 } (:pos md5-info)))

; part2: "abc" -> "05ace8e3"
(->> (find-starting-with "abc" "0") ; "00000")
     (map #(hash-map :pos (nth % 5) :ch (nth % 6)))
     (filter valid-position?)
     first
     println)
