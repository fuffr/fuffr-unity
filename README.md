# Fuffr Unity Plugin
Here you can find a Unity plugin to enable magic of Fuffr in your game. There's also a preconfigured example project included to get you up and running in few minutes.

Please note that Fuffr relies on a native code to handle and communicate with hardware, hence currently **only supports iOS** and **is not possible to test in Unity editor**. You will need to export your project to iOS to experience Fuffr.

## Running example project
Fuffr relies on Bluetooth to communicate with your phone, so there're two extra steps required for the Xcode project to be able to build successfully after Unity export.

Here's a step by step guide:

1. Open the project in Unity.
2. Select iOS in File > Build Settings...
3. Build
4. Open generated Xcode project
5. Under "Build Phases/Link Binary With Libraries" click "+" and add "CoreBluetooth.framework".
6. Under "Build Settings/Linking/Other Linker Flags" add "-ObjC" (without the quote marks).

## Using Fuffr Plugin in your project
1. Double click on `FuffrUnityPlugin.unitypackage` and import it to your project
2. If you'd like to quickly test Fuffr, drag `FuffrCube` prefab located in `Plugins/Fuffr` to your game scene.
3. To start listening and subscribe to touch events coming from Fuffr hardware in your own game object, look at `Start()`method inside `FuffrCube.cs` for an example.
4. Use iOS Player export to generate Xcode project
5. Follow steps 5 & 6 from __Running example project__ section above to complete setup.