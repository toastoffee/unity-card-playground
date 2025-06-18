using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoOnce {
  private bool done;
  public void Action(System.Action action) {
    if (done) {
      return;
    }
    done = true;
    action?.Invoke();
  }
}

