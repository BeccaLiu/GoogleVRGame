using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour {
	// the prefab that holds the cannonball
	public GameObject cannonballPrefab;

	private LineRenderer _trajectoryLineRenderer;

	private const float RotationalVelocity = 50;

	public void RotateClockwise() {
		Rotate(0, RotationalVelocity);
	}

	public void RotateCounterClockwise() {
		Rotate(0, -RotationalVelocity);
	}

	private float _force = 20;

	protected Vector3 Velocity {
		get {
			return transform.localToWorldMatrix * Vector3.up * _force;
		}
	}

	// Use this for initialization
	void Start() {
		// get the line renderer for the trajectory simulation
		_trajectoryLineRenderer = GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	// Update is called once per frame
	private void Update() {

		const int numberOfPositionsToSimulate = 50;
		const float timeStepBetweenPositions = 0.2f;

		// setup the initial conditions
		Vector3 simulatedPosition = transform.position;
		Vector3 simulatedVelocity = Velocity;

		// update the position count
		_trajectoryLineRenderer.positionCount = numberOfPositionsToSimulate;

		for (int i = 0; i < numberOfPositionsToSimulate; i++) {
			// set each position of the line renderer
			_trajectoryLineRenderer.SetPosition(i, simulatedPosition);

			// change the velocity based on Gravity and the time step.
			simulatedVelocity += Physics.gravity * timeStepBetweenPositions;

			// change the position based on Gravity and the time step.
			simulatedPosition += simulatedVelocity * timeStepBetweenPositions;
		}
	}

	// fire a cannonball!
	public void Fire() {
		// create a cannonball at the current position/rotation
		GameObject ball = Instantiate(cannonballPrefab, 
			transform.position, 
			transform.rotation);

		// get the rigidbody physics component
		Rigidbody cannonballRigidbody = ball.GetComponent<Rigidbody>();

		// apply the Velocity
		cannonballRigidbody.AddForce(Velocity, ForceMode.VelocityChange);
	}
	private void Rotate(float x, float y) {
		Vector3 transformEulerAngles = transform.eulerAngles;
		transformEulerAngles.x += x * Time.deltaTime;
		transformEulerAngles.y += y * Time.deltaTime;
		transform.eulerAngles = transformEulerAngles;
	}
}
