using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LiteState : MonoBehaviour {
	public virtual void OnEnter() { }
	public virtual void OnExit() { }
	public virtual void OnResume() { }
}


