# Fuffr Unity

Unity project with bindnings for Fuffr events.

You need to assemble the file FuffrIOS/Libraries/libiPhone-lib.a to build the Xcode project. It is split into sveral parts, beacuse itis too big to be put on GitHub.

Do this assemble the file, you only need to do this once (or when the lib file is updated):

    cd FuffrIOS
    ./merge_libiPhone.sh

The Fuffr modifications are in file: FuffrIOS/Classes/UnityAppController.mm
