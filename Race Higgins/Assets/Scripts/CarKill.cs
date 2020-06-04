using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarKill : MonoBehaviour {

	// The size of killzone tiles, lower values mean higher resolution but more memory usage
	public int TileSize = 50;
	// How far down from the bottom of the track the kill zone should be
	public float HeightOffset = 200;
	// How far to propagate distance values
	public float PropagationDistance = 250;
	// Large track objects from which to base the boundaries of the killzone
	// This array does not need to include small objects like obstacles
	public GameObject[] TrackObjects;

	// The minimum values of the killzone grid
	Vector3 min;
	// The actual array grid storing the height of each tile in the kill zone
	float[,] killGrid;

	// Used in place of null
	// Odds of this causing issues are extremely low :)
	const float floatNull = 0.23819238f;

	// How long the player has been stuck upside down
	float turtleTimer = 0;
	
	// Converts coordinates from the world space to the grid space
	Vector3 worldToGrid(Vector3 pos) {
		pos.x -= min.x;
		pos.z -= min.z;
		pos.x /= TileSize;
		pos.z /= TileSize;
		return pos;
	}

	// Converts coordinates from the grid space to the world space
	Vector3 gridToWorld(Vector3 pos) {
		pos.x *= TileSize;
		pos.z *= TileSize;
		pos.x += min.x;
		pos.z += min.z;
		return pos;
	}

	// Sets up the killzone grid for usage
	void createGrid() {
		// Set default bounds to be updated
		min = new Vector3(float.MaxValue, 0, float.MaxValue);
		Vector3 max = new Vector3(float.MinValue, 0, float.MinValue);
		// For each object in TrackObjects, update the bounds
		foreach(GameObject obj in TrackObjects) {
			Bounds bounds = obj.GetComponent<Collider>().bounds;
			min.x = Mathf.Min(min.x, bounds.min.x);
			min.z = Mathf.Min(min.z, bounds.min.z);
			max.x = Mathf.Max(max.x, bounds.max.x);
			max.z = Mathf.Max(max.z, bounds.max.z);
		}
		// Give a bit of space
		min.x -= TileSize;
		min.z -= TileSize;
		max.x += TileSize;
		max.z += TileSize;
		// Calculate the dimensions of the collision array based on the bounding box
		int dimX = (int)(max.x - min.x) / TileSize;
		int dimZ = (int)(max.z - min.z) / TileSize;
		killGrid = new float[dimX, dimZ];
	}

	// Gets the height of the killzone in each space where a track exists
	void getHeights() {
		int amt = 0;
		Ray ray = new Ray();
		int mask = LayerMask.GetMask("Track");
		RaycastHit hit;
		ray.direction = new Vector3(0, 1, 0);
		for(int i = 0; i < killGrid.GetLength(0); i++) {
			for(int j = 0; j < killGrid.GetLength(1); j++) {
				ray.origin = gridToWorld(new Vector3(i, -1e6f, j));
				if(Physics.Raycast(ray, out hit, float.PositiveInfinity, mask)) {
					amt++;
					killGrid[i, j] = hit.point.y - HeightOffset;
				} else {
					killGrid[i, j] = floatNull;
				}
			}
		}
	}

	// Fills in the empty spaces with values so the kill zone works when you leave the track
	void fillSpaces() {
		bool[,] wasNull = new bool[killGrid.GetLength(0), killGrid.GetLength(1)];
		for(int x = 0; x < killGrid.GetLength(0); x++) {
			for(int z = 0; z < killGrid.GetLength(1); z++) {
				wasNull[x, z] = killGrid[x, z] == floatNull;
			}
		}
		int amt = (int)(PropagationDistance / TileSize + 0.999f);
		for(int x = 0; x < killGrid.GetLength(0); x++) {
			for(int z = 0; z < killGrid.GetLength(1); z++) {
				killGrid[x, z] = float.MaxValue;
				for(int x2 = x - amt; x2 <= x + amt; x2++) {
					for(int z2 = z - amt; z2 <= z + amt; z2++) {
						if(x2 >= 0 && x2 < killGrid.GetLength(0) && z2 >= 0 && z2 < killGrid.GetLength(1)) {
							if(!wasNull[x2, z2]) {
								killGrid[x, z] = Mathf.Min(killGrid[x, z], killGrid[x2, z2]);
							}
						}
					}
				}
				if(killGrid[x, z] == float.MaxValue) {
					killGrid[x, z] = floatNull;
				}
			}
		}
	}

	// Start is called before the first frame update
	void Start() {
		Physics.queriesHitBackfaces = true;
		createGrid();
		getHeights();
		fillSpaces();
	}

	void FixedUpdate() {
		// Check if the player is in the killzone and reset if they are
		Vector3 pos = worldToGrid(transform.position);
		if(!(pos.x >= 0 && pos.x < killGrid.GetLength(0) && pos.z >= 0 && pos.z < killGrid.GetLength(1)) || 
			killGrid[(int)pos.x, (int)pos.z] > transform.position.y || killGrid[(int)pos.x, (int)pos.z] == floatNull) {
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
		// Check the player has been "turtled" (flipped over) and reset if they have been for a certain amount of time
		Ray ray = new Ray(transform.position, transform.rotation * Vector3.up);
		Debug.DrawRay(ray.origin, ray.direction * 3, Color.blue);
		if(Physics.Raycast(ray, 3, ~(1 << 8))) {
			turtleTimer += Time.fixedDeltaTime;
			if(turtleTimer >= 1) {
				SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			}
		} else {
			turtleTimer = 0;
		}
	}
}
