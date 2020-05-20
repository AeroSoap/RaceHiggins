using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GyroscopeInput : MonoBehaviour {

	StreamWriter writer;
	Gyroscope gyro;

	// Start is called before the first frame update
	void Start() {
		gyro = Input.gyro;
		gyro.enabled = true;
		gyro.updateInterval = 1f / 240;
		writer = new StreamWriter("test.txt");
		writer.WriteLine("This is a test.");
		writer.Close();
	}

	// Update is called once per frame
	void Update() {
		Vector3 grav = gyro.gravity;
		float other;
		if(Mathf.Abs(grav.y) < Mathf.Abs(grav.z)) {
			other = grav.z;
		} else {
			other = grav.y;
		}
		float angle = Mathf.Rad2Deg * Mathf.Atan2(grav.x, other);
		transform.rotation = Quaternion.Euler(0, 0, angle + 180);
	}
}
