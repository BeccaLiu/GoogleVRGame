using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Throwable : MonoBehaviour {
	// the calculated velocity for when the user releases the GameObject
	private Vector3 _throwVelocity;

	// the position of the object during the previous frame.
	private Vector3 _previousPosition;

	// Use this for initialization
	void Start () {
		gameObject.AddListener(EventTriggerType.PointerDown, Hold);
		gameObject.AddListener(EventTriggerType.PointerUp, Release);
	}

	public void Hold() {
		// get the Transform component of the pointer
		Transform pointerTransform = GvrPointerManager.Pointer.PointerTransform;

		// set the GameObject's parent to the pointer
		transform.SetParent(pointerTransform, false);

		// position it in the view
		transform.localPosition = new Vector3(0, 0, 2);

		// disable physics
		GetComponent<Rigidbody>().isKinematic = true;
	}

	public void Release() {
		// set the parent to the world
		transform.SetParent(null, true);

		// get the rigidbody physics component
		Rigidbody rigidbody = GetComponent<Rigidbody>();

		// reset velocity
		rigidbody.velocity = Vector3.zero;

		// enable physics
		rigidbody.isKinematic = false;

		// throw the object when releasing while held
		rigidbody.AddForce(_throwVelocity, ForceMode.VelocityChange);
	}

	// Update is called once per frame
	void Update() {
		// the velocity is based on the previous position
		Vector3 frameVelocity = (transform.position - _previousPosition) / Time.deltaTime;

		const int samples = 3;
		// average the velocity calculate over the last number of frames
		_throwVelocity = _throwVelocity * (samples - 1) / samples + frameVelocity / samples;

		// update previous position
		_previousPosition = transform.position;
	}
}
