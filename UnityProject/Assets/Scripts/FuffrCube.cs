using UnityEngine;

public class FuffrCube : MonoBehaviour
{
	void Start()
	{
		FuffrTouchHandler.Instance.TouchesMoved += TouchesMovedHandler;
	}

	void OnDestroy()
	{
		FuffrTouchHandler.Instance.TouchesMoved -= TouchesMovedHandler;
	}

	void Update()
	{
	}

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
