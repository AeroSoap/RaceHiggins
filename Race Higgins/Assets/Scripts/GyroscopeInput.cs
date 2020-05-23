using UnityEngine;

public class GyroscopeInput : MonoBehaviour {

	public GameObject Vehicle;

	Gyroscope gyro;

	public static float angle;

	// Start is called before the first frame update
	void Start() {
		gyro = Input.gyro;
		gyro.enabled = true;
		gyro.updateInterval = 1f / 240;
	}

	// Update is called once per frame
	void Update() {
		Vector3 grav = gyro.gravity;
		float other;
		// Determine the most accurate value to use
		if(Mathf.Abs(grav.y) < Mathf.Abs(grav.z)) {
			other = grav.z;
		} else {
			other = grav.y;
		}
		// Use that value with grav.x to determine the rotation
		angle = Mathf.Rad2Deg * Mathf.Atan2(grav.x, other);
		// Angle is centered around 180 and -180 for some reason, fix it
		angle = -Mathf.Sign(angle) * (180 - Mathf.Abs(angle));
		Vector3 dir = Vehicle.transform.rotation.eulerAngles;
		// Rotate the camera so that it stays level in the real world
		transform.rotation = Quaternion.Euler(dir + new Vector3(0, 0, angle));
		Debug.Log(transform.InverseTransformDirection(transform.rotation.eulerAngles) + " " + transform.rotation.eulerAngles);
		// Clamp the angle from -45 to 45, to prevent over-steering
		angle = Mathf.Clamp(angle, -45, 45);
	}
}
