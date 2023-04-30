using System;
using UnityEngine;

public static class EntityFetcher {

	public static GameManager s_GameManager { get; private set; }
	public static TerrainSplit s_TerrainSplit { get; private set; }
	public static GameObject s_Player { get; private set; }
	public static Camera s_MainCamera { get; private set; }
	static EntityFetcher() {
		s_GameManager = FetchWithTag<GameManager>("GameManager");
		s_TerrainSplit = FetchWithTag<TerrainSplit>("Terrain");
		s_Player = FetchWithTag("Player");
		s_MainCamera = Camera.main;
	}

	private static T FetchWithTag<T>(string tag) {
		var obj = GameObject.FindGameObjectWithTag(tag);
		if (obj == null) {
			Debug.LogError($"Error trying to get {tag}, object with this tag does not exist in the active scene!");
			return default(T);
		}
		return obj.GetComponent<T>();
	}

	private static GameObject FetchWithTag(string tag) {
		var obj = GameObject.FindGameObjectWithTag(tag);
		if (obj == null) {
			Debug.LogError($"Error trying to get {tag}, object with this tag does not exist in the active scene!");
			return null;
		}
		return obj;
	}

}
