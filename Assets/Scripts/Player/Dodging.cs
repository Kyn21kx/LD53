using Auxiliars;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Dodging : MonoBehaviour  {

    private const int LEFT = -1;
    private const int RIGHT = 1;
    //Can change movement speed in Unity just 
    //set it to something random for now
    [SerializeField] 
    private float moveSpeed = 4f;

    public int LaneIndex { get; private set; }
    public List<Vector3> LanePositions { get; private set; }
    private TerrainSplit terrainInfo;

    private float DashDuration => this.terrainInfo.CenterSpace / this.moveSpeed;
    private int dashDirection = 0;
    private Rigidbody rig;

    SpartanTimer dashTimer;


    private void Start() {
        this.dashDirection = 0;
        this.dashTimer = new SpartanTimer(TimeMode.Framed);
        this.rig = GetComponent<Rigidbody>();
        this.terrainInfo = EntityFetcher.s_TerrainSplit;
        this.LanePositions = new List<Vector3>(TerrainSplit.LANE_COUNT);
        this.AddLanePositions();
        this.LaneIndex = Random.Range(0, LanePositions.Count);
        this.PlacePlayer(LaneIndex, 0f, true);
    }

    private void AddLanePositions() {
        //Do the same thing we did for the spawnCubes
        //Get lowerLeft corner
        float leftCorner = terrainInfo.transform.position.x - (terrainInfo.SpaceBetweenLanes * (TerrainSplit.LANE_COUNT / 2f));
        float lowerPart = terrainInfo.transform.position.z - (terrainInfo.transform.localScale.z / 2f);
        Vector3 initialPos = new Vector3(leftCorner, terrainInfo.transform.position.y, lowerPart);
        //Spawn them in a for loop
        const int STEP = 2;
        for (int i = 1; i < TerrainSplit.LANE_COUNT * 2; i += STEP) {
            Vector3 center = initialPos;
            center.x = initialPos.x + (terrainInfo.CenterSpace * i);
			this.LanePositions.Add(center);
            Debug.Log($"Lane position #{LanePositions.Count - 1}: {this.LanePositions[LanePositions.Count - 1]}");
        }
    }

    private void Update() {
        this.HandleInput();
        if (SentDashSignal() && !this.dashTimer.Started) {
            this.dashTimer.Reset();
		}
    }

	private void FixedUpdate() {
        if (this.dashTimer.Started) {
            Dash(Time.fixedDeltaTime);
		}
	}

	private void HandleInput () {
        if (this.dashTimer.Started) return;
        if (Input.GetKeyDown(KeyCode.A)) {
            this.dashDirection = LEFT;
        }
        else if (Input.GetKeyDown(KeyCode.D)) {
            this.dashDirection = RIGHT;
        }
        else {
            this.dashDirection = 0;
		}
        this.UpdateLaneIndex(this.LaneIndex + this.dashDirection);
    }

    private void Dash(float timeStep)  {
        float currTime = this.dashTimer.GetCurrentTime(TimeScaleMode.Seconds);
        System.Console.WriteLine($"Current dashing time: {currTime}");

        Vector3 targetPos = this.PlacePlayer(LaneIndex, timeStep * this.moveSpeed);
        if (currTime >= this.DashDuration || SpartanMath.ArrivedAt(this.rig.position, targetPos)) {
            this.dashTimer.Stop();
            //cooldownTimer.Reset();
            //We stopped the dash
            return;
        }
    }

    private bool SentDashSignal() {
        return this.dashDirection != 0f;
    }

    private void UpdateLaneIndex(int index) {
        this.LaneIndex = Mathf.Clamp(index, 0, this.LanePositions.Count - 1);
    }

    private Vector3 PlacePlayer(int index, float speed, bool isInstant = false) {
        Vector3 targetPos = this.LanePositions[index];
        targetPos.y = 0f;
        const float DEPTH_TOLERANCE = 0.5f;
        targetPos.z += this.transform.localScale.z + DEPTH_TOLERANCE;
        if (isInstant) {
            this.rig.position = targetPos;
        }
        this.rig.position = Vector3.Lerp(this.rig.position, targetPos, speed);
        return targetPos;
	}

}


