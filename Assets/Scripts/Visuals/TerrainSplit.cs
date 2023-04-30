using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObstacleType {
	Basic,
	Interactable
}

public class TerrainSplit : MonoBehaviour {
	
	public const int LANE_COUNT = 4;
	private const float FLOOR_LEVEL = 0.6f;

	public float SpaceBetweenLanes => transform.localScale.x / LANE_COUNT;
	public float CenterSpace => SpaceBetweenLanes / 2f;
	
	[SerializeField]
	private GameObject lanePrefab;
	[SerializeField]
	private GameObject obstaclePrefab;
    [SerializeField]
    private GameObject interactableObstaclePrefab;
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
		this.SpawnObstacles(this.obstaclePrefab);
	}

    public void SpawnObstacles(ObstacleType type)
    {
        switch (type)
        {
            case ObstacleType.Basic:
                this.SpawnObstacles(this.obstaclePrefab);
                break;
            case ObstacleType.Interactable:
                if (Random.Range(0, 2) == 0)
                {
                    // Spawn interactable obstacle
                    Vector3 spawnPos = new Vector3(
                        transform.position.x + (Random.Range(-1, 2) * CenterSpace),
                        transform.position.y + obstaclePrefab.transform.localScale.y,
                        transform.position.z + (transform.localScale.z / 2f)
                    );
                    Instantiate(interactableObstaclePrefab, spawnPos, Quaternion.identity);
                }
                break;
            default:
                throw new System.NotImplementedException($"Obstacle of type {type} has not been implemented");
        }
    }


    private void SpawnObstacles(GameObject obstacle) {
		//Get the upper left position of the terrain
		float leftCorner = transform.position.x - (SpaceBetweenLanes * (LANE_COUNT / 2f));
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
            if (Random.Range(0, 2) == 1)
            {
                this.SpawnObstacleAt(obstacle, upperLeft, CenterSpace, SpaceBetweenLanes, i);
            }
            else
            {
                GameObject interactableObstacle = Instantiate(interactableObstaclePrefab, upperLeft, Quaternion.identity);
                interactableObstacle.GetComponent<InteractableObstacle>().points = Random.Range(1, 5);
                interactableObstacle.transform.position += new Vector3(CenterSpace * (i - 2), 0f, 0f);
            }
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

	private void SpawnCosmicRay() {
		Debug.Log($"We have a ray!!");
	}

}
