# Fuffr Unity

Unity project with iOS bindnings for Fuffr events.

## Unity Code

The Unity project contains a GameObject with a script called "FuffrCube.cs" that includes a simple test of the touch event handling.

The file "FuffrTouchManager.cs" contains classes for touch handling:

* FuffrTouchManager - receives touch events from native code.
* FuffrTouchHandler - singleton class you use to register touch delegates.
* FuffrTouchEvent - data structure that holds information about a touch event.
* FuffrTouchDelegate  touch delegate type used to listen to touch events.

## iOS Code - setting active sides and number of touches

The Fuffr modifications for the Unity Xcode project are in file: FuffrIOS/Classes/UnityAppController.mm

To set the active sides of the Fuffr case and the number of touches per side, modify the following values in the method **setupFuffr**:

	// Set active sides and number of touches per side.
	FFRSide activeSides = (FFRSide) (FFRSideLeft | FFRSideRight);
	NSNumber* touchesPerSide = @1;

For example, to enable two touches on all four sides, you would use:

	// Set active sides and number of touches per side.
	FFRSide activeSides = (FFRSide) (FFRSideLeft | FFRSideRight | FFRSideTop | FFRSideBottom);
	NSNumber* touchesPerSide = @2;

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
* Modify the file "Classes/UnityAppController.mm" in the Unity Xcode project to setup Fuffr touch events and send events to unity. The modifications are found in this repository and are marked with "// FUFFR" comments. (This code should go in a separate file, but for now all code is added to this file.)


