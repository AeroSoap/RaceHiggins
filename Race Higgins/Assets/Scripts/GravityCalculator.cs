using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityCalculator : MonoBehaviour {

	public static Vector3 GetGravity(Vector3 pos) {
		Vector3 rel = pos - new Vector3(395, 200, -226);
		if(rel.magnitude < 155) {
			return rel.normalized * 50;
		}
		return new Vector3(0, -50, 0);
	}
}
