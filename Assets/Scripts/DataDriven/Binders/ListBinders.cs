using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SimpleListBinder<T> : HoldCompBinder<List<DProp<T>>, GameObject> {
  public class BinderCache {
    public DBinder<T> binder;
    public GameObject gameObject;
  }
  public struct BinderCreateInput {
    public int idx;
    public T value;
    public GameObject inst;
  }

  [SerializeField]
  private GameObject _prefab;
  public Func<BinderCreateInput, DBinder<T>> binderCreateHandler;

  private List<BinderCache> m_binderCaches = new List<BinderCache>();

  public override void OnValueChange(List<DProp<T>> val) {
    var require = val.SafeCount();
    for (int i = 0; i < m_binderCaches.Count; i++) {
      SetCacheActive(m_binderCaches[i], i < require, i, val[i]);
      if (i < require) {
        m_binderCaches[i].binder.OnValueChange(val[i].value);
      }
    }
    for (int i = m_binderCaches.Count; i < require; i++) {
      m_binderCaches.Add(new BinderCache());
      SetCacheActive(m_binderCaches[i], true, i, val[i]);
      m_binderCaches[i].binder.OnValueChange(val[i].value);
    }
  }

  private void SetCacheActive(BinderCache cache, bool active, int idx, DProp<T> prop) {
    if (cache.gameObject == null) {
      var binderInst = GameObject.Instantiate(_prefab, comp.transform);
      cache.gameObject = binderInst.gameObject;
      cache.binder = binderCreateHandler?.Invoke(new BinderCreateInput {
        idx = idx,
        inst = binderInst,
        value = prop.value,
      }) ?? default;
      if (cache.binder != null) {
        prop.Bind(cache.binder);
      }
    }
    cache.gameObject.SetActive(active);
  }
}