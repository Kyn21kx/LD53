﻿using System;
using UnityEngine;

public static class EntityFetcher {

	public static GameManager s_GameManager { get; private set; }

	static EntityFetcher() {
		s_GameManager = FetchWithTag<GameManager>("GameManager");
	}

	private static T FetchWithTag<T>(string tag) {
		var obj = GameObject.FindGameObjectWithTag(tag);
		if (obj == null) {
			Debug.LogError($"Error trying to get {tag}, object with this tag does not exist in the active scene!");
			return default(T);
		}
		return obj.GetComponent<T>();
	}

}