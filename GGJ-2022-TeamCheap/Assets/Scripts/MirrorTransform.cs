using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MirrorTransform : MonoBehaviour {
	[FormerlySerializedAs("referenceTransform")]
	public GameObject reference;

	private void Start() { }

	private void Update() {
		transform.rotation = Quaternion.Euler(new Vector3(reference.transform.rotation.eulerAngles.x, reference.transform.rotation.eulerAngles.y * -1, reference.transform.rotation.eulerAngles.z));
		transform.position = new Vector3(reference.transform.position.x * -1, reference.transform.position.y, reference.transform.position.z);
	}
}