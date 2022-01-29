using UnityEngine;

public abstract class Interactable : MonoBehaviour {
	public virtual void Interact() { }
	public virtual void SecondaryInteract() { }
	public virtual void ShowInteractionAvailable() { }
}