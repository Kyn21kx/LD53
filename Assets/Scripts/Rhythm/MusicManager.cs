using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PureDataEvent {
	Obstacles,
	Bpm,
	StartStop,
	ParryCheck,
	ParryResult
};

[RequireComponent(typeof(LibPdInstance))]
public class MusicManager : MonoBehaviour {

	private readonly Dictionary<string, PureDataEvent> eventDictionary = new Dictionary<string, PureDataEvent> {
		{ "obstacles", PureDataEvent.Obstacles },
		{ "bpm", PureDataEvent.Bpm },
		{ "start-stop", PureDataEvent.StartStop },
		{ "parry-check", PureDataEvent.ParryCheck }, //Just send a bang
		{ "parry-result", PureDataEvent.ParryResult },

	};

	[SerializeField]
	private float bpm;

    private LibPdInstance pdInstance;
	private TerrainSplit obstacleGenerator;
	private Parry parryRef;
	private List<int> spawnBuffer;

	private void Start() {
		GameObject player = EntityFetcher.s_Player;
		this.parryRef = player.GetComponent<Parry>();
		this.spawnBuffer = new List<int>(TerrainSplit.LANE_COUNT);
		this.obstacleGenerator = EntityFetcher.s_TerrainSplit;
		this.pdInstance = GetComponent<LibPdInstance>();
		foreach (var key in this.eventDictionary.Keys) {
			this.pdInstance.Bind(key);
		} 
	}

	public void OnReceiveFloat(string name, float value) {
		Debug.Log($"Event received from patch! Name: {name}, Value: {value}");
		
		PureDataEvent e = this.ValidateEventOrThrow(name);

		switch (e) {
			case PureDataEvent.Obstacles:
				//Call the obstacles
				this.AddToBuffer((int)value);
				break;
			case PureDataEvent.Bpm:
				break;
			case PureDataEvent.StartStop:
				break;
			case PureDataEvent.ParryResult:
				this.ConvertAndSendParryInfo((int)value);
				break;
			case PureDataEvent.ParryCheck:
				throw new System.Exception($"Parry check is a send-only event, but received {value} instead!");
			default:
				throw new System.NotImplementedException($"The event {e} has not been implemented!");
		}
	}

	private void AddToBuffer(int value) {
		this.spawnBuffer.Add(value);
		//If we hit 4, spawn the obstacles, and clear the buffer
		if (this.spawnBuffer.Count >= TerrainSplit.LANE_COUNT) {
			this.obstacleGenerator.SpawnObstacles(ObstacleType.Basic, this.spawnBuffer.ToArray());
			this.spawnBuffer.Clear();
		}
	}

	private void ConvertAndSendParryInfo(int value) {
		//Convert the value into the proper data type
		ParryResult actualResult = (ParryResult)value;
		this.parryRef.SendParryResult(actualResult);
	}

	private PureDataEvent ValidateEventOrThrow(string name) {
		bool validEvent = this.eventDictionary.TryGetValue(name, out PureDataEvent e);
		if (!validEvent) throw new System.Exception($"The event with name {name} was not binded to the instance of pure data");
		return e;
	}

}
