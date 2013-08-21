#!/bin/sh

PROJECT_DIR=$(cd `dirname $0` && pwd)/..
LOG_FILE=${PROJECT_DIR}/_work/optimize-images.log

cd ${PROJECT_DIR}/_patterns

echo -n "Crushing png's"
for png in `find . -iname "*.png" -type f`
do
    echo -n "."
    #echo "Crushing $png"
    #pngcrush -rem alla -brute -reduce "$png" temp.png >> ${LOG_FILE}
    optipng -o7 "$png" --out temp.png >> ${LOG_FILE}
    mv -f temp.png $png
done

echo ""
echo -n "Optimizing jpg's"
for jpg in `find . -iname "*.jpg" -type f`
do
    echo "Optimizing $jpg"
    echo -n "."
    jpegtran -copy none -progressive -optimize -outfile temp.jpg "$jpg"
    mv -f temp.jpg $jpg
done

echo ""

cd -

