# Fuffr Unity

Unity project with iOS bindnings for Fuffr events.

## Unity Code

The Unity project contains a GameObject with a script called "FuffrCube.cs" that includes a simple test of the touch event handling.

The file "FuffrTouchManager.cs" contains classes for touch handling, and some documentation comments. Overview of objects in this file:

* FuffrTouchManager - receives touch events from native code.
* FuffrTouchHandler - singleton class you use to register touch delegates.
* FuffrTouchEvent - data structure that holds information about a touch event.
* FuffrTouchDelegate  touch delegate type used to listen to touch events.

## iOS Code

The Fuffr modifications for the Unity Xcode project are in file: FuffrIOS/Classes/UnityAppController.mm

You need to assemble the file FuffrIOS/Libraries/libiPhone-lib.a to build the Xcode project. It is split into sveral parts, beacuse it is too big to be put on GitHub.

To this assemble the file, you only need to do this when the lib file has been updated:

    cd FuffrIOS
    ./merge_libiPhone.sh

When you modify the Unity project and rebuild the Xcode project from Unity, you ned to split this file again (because parts have been deleted by the build):

    cd FuffrIOS
    ./split_libiPhone.sh

## Linking With FuffrLib in Xcode

Here is a summary fo the settings/modifications needed to link your own Unity generated Xcode project with FuffrLib:

* Drag the FuffrLib Xcode project file into the Unity Xcode project.
* Under "Build Phases/Link Binary With Libraries" click "+" and add "libFiffrLib.a" and "CoreBluetooth.framework".
* Under "Build Settings/Search Paths/Header Search Paths/Debug" add "$(BUILT_PRODUCTS_DIR)/Debug-iphoneos/include" (including the quote marks).
* Under "Build Settings/Search Paths/Header Search Paths/Release" add "$(BUILT_PRODUCTS_DIR)/Release-iphoneos/include" (including the quote marks).
* Under "Build Settings/Linking/Other Linker Flags" add "-ObjC" (without the quote marks).
* Modify the file "Classes/UnityAppController.mm" in the Unity Xcode project to setup Fuffr touch events and send events to unity. The modifications are found in this repository and are marked with "// FUFFR" comments. (This code should go in a separate file, but for now all code is added to this file.)

Note that the above has already been done with the project in this repository!
