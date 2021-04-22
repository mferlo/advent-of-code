(require 'clojure.set)

;;; Input parsing

(def input-regex #"(?:(bot \d+) gives low to (.*) and high to (.*))|(?:value (\d+) goes to (bot \d+))")

(defn parse-input [[state actions] cur]
  (let [[_ from low high value to] (re-matches input-regex cur)]
    (if from
      [state (assoc actions from { :low low :high high })]
      [(assoc state (Long/parseLong value) to) actions])))

(defn get-input [s]
  (->> (slurp s)
       (clojure.string/split-lines)
       (reduce parse-input [{} {}])))

; Input like "bot 2 gives low to bot 5 and high to output 0" adds to `actions`
; { "bot 2" { :low "bot 5" :high "output 0" } }
;
; Input like "value 5 goes to bot 2" adds to `state`
; { 5 "bot 2" }
;
; To execute:
; * In state, the duplicate *value* is the name of the bot holding 2 chips.
; * Use that bot's action to update state.
; * Repeat until no more duplicate values.

(defn print-if-answer-to-part-1 [low-chip high-chip bot]
  (if (and (= 17 low-chip)
           (= 61 high-chip))
    (println bot "is the droid we're looking for.")))

(defn print-answer-to-part-2 [state]
  (let [bins (clojure.set/map-invert state)]
    (println (* (bins "output 0") (bins "output 1") (bins "output 2")))))

(defn most-common-value [state]
  (->> (vals state)
       frequencies
       (sort-by val >)
       first))

(defn find-duplicate-value [state]
  (first (most-common-value state)))

(defn has-duplicate-value [state]
  (< 1 (second (most-common-value state))))

(defn get-chips-held-by [bot state]
  (->> (filter #(= bot (second %)) state )
       (map first)
       (sort <)))

(defn take-action [state actions]
  (let [bot (find-duplicate-value state)
        action (actions bot)
        [low-chip high-chip] (get-chips-held-by bot state)]
    (print-if-answer-to-part-1 low-chip high-chip bot)
    (assoc state low-chip (:low action) high-chip (:high action))))

(let [[initial-state actions] (get-input "input.txt")]
  (loop [state initial-state]
    (if (has-duplicate-value state)
      (recur (take-action state actions))
      (print-answer-to-part-2 state))))
