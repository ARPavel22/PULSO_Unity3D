using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SmoothCamera : MonoBehaviour {
	
	public int smoothingFrames = 10;
	//private VuforiaBehaviour qcarBehavior;
	
	private Quaternion smoothedRotation;
	private Vector3 smoothedPosition;
	
	private Queue<Quaternion> rotations;
	private Queue<Vector3> positions;

    public Transform aim;
	
	public void OnInitialized() {
	}


    public void Update()
    {
        OnTrackablesUpdated();
    }
	
	public void OnTrackablesUpdated() {
		if (rotations.Count >= smoothingFrames) {
			rotations.Dequeue();
			positions.Dequeue();
		}
		
		rotations.Enqueue(aim.rotation);
		positions.Enqueue(aim.position);
		
		Vector4 avgr = Vector4.zero;
		foreach (Quaternion singleRotation in rotations) {
			Math3d.AverageQuaternion(ref avgr, singleRotation, rotations.Peek(), rotations.Count);
		}
		
		Vector3 avgp = Vector3.zero;
		foreach (Vector3 singlePosition in positions) {
			avgp += singlePosition;
		}
		avgp /= positions.Count;
		
		smoothedRotation = new Quaternion(avgr.x, avgr.y, avgr.z, avgr.w);
		smoothedPosition = avgp;
	}
	
	// Use this for initialization
	void Start () {
		rotations = new Queue<Quaternion>(smoothingFrames);
		positions = new Queue<Vector3>(smoothingFrames);
		//qcarBehavior = GetComponent<VuforiaBehaviour>();
		//qcarBehavior.RegisterTrackerEventHandler(this);
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
		transform.rotation = smoothedRotation;
		transform.position = smoothedPosition;
	}
	
}