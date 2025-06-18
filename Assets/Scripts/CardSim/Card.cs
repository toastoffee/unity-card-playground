using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Text = TMPro.TMP_Text;
using Sirenix.OdinInspector;

public class Card : MonoBehaviour {
	public static int sEntityId;

	[System.Serializable]
	public class Data {
		public string name;
		public string desc;
		public int cost;
	}
	[HideLabel]
	public Data data;
	public Text nameTxt;
	public Text descTxt;
	public Text costTxt;
	public Text entityIdTxt;
	public int entityId;

	public void Awake() {
		entityId = sEntityId++;
	}

	public void Start() {
		Refresh();
	}
	public void Refresh() {
		nameTxt.text = string.Format($"{data.name}[{entityId}]");
		descTxt.text = data.desc;
		costTxt.text = data.cost.ToString();
		entityIdTxt.text = entityId.ToString();
	}

	public void EventOnPlayCard() {
		PlayGroundController.Instance.PlayCard(transform);
	}
}
