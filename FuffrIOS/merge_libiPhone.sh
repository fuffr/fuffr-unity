#!/bin/bash
# file: merge_libiPhone.sh

rm Libraries/libiPhone-lib.a 2> /dev/null
rm Libraries/libiPhone-lib.a.zip 2> /dev/null
cat Libraries/libiPhone-lib/part_* > Libraries/libiPhone-lib.a.zip
unzip Libraries/libiPhone-lib.a.zip Libraries/libiPhone-lib.a
rm Libraries/libiPhone-lib.a.zip