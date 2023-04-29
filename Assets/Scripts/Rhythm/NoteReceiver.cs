using UnityEngine;
/// <summary>
/// Example description of the class:
/// - Song is 190s <br/>
/// -Level is 500m <br/>
/// - Velocity is 11.5m / s <br/>
/// Note receiver is at 265m <br/>
/// 
/// So, how do we get the time estimate? <br/>
/// Velocity = d / t <br/>
/// t = d / V <br/>
/// So,
/// t = 265m / 11.5m / s
/// t = 23.043s
/// We have to receive a note at that time + or - the threshold
/// </summary>
public class NoteReceiver : MonoBehaviour {

	/// <summary>
	/// The target time will be determined by the distance of the obstacle in the level, and the total time of the song
	/// </summary>
	private float targetTime;
	private GameManager gameManager; // Needed to get the time of the level

	private void Start() {
		this.gameManager = EntityFetcher.s_GameManager;
	}


}
