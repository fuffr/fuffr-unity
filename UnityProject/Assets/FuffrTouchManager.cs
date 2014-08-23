using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;

/// <summary>
/// Main class that receives touch events from native code.
/// This script needs to be attached to an empty GameObject
/// named "FuffrTouchManager".
/// </summary>
public class FuffrTouchManager : MonoBehaviour
{
	void Start ()
	{
	}

	void Update ()
	{
	}

	/// <summary>
	/// Method called from Native.
	/// </summary>
	public void fuffrTouchEvent(string eventData)
	{
		FuffrTouchHandler.Instance.fuffrTouchEvent(eventData);
	}
}

/// <summary>
/// Data structure that holds information about a touch event.
/// </summary>
public class FuffrTouchEvent
{
	public int id;
	public int side;
	public int phase;
	public float x;
	public float y;
	public float prevx;
	public float prevy;
	public float normx;
	public float normy;
}

/// <summary>
/// Touch event delegate.
/// </summary>
public delegate void FuffrTouchDelegate(List<FuffrTouchEvent> touches);

/// <summary>
/// Singleton class that handles touch events. To add a touch event,
/// use this style:
///    FuffrTouchHandler.Instance.TouchesMoved += TouchesMovedHandler;
/// </summary>
public class FuffrTouchHandler
{
	public event FuffrTouchDelegate TouchesBegan;
	public event FuffrTouchDelegate TouchesMoved;
	public event FuffrTouchDelegate TouchesEnded;

	private static FuffrTouchHandler instance;

	/// <summary>
	/// Singleton property.
	/// </summary>
	public static FuffrTouchHandler Instance
	{
		get
		{
			if (null == instance)
			{
				instance = new FuffrTouchHandler();
			}
			return instance;
		}
	}

	// Event data is in JSON format.
	// Example data:
	// {"touchPhase":1,
	//   "touches":
	//     [{"id":1,
	//       "side":8,
	//       "x":105.110626,
	//       "y":287.396484,
	//       "prevx":102.786339,
	//       "prevy":289.388733,
	//       "normx":0.328471,
	//       "normy":0.598743}]}
	//
	public void fuffrTouchEvent(string eventData)
	{
		// Debug log.
		Debug.Log ("@@@ FuffrTouchEvent: " + eventData);

		try
		{
			var dict = Json.Deserialize(eventData) as Dictionary<string,object>;

			int touchPhase = GetIntParam(dict, "touchPhase");
			List<object> touchList = (List<object>) dict["touches"];

			// Create list of touches.
			List<FuffrTouchEvent> touches = new List<FuffrTouchEvent>();
			foreach (Dictionary<string,object> touchDict in touchList)
			{
				FuffrTouchEvent touch = new FuffrTouchEvent();
				touch.id = GetIntParam(touchDict, "id");
				touch.side = GetIntParam(touchDict, "side");
				touch.phase = touchPhase;
				touch.x = GetFloatParam(touchDict, "x");
				touch.y = GetFloatParam(touchDict, "y");
				touch.prevx = GetFloatParam(touchDict, "prevx");
				touch.prevy = GetFloatParam(touchDict, "prevy");
				touch.normx = GetFloatParam(touchDict, "normx");
				touch.normy = GetFloatParam(touchDict, "normy");
				touches.Add(touch);
            }

            // Dispatch touch events to event delegates.
            if (1 == touchPhase)
			{
				if (null != TouchesBegan)
				{
					TouchesBegan(touches);
				}
			}
			else if (2 == touchPhase)
			{
				if (null != TouchesMoved)
				{
					TouchesMoved(touches);
				}
			}
			else if (3 == touchPhase)
			{
				if (null != TouchesEnded)
				{
					TouchesEnded(touches);
				}
			}
		}
		catch (Exception ex)
		{
			// Error log.
			Debug.Log("FuffrEventHandler: Error parsing JSON response: " + ex);
		}
	}

	private int GetIntParam(Dictionary<string,object> dict, string key)
	{
		// Check if param exists.
		if (dict.ContainsKey(key))
		{
			return (int)(long)dict[key];
		}
		else
		{
			return 0;
		}
	}

	private float GetFloatParam(Dictionary<string,object> dict, string key)
	{
		// Check if param exists.
		if (dict.ContainsKey(key))
		{
			return (float)(double)System.Convert.ToDouble(dict[key]);
		}
		else
		{
			return 0f;
		}
	}
}
