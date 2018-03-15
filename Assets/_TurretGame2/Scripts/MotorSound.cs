
using UnityEngine;

public class MotorSound : MonoBehaviour {

	public Transform movable;

	public float min = 0.1f;
	public float max = 0.2f;
	public float angleInfluenceMultiplier = 100;

	private AudioSource source;
	private Vector3 lastForward;

	// Use this for initialization
	void Awake () {
		source = GetComponent<AudioSource>();
		source.volume = 0;

		lastForward = movable.forward;
	}
	
	// Update is called once per frame
	void Update () {
		float distance = Vector3.Dot(lastForward, movable.forward);

		source.pitch = Mathf.Min (min + (1 - distance) * angleInfluenceMultiplier, max);
		source.volume += 0.01f;

		lastForward = movable.forward;
	}
}
