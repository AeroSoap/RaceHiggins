using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroscopeInput : MonoBehaviour {

	Gyroscope gyro;

	// Start is called before the first frame update
	void Start() {
		gyro = Input.gyro;
		gyro.enabled = true;
	}

	// Update is called once per frame
	void Update() {
		transform.rotation = gyro.attitude;
	}
}
