using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LiteState : MonoBehaviour {
	public LiteStateEngine stateEngine {
		get; private set;
	}
	public void InitByStateEngine(LiteStateEngine liteStateEngine) {
		stateEngine = liteStateEngine;
	}
	public virtual void SetStataeActive(bool isActive) {
		gameObject.SetActive(isActive);
	}
	public virtual void OnEnter() { }
	public virtual void OnExit() { }
	public virtual void OnResume() { }
	public virtual void OnPause() { }
}


