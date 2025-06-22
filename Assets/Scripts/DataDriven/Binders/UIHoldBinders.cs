using System;
using TMPro;
using UnityEngine;
using Sirenix.OdinInspector;

[Serializable]
public class TextBinder : HoldCompBinder<string, TMP_Text> {
  public override void OnValueChange(string val) {
    comp.text = val;
  }
}

public abstract class FormatTextBinder<V> : HoldCompBinder<V, TMP_Text> {
  
}

