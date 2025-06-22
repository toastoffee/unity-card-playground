using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RogueMapState : UILiteState {
	public void EventOnClickBattle() {
		stateEngine.AddTop<RogueBattleState>();
	}
}
