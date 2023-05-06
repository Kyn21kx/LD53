using System;
using UnityEngine;

public class EntityFetcher {

	public static GameManager s_GameManager => s_Instance.gameManager;
	public static TerrainSplit s_TerrainSplit => s_Instance.terrainSplit;
	public static GameObject s_Player => s_Instance.player;
	public static Camera s_MainCamera => s_Instance.mainCamera;
	public static Canvas s_Canvas => s_Instance.canvas;

	public static EntityFetcher s_Instance { get; private set; }

	private GameManager gameManager;
	private TerrainSplit terrainSplit;
	private GameObject player;
	private Camera mainCamera;
	private Canvas canvas;
	
	static EntityFetcher() {
		if (s_Instance == null) {
			s_Instance = new EntityFetcher();
		}
		s_Instance.Initialize();
	}

	private static T FetchWithTag<T>(string tag) {
		var obj = GameObject.FindGameObjectWithTag(tag);
		if (obj == null) {
			Debug.LogWarning($"Error trying to get {tag}, object with this tag does not exist in the active scene!");
			return default(T);
		}
		return obj.GetComponent<T>();
	}

	private static GameObject FetchWithTag(string tag) {
		var obj = GameObject.FindGameObjectWithTag(tag);
		if (obj == null) {
			Debug.LogWarning($"Error trying to get {tag}, object with this tag does not exist in the active scene!");
			return null;
		}
		return obj;
	}

	public void Initialize() {
		this.gameManager = FetchWithTag<GameManager>("GameManager");
		this.terrainSplit = FetchWithTag<TerrainSplit>("Terrain");
		this.canvas = FetchWithTag<Canvas>("Canvas");
		this.player = FetchWithTag("Player");
		this.mainCamera = Camera.main;
	}

}
