using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Text = TMPro.TMP_Text;

public class OCounter : MonoBehaviour {
	public Text counterText;
	public Text inputText;

	public int val;

	public void Awake() {
		Refresh();
	}

	public void Refresh() {
		counterText.text = val.ToString();
	}

	public void EventOnInc() {
		val++;
		Refresh();
	}

	public void EventOnDec() {
		val--;
		Refresh();
	}

	public void EventOnSet() {
		var trimed = inputText.text.TrimEnd(inputText.text[inputText.text.Length - 1]);
		var input = System.Convert.ToInt32(trimed);
		val = input;
		Refresh();
	}
}
