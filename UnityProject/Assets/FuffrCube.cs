using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class FuffrCube : MonoBehaviour
{
	void Start()
	{
		FuffrTouchHandler.Instance.TouchesMoved += TouchesMovedHandler;

		// Test.
		//string json = "{\"touchPhase\":2,\"touches\":[{\"id\":1,\"side\":8,\"x\":105.110626,\"y\":287.396484, \"prevx\":102.786339,\"prevy\":289.388733,\"normx\":0.0098471,\"normy\":0.098743},{\"id\":2,\"side\":8,\"x\":105.110626,\"y\":287.396484, \"prevx\":102.786339,\"prevy\":289.388733,\"normx\":0.328471,\"normy\":0.598743}]}";
		//FuffrTouchHandler.Instance.fuffrTouchEvent(json);
	}

	void Update()
	{
	}

	public void TouchesMovedHandler(FuffrTouchEvent[] touches)
	{
		// Debug log.
        //foreach (FuffrTouchEvent touch in touches)
		//{
		//	Debug.Log("TouchesMovedHandler Touch id: " + touch.id);
        //}

        // Get normalized position of first touch event.
		float normx = touches[0].normx;
		float normy = touches[0].normy;

        // Move to the scaled normalized touch position.
        Vector3 pos = this.transform.position;
		pos.x = (normx * 12f) - 6f;
		pos.y = 9f - (normy * 18f);
		this.transform.position = pos;
	}
}
