using System;
using System.Collections.Generic;

public struct IfCacheChange<T> where T : IEquatable<T> {
  public T Value => m_value;
  private T m_value;
  private readonly EqualityComparer<T> _comparer;

  public IfCacheChange(T initialValue) {
    m_value = initialValue;
    _comparer = EqualityComparer<T>.Default;
  }

  public bool Check(T newValue) {
    // 使用 EqualityComparer 的 Equals 方法比较值
    if (_comparer.Equals(newValue, m_value)) {
      return false;
    }
    m_value = newValue;
    return true;
  }
}