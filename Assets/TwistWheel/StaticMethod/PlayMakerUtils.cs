using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if PLAYMAKER
using PlayMaker;
public static class PlayMakerUtil {
  public static PlayMakerFSM FindFSM(this GameObject obj, string name) {
    var fsms = obj.GetComponents<PlayMakerFSM>();
    for (int i = 0; i < fsms.SafeCount(); i++) {
      var fsm = fsms[i];
      if (fsm.FsmName == name) {
        return fsm;
      }
    }
    return null;
  }
}
#endif