using UnityEngine;

public class CarMovement : MonoBehaviour {

	// The layer of the car for layer masking
	const int LAYER_CAR = 8;

	// The acceleration rate of the car
	public float Acceleration;
	// The rotation rate of the car
	public float TurnRate;
	// How strongly the car corrects its height
	public float LevPower;
	// Gravitational constant of the universe (can only be changed by Q)
	public float Gravity;
	// How high the car levitates
	public float LevHeight;
	// Levitation points to use
	public Vector3[] Levs;

	// The previous distances of all the rays casted
	float[] prevDists;

	// The rigidbody for physics
	Rigidbody rb;

	// Start is called before the first frame update
	void Start() {
		// Grab the rigidbody
		rb = GetComponent<Rigidbody>();
		// Set gravity
		Physics.gravity = new Vector3(0, -Gravity, 0);
		// Initialize the previous distances array
		prevDists = new float[Levs.Length];
	}

	void levitate() {
		// Create the variables for storing the ray and ray hit
		RaycastHit hit;
		Ray ray = new Ray();
		// Cast a ray from the center of the car downwards to find the surface it's floating on
		ray.origin = transform.position;
		ray.direction = transform.TransformDirection(Vector3.down);
		if(Physics.Raycast(ray, out hit, Mathf.Infinity, ~(1 << LAYER_CAR))) {
			// Save the negative normal of the surface to raycast later
			ray.direction = -hit.normal;
		}
		// Loop through each corner of the car
		for(int i = 0; i < Levs.Length; i++) {
			// Set the origin of the ray to the corner of the car
			ray.origin = transform.TransformPoint(Levs[i]);
			// (Enabling gizmos will allow you to see the ray direction)
			Debug.DrawRay(ray.origin, ray.direction * LevHeight * 2, Color.cyan);
			// Cast the ray towards the surface the car is floating on
			if(Physics.Raycast(ray, out hit, LevHeight * 1.25f, ~(1 << LAYER_CAR))) {
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
		
	}

	void FixedUpdate() {
		levitate();
		// Do the keyboard input for movement
		if(Input.GetKey("w")) {
			// Add a force if we're going forwards or backwards
			rb.AddRelativeForce(new Vector3(0, 0, Acceleration));
		}
		if(Input.GetKey("s")) {
			rb.AddRelativeForce(new Vector3(0, 0, -Acceleration));
		}
		if(Input.GetKey("d")) {
			// Add some torque to rotate
			rb.AddRelativeTorque(new Vector3(0, 1, 0) * TurnRate);
		}
		if(Input.GetKey("a")) {
			rb.AddRelativeTorque(new Vector3(0, -1, 0) * TurnRate);
		}
	}

}
