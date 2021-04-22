; https://rosettacode.org/wiki/MD5#Clojure
(defn md5-as-hex [input]
  (apply str
         (map (partial format "%02x")
              (.digest (doto (java.security.MessageDigest/getInstance "MD5")
                         (.update (.getBytes input)))))))

(defn md5-as-hex-with-input [input]
  {:md5 (md5-as-hex input) :input input})

(defn find-first-starting-with [prefix]
  (->> (range)
       (map #(str "yzbqklnj" %))
       (map md5-as-hex-with-input)
       (filter #(clojure.string/starts-with? (:md5 %) prefix))
       first))

; 4a
(println (find-first-starting-with "00000"))

; 4b
(println (find-first-starting-with "000000"))
