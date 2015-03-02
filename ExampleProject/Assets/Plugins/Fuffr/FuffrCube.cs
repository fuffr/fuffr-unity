using UnityEngine;

public class FuffrCube : MonoBehaviour
{
	void Start()
	{
		// It's possible to configure manually what sides and how many touches Fuffr will track.
		// Important to set those values before calling StartListening().
		// By default Fuffr will listen on all sides and maximum of 5 touches per side
		//
		// FuffrTouchHandler.Instance.ActiveSides = FuffrSide.Left | FuffrSide.Right;
		// FuffrTouchHandler.Instance.TouchesPerSide = 2;

		// Tells Fuffr to start tracking touch events
		FuffrTouchHandler.Instance.StartListening();
		FuffrTouchHandler.Instance.TouchesMoved += TouchesMovedHandler;
	}

	void OnDestroy()
	{
		// Remove the listener before the scene gets destroyed
		FuffrTouchHandler.Instance.TouchesMoved -= TouchesMovedHandler;
	}

	void Update()
	{

	}

	// Respond to incoming Fuffr touch events
	public void TouchesMovedHandler(FuffrTouchEvent[] touches)
	{
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
