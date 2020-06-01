using System;
using UnityEngine;

public class CarTilt : MonoBehaviour {

	public float TiltStrength;
	public float Smoothing;

	float tilt;

	Rigidbody rb;

	// Start is called before the first frame update
	void Start() {
		rb = transform.parent.gameObject.GetComponent<Rigidbody>();
		tilt = 0;
	}

	// Update is called once per frame
	void Update() {
		Vector3 rot = rb.rotation.eulerAngles;
		tilt = tilt * Smoothing + rb.angularVelocity.y * (1 - Smoothing);
		rot.z -= tilt * TiltStrength;
		transform.rotation = Quaternion.Euler(rot);
	}
}
