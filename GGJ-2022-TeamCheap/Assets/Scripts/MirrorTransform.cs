using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

[ExecuteInEditMode]
public class MirrorTransform : MonoBehaviour {
	[FormerlySerializedAs("reference")] [FormerlySerializedAs("referenceTransform")]
	public GameObject mirroredObject;

	private void Update() {
		if (mirroredObject != null) {
			transform.rotation = Quaternion.Euler(new Vector3(mirroredObject.transform.rotation.eulerAngles.x * 1, mirroredObject.transform.rotation.eulerAngles.y * -1,
				mirroredObject.transform.rotation.eulerAngles.z * -1));
			transform.position = new Vector3(mirroredObject.transform.position.x * -1, mirroredObject.transform.position.y, mirroredObject.transform.position.z);
		}
	}

	private void OnDrawGizmos() {
		Gizmos.color = Color.gray;
		Gizmos.DrawWireSphere(transform.position, 0.25f);
		if (mirroredObject != null) {
			Gizmos.color = Color.cyan;
			Gizmos.DrawLine(transform.position, mirroredObject.transform.position);
		}
	}

	[Button("Create Mirrored Copy")]
	private void CreateMirroredObject() {
		if (mirroredObject == null) {
			GameObject newGameObject = Instantiate(gameObject);
			newGameObject.transform.parent = transform.parent;
			if (transform.position.x > 0) {
				newGameObject.name = name + " young";
			}
			else {
				newGameObject.name = name + " old";
			}
			newGameObject.transform.localScale = new Vector3(-1, 1, 1);
			newGameObject.transform.position = new Vector3(-newGameObject.transform.position.x, newGameObject.transform.position.y, newGameObject.transform.position.z);

			newGameObject.GetComponent<MirrorTransform>().mirroredObject = gameObject;
			newGameObject.GetComponent<MirrorTransform>().enabled = true;
			mirroredObject = newGameObject;
		}
	}
}