using Sirenix.OdinInspector;
using TheDay;
using UnityEngine;

public abstract class Interactable : MonoBehaviour {
	public virtual void Interact() { }

	public virtual void SecondaryInteract() { }

	public virtual void ShowInteractionAvailable() { }

	[HorizontalGroup("Row1")]
	[Button("Add Sound Action")]
	private void AddSound() {
		gameObject.AddComponent<ActionPlaySound>();
	}

	[HorizontalGroup("Row1")]
	[Button("Add Look At Action")]
	private void AddLookAtAction() {
		gameObject.AddComponent<ActionLookAt>();
	}

	[HorizontalGroup("Row2")]
	[Button("Add Activate Objects Action")]
	private void AddActivateObjects() {
		gameObject.AddComponent<ActionActivate>();
	}

	[HorizontalGroup("Row2")]
	[Button("Add Deactivate Objects Action")]
	private void AddDeactivateObjects() {
		gameObject.AddComponent<ActionDeactivate>();
	}
}