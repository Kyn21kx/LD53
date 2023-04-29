using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainSplit : MonoBehaviour {
	
	private const int LANE_COUNT = 4;
	private const float FLOOR_LEVEL = 0.6f;

	[SerializeField]
	private GameObject lanePrefab;
	[SerializeField]
	private GameObject obstaclePrefab;
	private float proportionalWidth;

	private void Start() {
		//Get the scale and divide into laneCount
		float width = transform.localScale.x;
		this.proportionalWidth = width / LANE_COUNT;
		Debug.Log($"ProportionalWidth: {this.proportionalWidth}");
		for (int i = 0; i < LANE_COUNT + 1; i++) {
			Vector3 instancePos = transform.position;
			instancePos.x += (proportionalWidth * (i - 2));
			//Make it stand out in the terrain
			instancePos.y += FLOOR_LEVEL;
			Instantiate(lanePrefab, instancePos, Quaternion.identity);
		}
		this.SpawnObstacles();
	}


	private void SpawnObstacles() {
		//Get the upper left position of the terrain
		float spaceBetweenLanes = transform.localScale.x / LANE_COUNT;
		float centerSpace = spaceBetweenLanes / 2f;
		float leftCorner = transform.position.x - (spaceBetweenLanes * (LANE_COUNT / 2f));
		Vector3 upperLeft = new Vector3(leftCorner, transform.position.y, transform.position.z + (transform.localScale.z / 2f));
		//Spawn them in a for loop
		int skippedPosition = this.GetSkippedPosition();
		const int STEP = 2;
		for (int i = 1; i < LANE_COUNT * 2; i += STEP) {
			//If skipped position == 4, spawn a cosmic ray
			if (skippedPosition == 4) {
				this.SpawnCosmicRay();
			}
			bool shouldSkip = (int)(i / STEP) == skippedPosition;
			if (shouldSkip) continue;
			//Otherwise, spawn the object
			//Keep a reference, or destroy once they pass a certain threshold
			this.SpawnObstacleAt(upperLeft, centerSpace, spaceBetweenLanes, i);
		}
	}

	private int GetSkippedPosition() {
		const int COSMIC_RAY_THRESHOLD = 9;
		int cosmicRayProbability = Random.Range(0, 11);
		if (cosmicRayProbability > COSMIC_RAY_THRESHOLD) {
			return 4;
		}
		return Random.Range(0, 4);
	}

	private GameObject SpawnObstacleAt(Vector3 initialPos, float centerSpace, float spaceBetweenLanes, int index) {
		Vector3 center = initialPos;
		center.x = initialPos.x + (centerSpace * index);
		center.y += obstaclePrefab.transform.localScale.y;
		GameObject instance = Instantiate(obstaclePrefab, center, Quaternion.identity);
		Vector3 scale = instance.transform.localScale;
		//Give it a little space to breathe
		scale.x = spaceBetweenLanes - (lanePrefab.transform.localScale.x * 10f);
		instance.transform.localScale = scale;
		return instance;
	}

	private void SpawnCosmicRay() {
		Debug.Log($"We have a ray!!");
	}

}
