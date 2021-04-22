#!/bin/sh

i=$1
zero_padded=$(seq -f '%02g' $i $i)

mkdir $zero_padded
cd $zero_padded
echo "java -cp ../../clojure-1.8.0/clojure-1.8.0.jar clojure.main $i.clj" > run
chmod +x run
