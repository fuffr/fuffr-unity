using UnityEngine;
using System.Collections;

public class FuffrCube : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void fuffrTouchEvent(string eventData)
	{
		Debug.Log ("@@@ FuffrTouchEvent: " + eventData);
	}
}
