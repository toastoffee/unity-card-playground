using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayGroundController : MonoSingleton<PlayGroundController> {
	public RectTransform tokenContainer;
	public RectTransform deckContainer;
	public RectTransform graveyardContainer;

	public void Shuffle(RectTransform transform) {
		Debug.Log($"“—œ¥≈∆: {transform}");
		var children = new List<Transform>();
		for (int i = 0; i < transform.childCount; i++) {
			var child = transform.GetChild(i);
			children.Add(child);
		}
		children.Shuffle();
		for (int i = 0; i < children.Count; i++) {
			children[i].SetSiblingIndex(i);
		}
	}

	public void TransferChilds(Transform from, Transform to) {
		while (from.childCount > 0) {
			var child = from.GetChild(0);
			child.SetParent(to);
		}
	}

	public void EventOnShuffleDeck() {
		Shuffle(deckContainer);
	}

	public void EventOnDraw() {
		if (deckContainer.childCount == 0) {
			Debug.Log("≈∆ø‚“—ø’£¨Œﬁ∑®≥È≈∆");
			return;
		}
		var child = deckContainer.GetChild(0);
		child.SetParent(tokenContainer);
		Debug.Log("“—≥È≈∆: " + child.name);
	}

	public void EventOnShuffleGraveyard() {
		Debug.Log($"∆˙≈∆∂—œ¥ªÿ≥È≈∆∂—");
		Shuffle(graveyardContainer);
		TransferChilds(graveyardContainer, deckContainer);
	}

	public void EventOnDiscardToken() {
		Debug.Log($"∆˙≈∆µΩ∆˙≈∆∂—");
		TransferChilds(tokenContainer, graveyardContainer);
	}

	public void PlayCard(Transform cardTr) {
		Debug.Log($"≥ˆ≈∆£∫{cardTr.name}");
		cardTr.SetParent(graveyardContainer);
	}
}
