; https://rosettacode.org/wiki/MD5#Clojure
(defn md5-as-hex [input]
  (apply str
         (map (partial format "%02x")
              (.digest (doto (java.security.MessageDigest/getInstance "MD5")
                         (.update (.getBytes input)))))))

; part 1
(defn find-starting-with [input-prefix output-prefix]
  (->> (range)
       (map #(str input-prefix %))
       (map md5-as-hex)
       (filter #(clojure.string/starts-with? % output-prefix))))

; "abc" -> "18f47a30"
(comment
(->> (find-starting-with "ugkcyxxp" "00000")
     (take 8)
     (map #(nth % 5))
     (println))
) ;comment

; part 2
(def positions #{ \0 \1 \2 \3 \4 \5 \6 \7 })

(defn valid-position? [md5-info]
  (contains? positions (:pos md5-info)))

(defn have-all-positions? [acc]
  (= positions (set (keys acc))))

(defn process-until-all-positions-filled [acc cur]
  (if (have-all-positions? acc)
    (reduced acc)
    (if (contains? acc (:pos cur))
      acc
      (assoc acc (:pos cur) (:ch cur)))))

; "abc" -> "05ace8e3"
(->> (find-starting-with "ugkcyxxp" "00000")
     (map #(hash-map :pos (nth % 5) :ch (nth % 6)))
     (filter valid-position?)
     (reduce process-until-all-positions-filled (sorted-map))
     vals
     clojure.string/join
     println)
