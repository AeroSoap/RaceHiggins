using UnityEngine;

public class CarTilt : MonoBehaviour {

	Rigidbody rb;

	// Start is called before the first frame update
	void Start() {
		rb = transform.parent.gameObject.GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void Update() {
		transform.rotation = rb.rotation;
	}
}
