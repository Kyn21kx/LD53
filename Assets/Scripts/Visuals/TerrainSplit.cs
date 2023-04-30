using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObstacleType {
	Basic,
	Interactable,
	CosmicRay,
}

public class TerrainSplit : MonoBehaviour {
	
	public const int LANE_COUNT = 4;
	private const float FLOOR_LEVEL = 0.6f;
	public float scrollFactor = 0.5f;
	public Vector3 ScaleToWorld => this.transform.localScale * 10f; //Because planes are reduced in scale by 10
	public float SpaceBetweenLanes => ScaleToWorld.x / LANE_COUNT;
	public float CenterSpace => SpaceBetweenLanes / 2f;
	
	[SerializeField]
	private GameObject lanePrefab;
	[SerializeField]
	private GameObject obstaclePrefab;
	[SerializeField]
	private GameObject cosmicRayPrefab;
	private float proportionalWidth;
	[SerializeField]
	private MeshRenderer scrollingVisuals;
	private Material material;

	private void Start() {
		this.material = scrollingVisuals.material;
		//Get the scale and divide into laneCount
		float width = ScaleToWorld.x;
		this.proportionalWidth = width / LANE_COUNT;
		Debug.Log($"ProportionalWidth: {this.proportionalWidth}");
		for (int i = 0; i < LANE_COUNT + 1; i++) {
			Vector3 instancePos = transform.position;
			instancePos.x += (proportionalWidth * (i - 2));
			//Make it stand out in the terrain
			instancePos.y += FLOOR_LEVEL;
			Instantiate(lanePrefab, instancePos, Quaternion.identity);
		}
	}

	private void Update() {
		this.Scroll(Time.deltaTime);
	}

	public void SpawnObstacles(ObstacleType type, int[] spawnFlags) {
		type = type != ObstacleType.CosmicRay && this.IsFilledActive(spawnFlags) ? ObstacleType.CosmicRay : type;
		switch (type) {
			case ObstacleType.Basic:
				this.SpawnObstacles(this.obstaclePrefab, spawnFlags);
				break;
			case ObstacleType.Interactable:
				//Your code that will spawn interactable obstacles
				break;
			case ObstacleType.CosmicRay:
				this.SpawnObstacles(this.cosmicRayPrefab, spawnFlags);
				break;
			default:
				throw new System.NotImplementedException($"Obstacle of type {type} has not been implemented");
		}
	}

	private void SpawnObstacles(GameObject obstacle, int[] spawnFlags) {
		//Get the upper left position of the terrain
		float leftCorner = transform.position.x - (SpaceBetweenLanes * (LANE_COUNT / 2f));
		Vector3 upperLeft = new Vector3(leftCorner, transform.position.y, transform.position.z + (ScaleToWorld.z / 2f));
		//Spawn them in a for loop
		const int STEP = 2;
		for (int i = 1; i < LANE_COUNT * 2; i += STEP) {
			int nativeIndex = (int)(i / STEP);
			bool shouldSkip = spawnFlags[nativeIndex] == 0;
			if (shouldSkip) continue;
			//Otherwise, spawn the object
			//Keep a reference, or destroy once they pass a certain threshold
			this.SpawnObstacleAt(obstacle, upperLeft, CenterSpace, SpaceBetweenLanes, i);
		}
	}

	private bool IsFilledActive(int[] spawnFlags) {
		for (int i = 0; i < spawnFlags.Length; i++) {
			if (spawnFlags[i] == 0) return false;
		}
		return true;
	}
	
	private int GetSkippedPosition() {
		const int COSMIC_RAY_THRESHOLD = 20;
		int cosmicRayProbability = Random.Range(0, 101);
		if (cosmicRayProbability == COSMIC_RAY_THRESHOLD) {
			return 4;
		}
		return Random.Range(0, 4);
	}

	private GameObject SpawnObstacleAt(GameObject obstacle, Vector3 initialPos, float centerSpace, float spaceBetweenLanes, int index) {
		Vector3 center = initialPos;
		center.x = initialPos.x + (centerSpace * index);
		center.y += obstaclePrefab.transform.localScale.y;
		GameObject instance = Instantiate(obstacle, center, Quaternion.identity);
		Vector3 scale = instance.transform.localScale;
		//Give it a little space to breathe
		scale.x = spaceBetweenLanes - (lanePrefab.transform.localScale.x * 10f);
		instance.transform.localScale = scale;
		return instance;
	}

	private void Scroll(float timeStep) {
		Vector2 offset = this.material.mainTextureOffset;
		offset.y -= timeStep * EntityFetcher.s_GameManager.travelSpeed * this.scrollFactor;
		this.material.mainTextureOffset = offset;
	}

}
