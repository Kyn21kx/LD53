using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LibPdInstance))]
public class MusicManager : MonoBehaviour {

    private LibPdInstance pdInstance;
	private TerrainSplit obstacleGenerator;
	private List<int> spawnBuffer;

	private void Start() {
		this.spawnBuffer = new List<int>(TerrainSplit.LANE_COUNT);
		this.obstacleGenerator = EntityFetcher.s_TerrainSplit;
		this.pdInstance = GetComponent<LibPdInstance>();
		this.pdInstance.Bind("obstacles");
	}

	public void OnReceiveFloat(string name, float value) {
		Debug.Log($"Event received from patch! Name: {name}, Value: {value}");
		//Call the obstacles
		this.AddToBuffer((int)value);
	}

	private void AddToBuffer(int value) {
		this.spawnBuffer.Add(value);
		//If we hit 4, spawn the obstacles, and clear the buffer
		if (this.spawnBuffer.Count >= TerrainSplit.LANE_COUNT) {
			this.obstacleGenerator.SpawnObstacles(ObstacleType.Basic, this.spawnBuffer.ToArray());
			this.spawnBuffer.Clear();
		}
	}

}
