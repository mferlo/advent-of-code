(defn md5-bytes [input]
  (.digest (doto (java.security.MessageDigest/getInstance "MD5")
             (.update (.getBytes input)))))

(def open? #{ \b \c \d \e \f })

(defn get-doors [input]
  (let [[b1 b2] (md5-bytes input)
        [u d l r] (format "%02x%02x" b1 b2)]
    { :u (open? u) :d (open? d) :l (open? l) :r (open? r) }))

;;  x++ -->
;; #########
;; #S| | | # Start = (0, 0)
;; #-#-#-#-#
;; # | | | # y++
;; #-#-#-#-#  |
;; # | | | #  |
;; #-#-#-#-#  v
;; # | | |   Exit = (3, 3)
;; ####### V
(defn neighbors [cur doors]
  (filter
   (complement nil?)
   (list (if (and (:u doors) (< 0 (:y cur)))
           {:x (:x cur) :y (dec (:y cur)) :path (str (:path cur) "U")})
         (if (and (:d doors) (< (:y cur) 3))
           {:x (:x cur) :y (inc (:y cur)) :path (str (:path cur) "D")})
         (if (and (:l doors) (< 0 (:x cur)))
           {:x (dec (:x cur)) :y (:y cur) :path (str (:path cur) "L")})
         (if (and (:r doors) (< (:x cur) 3))
           {:x (inc (:x cur)) :y (:y cur) :path (str (:path cur) "R")}))))

(defn find-shortest-path [input]
  (loop [q (list { :x 0 :y 0 :path "" })]
    (let [cur (first q)]
      (if (and (= 3 (:x cur)) (= 3 (:y cur)))
        cur
        (let [doors (get-doors (str input (:path cur)))]
          (recur (concat (rest q) (neighbors cur doors))))))))

(defn find-longest-path [input]
  (loop [q (list { :x 0 :y 0 :path "" })
         winner nil]
    (if-let [cur (first q)]
      (let [doors (get-doors (str input (:path cur)))]
        (if (and (= 3 (:x cur)) (= 3 (:y cur)))
          (recur (rest q) cur)
          (recur (concat (rest q) (neighbors cur doors)) winner)))
      winner)))

(println (find-shortest-path "veumntbg"))
(println (count (:path (find-longest-path  "veumntbg"))))
