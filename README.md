# Fuffr Unity

Unity project with iOS bindnings for Fuffr events.

## Unity Code

The Unity project contains a GameObject with a script called "FuffrCube.cs" that includes a simple test of the touch event handling.

File "FuffrTouchHandler.cs" contains following:

* FuffrTouchHandler - singleton class you use to register touch delegates.
* FuffrTouchEvent - data structure that holds information about a touch event.
* FuffrTouchDelegate  touch delegate type used to listen to touch events.

## Merging/splitting the Unity Xcode lib file

You need to assemble the file FuffrIOS/Libraries/libiPhone-lib.a to build the Xcode project. It is split into sveral parts, beacuse it is too big to be put on GitHub.

To this assemble the file, you only need to do this when the lib file has been updated, that is when you have rebuilt the Xcode project from Unity:

    cd FuffrIOS
    ./merge_libiPhone.sh

Be sure to select "Append" when you build the Xcode project from Unity! (Select "File/Build Settings", "iOS", "Development Build", and press "Build". Then press "Append" in the dialog that displays.)

When you modify the Unity project and rebuild the Xcode project from Unity, you need to split this file again before you commit any changes to GitHub (because parts have been deleted by the build):

    cd FuffrIOS
    ./split_libiPhone.sh

## Linking With FuffrLib in Xcode

Note that the following has already been done with the Xcode project in this repository! You only need to do this with new projects generated from Unity.

Here is a summary of the settings/modifications needed to link a Unity generated Xcode project with FuffrLib:

* Drag the FuffrLib Xcode project file into the Unity Xcode project.
* Under "Build Phases/Link Binary With Libraries" click "+" and add "libFiffrLib.a" and "CoreBluetooth.framework".
* Under "Build Settings/Search Paths/Header Search Paths/Debug" add "$(BUILT_PRODUCTS_DIR)/Debug-iphoneos/include" (including the quote marks).
* Under "Build Settings/Search Paths/Header Search Paths/Release" add "$(BUILT_PRODUCTS_DIR)/Release-iphoneos/include" (including the quote marks).
* Under "Build Settings/Linking/Other Linker Flags" add "-ObjC" (without the quote marks).

