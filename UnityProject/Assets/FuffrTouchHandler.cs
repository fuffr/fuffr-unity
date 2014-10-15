using UnityEngine;
using System;
using System.Runtime.InteropServices;

/// <summary>
/// Enum for the sides of Fuffr.
/// </summary>
public enum FuffrSide {
	NotSet 	= 0x0,
	Top 	= 0x1,
	Bottom 	= 0x2,
	Left 	= 0x4,
	Right 	= 0x8
}

/// <summary>
/// Enum for the states of a touch.
/// </summary>
public enum FuffrTouchPhase {
	Unknown = 0,
	Began = 1,
	Moved = 2,
	Ended = 4
}

/// <summary>
/// Struct that encapsulates touch data.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct FuffrTouchEvent {
	public Int32 id;
	public FuffrSide side;
	public FuffrTouchPhase phase;
	public float x;
	public float y;
	public float normx;
	public float normy;
	public float prevx;
	public float prevy;
}

/// <summary>
/// Touch event delegate.
/// </summary>
public delegate void FuffrTouchDelegate(FuffrTouchEvent[] touches);

/// <summary>
/// Singleton class that handles touch events. To add a touch event,
/// use this style:
///    FuffrTouchHandler.Instance.TouchesMoved += TouchesMovedHandler;
/// </summary>
public class FuffrTouchHandler : MonoBehaviour {

	public event FuffrTouchDelegate TouchesBegan;
	public event FuffrTouchDelegate TouchesMoved;
	public event FuffrTouchDelegate TouchesEnded;

	// <summary>
	// Singleton property.
	// </summary>
	public static FuffrTouchHandler Instance { get { return instance; } }
	private static FuffrTouchHandler instance;

	private delegate void ReceiveDeviceMessageDelegate([In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=1)] FuffrTouchEvent[] touches, [In] Int32 size);

	static FuffrTouchHandler() {
		if (instance == null) {
			instance = GameObject.FindObjectOfType(typeof(FuffrTouchHandler)) as FuffrTouchHandler;
		}
		if (instance == null) {
			instance = (new GameObject("FuffrTouchManager")).AddComponent<FuffrTouchHandler>();
		}
		_RegisterRecieveDeviceMessageCallback(ReceiveDeviceMessage);

		//_StartTestTimer();
	}

	private FuffrTouchHandler() {}

    private void Awake() {
        DontDestroyOnLoad(this);
		instance = this;
    }

	private void OnApplicationQuit() {
		//_StopTestTimer();
		Destroy(this);
	}

	[MonoPInvokeCallback (typeof (ReceiveDeviceMessageDelegate))]
	protected static void ReceiveDeviceMessage([In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=1)] FuffrTouchEvent[] touches, [In] Int32 size) {
		if (size == 0) return;

		//LogTouches(touches);

		FuffrTouchPhase phase = touches[0].phase;

        // Dispatch touch events to event delegates.
        if (phase == FuffrTouchPhase.Began) {
			if (Instance.TouchesBegan != null) {
				Instance.TouchesBegan(touches);
			}
		}
		else if (phase == FuffrTouchPhase.Moved) {
			if (null != Instance.TouchesMoved) {
				Instance.TouchesMoved(touches);
			}
		}
		else if (phase == FuffrTouchPhase.Ended) {
			if (null != Instance.TouchesEnded) {
				Instance.TouchesEnded(touches);
			}
		}
	}

	private static void LogTouches(FuffrTouchEvent[] touches) {
		foreach (FuffrTouchEvent touch in touches) {

			string phaseString = "Unknown";
			switch(touch.phase) {
				case FuffrTouchPhase.Began: phaseString = "Began"; break;
				case FuffrTouchPhase.Moved: phaseString = "Moved"; break;
				case FuffrTouchPhase.Ended: phaseString = "Ended"; break;
			}

			string sideString = "NotSet";
			switch(touch.side) {
				case FuffrSide.Top: sideString = "Top"; break;
				case FuffrSide.Bottom: sideString = "Bottom"; break;
				case FuffrSide.Left: sideString = "Left"; break;
				case FuffrSide.Right: sideString = "Right"; break;
			}

			Debug.Log("<FuffrTouchEvent id:" + touch.id + " x:" + touch.x + " y:"
				+ " normx:" + touch.normx + " normy:" + touch.normy
				+ " phase:" + phaseString + " side:" + sideString);
		}
	}

#if UNITY_EDITOR
	private static void _RegisterRecieveDeviceMessageCallback(ReceiveDeviceMessageDelegate callback) {}
#else
	#if UNITY_ANDROID
		[DllImport ("unity_bridge")]
	#elif UNITY_IPHONE
		[DllImport ("__Internal")]
	#endif
	private static extern void _RegisterRecieveDeviceMessageCallback(ReceiveDeviceMessageDelegate callback);
#endif

#if UNITY_EDITOR
	private static void _StartTestTimer() {}
#else
	#if UNITY_ANDROID
		[DllImport ("unity_bridge")]
	#elif UNITY_IPHONE
		[DllImport ("__Internal")]
	#endif
	private static extern void _StartTestTimer();
#endif

#if UNITY_EDITOR
	private static void _StopTestTimer() {}
#else
	#if UNITY_ANDROID
		[DllImport ("unity_bridge")]
	#elif UNITY_IPHONE
		[DllImport ("__Internal")]
	#endif
	private static extern void _StopTestTimer();
#endif
}