using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class Pushable : MonoBehaviour {
	// the current time value
	private float _animationTime = 0;

	// the velocity in which the animation value will change
	private float _animationVelocity;

	// store this so we can lerp between it and the pushed position
	private Vector3 _originalPosition;

	// fired once when the button transitions to the down state
	public UnityEvent onPressed;

	// fired every frame the button is in the down state
	public UnityEvent onDown;

	// fired once when the button transitions out of the down state
	public UnityEvent onReleased;

	private void UpdateTransform() {
		// we will move the object 0.5 units based on its scale
		float pushDistance = 0.5f * transform.localScale.y;

		// determine the direction from the object's rotation and the up vector
		Vector3 pushDirection = transform.localRotation * Vector3.up;

		// determine where the new position will be based on the direction and distance
		Vector3 pushedPosition = _originalPosition - pushDirection * pushDistance;

		// set the local position to the lerp between the original and pushed position
		transform.localPosition = Vector3.Lerp(_originalPosition, pushedPosition, _animationTime);
	}

	private void UpdateAnimationTime() {
		// if pushed, time moves forwards, if not, time moves backwards
		float timestep = _animationVelocity * Time.deltaTime;

		// calculate the new animation time and clamp it to [0..1]
		_animationTime = Mathf.Clamp(_animationTime + timestep, 0, 1.0f);
	}

	// Use this for initialization
	void Start () {
		_originalPosition = transform.localPosition;

		gameObject.AddListener(EventTriggerType.PointerDown, () => {
			_animationVelocity = 5.0f;
		});
		gameObject.AddListener(EventTriggerType.PointerUp, () => {
			_animationVelocity = -5.0f;
		});
	}
	
	// Update is called once per frame
	void Update () {
		// store the previous animation time for later
		float previousAnimationTime = _animationTime;

		UpdateAnimationTime();

		UpdateTransform();

		CheckForEvents(previousAnimationTime);
		
	}

	private void CheckForEvents(float previousAnimationTime) {
		// when the animation time crosses this threshold, it will be considered pushed
		const float pushThreshold = 0.9f;

		// calculate before and after states
		bool wasDown = previousAnimationTime > pushThreshold;
		bool isDown = _animationTime > pushThreshold;

		// send press event if first frame pressed
		if (!wasDown && isDown) {
			onPressed.Invoke();
		}

		// always send down event if down
		if (isDown) {
			onDown.Invoke();
		}

		// send released event if first frame released
		if (wasDown && !isDown) {
			onReleased.Invoke();
		}
	}
}
