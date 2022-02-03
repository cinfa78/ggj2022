using System;
using UnityEngine;

namespace TheDay {
	[Serializable]public abstract class Action : MonoBehaviour {
		public float delay;
		public virtual void Execute(GameObject caller) { }
	}
}