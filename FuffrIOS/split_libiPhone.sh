#!/bin/bash
# file: split_libiPhone.sh

mkdir -p Libraries/libiPhone-lib
rm Libraries/libiPhone-lib.a.zip 2> /dev/null
zip -5 Libraries/libiPhone-lib.a.zip Libraries/libiPhone-lib.a
split -b 50m Libraries/libiPhone-lib.a.zip Libraries/libiPhone-lib/part_
rm Libraries/libiPhone-lib.a.zip
