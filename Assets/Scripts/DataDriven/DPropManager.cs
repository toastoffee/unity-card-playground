using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DProp {
  public class DPropManager : MonoBehaviour {
    private static DPropManager m_instance;
    public static DPropManager Instance {
      get {
        if (m_instance == null) {
          m_instance = new GameObject("DPropManager").AddComponent<DPropManager>();
        }
        return m_instance;
      }
    }
    private struct PropUpdate {
      public int seqNum;
      public Action updateAction;
    }
    private int m_seqNum = 1;
    private int m_nextAvailIdx;
    private List<PropUpdate> m_updateList = new();
    public int seqNum => m_seqNum;

    public int AddToUpdateList(Action action) {
      var idx = _GetNextUpdateId();
      m_updateList[idx] = new PropUpdate {
        seqNum = m_seqNum,
        updateAction = action,
      };
      return m_seqNum;
    }

    private void _ExecuteUpdateList() {
      if (m_updateList.Count == 0) return;
      for (int i = 0; i < m_updateList.Count; i++) {
        var update = m_updateList[i];
        if (update.seqNum == m_seqNum) {
          update.updateAction?.Invoke();
        } else {
          break;
        }
      }
      m_seqNum++;
      m_nextAvailIdx = 0; // Reset for next updates
    }

    private void LateUpdate() {
      _ExecuteUpdateList();
    }

    private int _GetNextUpdateId() {
      if (m_nextAvailIdx >= m_updateList.Count) {
        m_updateList.Add(new PropUpdate());
      }
      return m_nextAvailIdx++;
    }
  }
}
