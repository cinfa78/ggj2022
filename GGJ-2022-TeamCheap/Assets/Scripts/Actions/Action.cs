using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheDay {
	public abstract class Action : ScriptableObject {
		public virtual void Execute() { }
	}
}