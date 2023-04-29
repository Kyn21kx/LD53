using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainSplit : MonoBehaviour {

	[SerializeField]
	private GameObject lanePrefab;

	private void Start() {
		const int LANE_COUNT = 4;
		//Get the scale and divide into laneCount
		float width = transform.localScale.x;
		float proportionalWidth = width / LANE_COUNT;
		for (int i = 0; i < LANE_COUNT + 1; i++) {
			Vector3 instancePos = transform.position;
			instancePos.x += (proportionalWidth * (i - 2));
			//Make it stand out in the terrain
			instancePos.y += 0.6f;
			Instantiate(lanePrefab, instancePos, Quaternion.identity);
		}
	}

}
