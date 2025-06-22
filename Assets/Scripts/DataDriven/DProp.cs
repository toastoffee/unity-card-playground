using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using DProp;

public class DProp<V> {
  private V m_value;
  private List<DBinder<V>> m_binders = new();
  private int m_lastSeqNum;
  private Action updateAction;
  public V value {
    get => m_value;
    set {
      m_value = value;
      NotifyUpdate();
    }
  }
  public void Bind(DBinder<V> binder) {
    if (m_binders == null) {
      m_binders = new List<DBinder<V>>();
    }
    m_binders.Add(binder);
  }
  public void NotifyUpdate() {
    if (m_lastSeqNum != DPropManager.Instance.NotNull().seqNum) {
      updateAction ??= _NotifyBinderUpdate;
      m_lastSeqNum = DPropManager.Instance.AddToUpdateList(updateAction);
    }
  }

  private void _NotifyBinderUpdate() {
    m_binders.RemoveAll(x => !x.IsBinderAlive());
    foreach (var binder in m_binders) {
      binder.OnValueChange(m_value);
    }
  }
}

public interface DBinder<V> {
  bool IsBinderAlive();
  void OnValueChange(V val);
}

[Serializable]
public abstract class CompHolder<Comp> where Comp : UnityEngine.Object {
  [SerializeField]
  protected Comp comp;

  public void SetComp(Comp comp) {
    this.comp = comp;
  }
}

public abstract class HoldCompBinder<TValue, Comp> : CompHolder<Comp>, DBinder<TValue> where Comp : UnityEngine.Object {
  protected virtual Object GetHost() => comp;
  public bool IsBinderAlive() {
    return GetHost() != null;
  }
  public abstract void OnValueChange(TValue val);
}

public abstract class MonoBinder<TValue> : MonoBehaviour, DBinder<TValue> {
  public bool IsBinderAlive() => this != null;
  public abstract void OnValueChange(TValue val);
}
