using UnityEngine;
using UnityEngine.SceneManagement;

public class CarMovement : MonoBehaviour {

	// The layer of the car for layer masking
	const int LAYER_CAR = 8;

	// The acceleration rate of the car
	public float Acceleration;
	// The rotation rate of the car
	public float TurnRate;
	// How strongly the car corrects its height
	public float LevPower;
	// How high the car levitates
	public float LevHeight;
	// Maximum velocity
	public float MaxSpeed;
	// Braking speed
	public float BrakeSpeed;
	// How quickly the effective normal is updated
	public float NormalSpeed;
	// Levitation points to use
	public Vector3[] Levs;
	// Audio source to change volume and pitch of
	public AudioSource sound;

	// Previous velocity
	Vector3 prevVel;

	// Smoothed difference value of car velocity
	float delta;

	// The previous distances of all the rays casted
	float[] prevDists;

	// The rigidbody for physics
	Rigidbody rb;

	// Touch input
	bool leftDown, rightDown;

	// Start is called before the first frame update
	void Start() {
		// Grab the rigidbody
		rb = GetComponent<Rigidbody>();
		// Set gravity
		// Initialize the previous distances array
		prevDists = new float[Levs.Length];
	}

	Vector3 updateNormal(Ray ray, RaycastHit hit) {
		// Cast a ray from the center of the car downwards to find the surface it's floating on
		ray.origin = transform.position;
		ray.direction = transform.TransformDirection(Vector3.down);
		Debug.DrawRay(ray.origin, ray.direction * 20, Color.green);
		if(Physics.Raycast(ray, out hit, Mathf.Infinity, ~(1 << LAYER_CAR))) {
			return -hit.normal;
		}
		return Vector3.down;
	}

	bool levitate() {
		// Track if we're on the ground
		bool onGround = false;
		// Create the variables for storing the ray and ray hit
		Ray ray = new Ray();
		RaycastHit hit = new RaycastHit();
		ray.direction = updateNormal(ray, hit);
		// Loop through each corner of the car
		for(int i = 0; i < Levs.Length; i++) {
			// Set the origin of the ray to the corner of the car
			ray.origin = transform.TransformPoint(Levs[i]);
			// (Enabling gizmos will allow you to see the ray direction)
			Debug.DrawRay(ray.origin, ray.direction * LevHeight * 2, Color.red);
			// Cast the ray towards the surface the car is floating on
			if(Physics.Raycast(ray, out hit, LevHeight * 1.25f, ~(1 << LAYER_CAR))) {
				// We're on the ground (kinda, anyway)
				onGround = true;
				// If there was no previous hit, account for it so dif is just 0
				if(prevDists[i] == 0) {
					prevDists[i] = hit.distance;
				}
				// Calculate the rate of change in distance
				float dif = hit.distance - prevDists[i];
				// Calculate the error value
				float err = hit.distance - LevHeight;
				float power = 0;
				// If that point on the car is not already moving towards the goal (or is moving way too slowly),
				// apply a force at that location proportional to the error value
				if(Mathf.Sign(err) == Mathf.Sign(dif) || Mathf.Abs(dif) < 0.002) {
					power = err;
				}
				// Actually apply the force, using the ray's direction and the error value, scaled by LevPower
				rb.AddForceAtPosition(ray.direction.normalized * power * LevPower, ray.origin);
				// Save the previous hit distance
				prevDists[i] = hit.distance;
			} else {
				// Reset the previous distance if there was no hit
				prevDists[i] = 0;
			}
		}
		return onGround;
	}

	// Limits the current velocity to max by applying a counteracting force
	void limitVel(float max) {
		if(rb.velocity.magnitude > max) {
			Vector3 counter = -rb.velocity;
			counter = counter.normalized * (counter.magnitude - max);
			rb.AddForce(counter / Time.fixedDeltaTime);
		}
	}

	// Sets the booleans leftDown and rightDown accordingly based on touch input
	void getTouchInputs() {
		leftDown = false;
		rightDown = false;
		for(int i = 0; i < Input.touchCount; i++) {
			Touch touch = Input.GetTouch(i);
			if(touch.position.x < Screen.width / 2) {
				leftDown = true;
			} else {
				rightDown = true;
			}
		}
	}

	// Changes the volume and pitch of the hovering sound
	void changeSound() {
		float dif = (rb.velocity - prevVel).magnitude;
		dif = Mathf.Min(dif, 1);
		delta = delta * 0.99f + dif * 0.01f;
		sound.volume = delta / 4 + 0.1f;
		sound.pitch = delta / 2 + 1;
		prevVel = rb.velocity;
	}

	void FixedUpdate() {
		// Update gravity
		Physics.gravity = GravityCalculator.GetGravity(transform.position);
		getTouchInputs();
		// Only allow acceleration and braking if the car is grounded
		if(levitate()) {
			// Do the keyboard input for movement
			if(Input.GetKey("w") || leftDown) {
				// Add a force if we're going forwards or backwards
				rb.AddRelativeForce(new Vector3(0, 0, Acceleration));
			}
			if(Input.GetKey("s") || rightDown) {
				rb.AddRelativeForce(new Vector3(0, 0, -Acceleration));
			}
			if(Input.GetKey(KeyCode.LeftShift) || (leftDown && rightDown)) {
				// Brakes
				limitVel(rb.velocity.magnitude - BrakeSpeed);
			}
		}
		if(Input.GetKey("d")) {
			// Add some torque to rotate
			rb.AddRelativeTorque(new Vector3(0, 1, 0) * TurnRate);
		}
		if(Input.GetKey("a")) {
			rb.AddRelativeTorque(new Vector3(0, -1, 0) * TurnRate);
		}
		// Do steering based off of phone rotation
		rb.AddRelativeTorque(new Vector3(0, -1, 0) * TurnRate * GyroscopeInput.angle / 45);
		limitVel(MaxSpeed);
		changeSound();
	}

}
