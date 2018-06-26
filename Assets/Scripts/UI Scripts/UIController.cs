using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour {

	public float movementSpeed = 0.5f;
	public Vector2 disabledPosition;
		
	private Vector2 enabledPosition;

	private float vectorDistance;

	private bool enabling = false;
	private bool disabling = false;
	private float startTime;

	void Start() {
		enabledPosition = (Vector2)transform.localPosition;
		vectorDistance = enabledPosition.x - disabledPosition.x;
	}

	public void MoveToDisplay() {

		enabling = true;

		startTime = Time.time;

		Debug.Log ("Checkit");
	}

	public void MoveAndDisable() {

		disabling = true;

		startTime = Time.time;

	}

	void LateUpdate() {

		if (disabling) {
			float step = movementSpeed * (Time.time - startTime);
			float stepAsFrac = step / vectorDistance;
			transform.localPosition = Vector2.Lerp (enabledPosition, disabledPosition, stepAsFrac);

			if (transform.localPosition.x <= disabledPosition.x) {
				disabling = false;
			}
		}

		if (enabling) {
			float step = movementSpeed * (Time.time - startTime);
			float stepAsFrac = step / vectorDistance;
			transform.localPosition = Vector2.Lerp (disabledPosition, enabledPosition, stepAsFrac);

			if (transform.localPosition.x >= enabledPosition.x) {
				enabling = false;
			}
		}
	}
}
