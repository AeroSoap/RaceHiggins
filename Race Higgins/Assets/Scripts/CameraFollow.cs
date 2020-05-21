using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public GameObject Target;
	public float CameraSpeed;
	public float CameraRotSpeed;
	public float ViewAngle;
	public float VelSense;
	public float RotSense;
	public Vector3 Offset;

	// Start is called before the first frame update
	void Start() {
		
	}

	Vector3 moveTo(Vector3 cur, Vector3 tar) {
		Vector3 moved = cur * (1 - CameraSpeed) + tar * CameraSpeed;
		return moved;
	}

	void FixedUpdate() {
		Rigidbody orb = Target.gameObject.GetComponent<Rigidbody>();
		transform.position = moveTo(transform.position, Target.transform.TransformPoint(Offset)) +
			orb.velocity * VelSense;
		float dist = Quaternion.Angle(transform.rotation, Target.transform.rotation);
		transform.position += Target.transform.TransformDirection(new Vector3(orb.angularVelocity.y * RotSense, 0, 0));
		transform.rotation = Quaternion.Slerp(transform.rotation, orb.rotation, CameraRotSpeed * dist);
	}
}
