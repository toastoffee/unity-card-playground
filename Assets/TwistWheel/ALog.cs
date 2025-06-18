using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ALog {
  public static void Log(object obj) {
    var str = obj.ToString();
    str = $"{str}\n [Frame] {Time.frameCount} \n [Time] {System.DateTime.Now}]";
    UnityEngine.Debug.Log(str);
  }
}
