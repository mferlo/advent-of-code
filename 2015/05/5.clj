(defn nice-a [s]
  (and (>= (count (.replaceAll s "[^aeiou]" "")) 3)
       (re-matches #".*(.)\1.*" s)
       (not (or (clojure.string/includes? s "ab")
                (clojure.string/includes? s "cd")
                (clojure.string/includes? s "pq")
                (clojure.string/includes? s "xy")))))

(defn nice-b [s]
  (and (re-matches #".*(..).*\1.*" s)
       (re-matches #".*(.).\1.*" s)))

(with-open [rdr (clojure.java.io/reader "input.txt")]
  (->> (line-seq rdr)
       (filter nice-a)
       count
       println))

(with-open [rdr (clojure.java.io/reader "input.txt")]
  (->> (line-seq rdr)
       (filter nice-b)
       count
       println))
