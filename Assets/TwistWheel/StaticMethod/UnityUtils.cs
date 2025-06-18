using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
public static class UnityUtils {
  public static bool IsCShrapNull(this Object uobj) {
    return ((object)uobj) == null;
  }
  public static bool IsNullOrUObjectNull(this object obj) {
    if (obj == null) {
      return true;
    }

    if (obj is UnityEngine.Object uobj) {
      return uobj == null;
    }

    if (obj is UnityEngine.MonoBehaviour mono) {
      return mono == null;
    }
    return false;
  }

  public static IEnumerable<T> SafeUObjects<T>(this T[] objArr) where T : class {
    for (int i = 0, count = objArr.SafeCount(); i < count; i++) {
      if (objArr[i].IsNullOrUObjectNull()) {
        continue;
      }
      yield return objArr[i] as T;
    }
  }

  public static void RemoveNullUObjects<T>(this IList<T> list) {
    if (list == null) {
      return;
    }
    for (int i = 0; i < list.Count; i++) {
      if (list[i].IsNullOrUObjectNull()) {
        list.RemoveAt(i);
        i--;
      }
    }
  }

  public static bool TryFindParentWithTag(this GameObject go, string tag, out GameObject res) {
    res = FindParentWithTag(go, tag);
    return res != null;
  }

  public static GameObject FindParentWithTag(this GameObject go, string tag) {
    if (go == null) {
      return null;
    }

    var parent = go.transform;
    while (parent != null) {
      if (parent.tag == tag) {
        return parent.gameObject;
      }
      parent = parent.parent;
    }

    return null;
  }

  public static bool TryFindParentWithTag(this Component component, string tag, out GameObject res) {
    res = FindParentWithTag(component, tag);
    return res != null;
  }

  public static GameObject FindParentWithTag(this Component component, string tag) {
    if (component == null) {
      return null;
    }
    return component.gameObject.FindParentWithTag(tag);
  }

  public static void SetLocalScaleX(this Transform transform, float x) {
    var cur = transform.localScale;
    cur.x = x;
    transform.localScale = cur;
  }

  public static void SetLocalScaleY(this Transform transform, float y) {
    var cur = transform.localScale;
    cur.y = y;
    transform.localScale = cur;
  }

  public static void SetLocalScaleZ(this Transform transform, float z) {
    var cur = transform.localScale;
    cur.z = z;
    transform.localScale = cur;
  }

  public static Vector2 RandomWithin(this BoxCollider2D box) {
    var position = (Vector2)box.transform.position;
    var xRange = new Vector2(position.x + box.offset.x - box.size.x / 2, position.x + box.offset.x + box.size.x / 2);
    var yRange = new Vector2(position.y + box.offset.y - box.size.y / 2, position.y + box.offset.y + box.size.y / 2);

    var rand = new Vector2(Random.Range(xRange.x, xRange.y), Random.Range(yRange.x, yRange.y));
    return rand;
  }

  public static void ClampedForce(this Rigidbody2D rb, Vector2 dir, float maxSpeed, float force) {
    dir = dir.normalized;
    maxSpeed = Mathf.Max(maxSpeed, 0);
    var cur = rb.velocity;
    float dot = Vector2.Dot(cur, dir.normalized);

    if (dot < maxSpeed) {
      rb.AddForce(force * dir);
    }
  }

  public static Vector3 rbForward(this Rigidbody rb) {
    return rb.rotation * Vector3.forward;
  }

  public static Vector3 rbUp(this Rigidbody rb) {
    return rb.rotation * Vector3.up;
  }

  public static List<Transform> FindChildren(this Transform transform, string name) {
    var res = new List<Transform>();
    foreach (Transform child in transform) {
      if (child.name.ToLower() == name.ToLower()) {
        res.Add(child);
      }
    }
    return res;
  }

  public static Quaternion ToQuaternion(this Vector3 vec) {
    return Quaternion.Euler(vec.x, vec.y, vec.z);
  }

  public static T NotNull<T>(this T obj) {
#if !UNITY_EDITOR
    return obj;
#else
    if (obj.IsNullOrUObjectNull()) {
      Debug.LogError($"[NotNull] 类型为【{typeof(T)}】的对象为空");
    }
    return obj;
#endif
  }

  public static Vector2 ClampByRect(this Vector2 vec, Rect rect) {
    vec.x = Mathf.Clamp(vec.x, rect.xMin, rect.xMax);
    vec.y = Mathf.Clamp(vec.y, rect.yMin, rect.yMax);
    return vec;
  }

  public static void DestroyAllChildren(this Transform transform) {
    List<Transform> children = new();
    for (int i = 0; i < transform.childCount; i++) {
      children.Add(transform.GetChild(i));
    }

    foreach (var child in children) {
      GameObject.Destroy(child.gameObject);
    }
  }

  public static string LogObjectScenePath(this GameObject obj) {
    var sb = new System.Text.StringBuilder();
    while (obj != null) {
      sb.Append(obj.name);
      sb.Append(" ->");
      obj = obj.transform.parent == null ? null : obj.transform.parent.gameObject;
    }
    if (sb.Length > 0) {
      return sb.ToString();
    }
    return string.Empty;
  }

  public static Rect GetScreenRect(this RectTransform rt) {
    var rect = rt.rect;
    rect.position = rt.localToWorldMatrix.MultiplyPoint(rect.position);
    rect.size = rt.localToWorldMatrix.MultiplyVector(rect.size);
    return rect;
  }

  public static void AdjustCount<T>(this List<T> comps, Transform container, T prefab, int count, System.Action<int, T> initer = null, System.Action<int, T> disposer = null) where T : Component {
    for (int i = 0; i < comps.SafeCount(); i++) {
      var comp = comps[i];
      if (disposer != null) {
        disposer(i, comp);
      }
      GameObject.Destroy(comp.gameObject);
    }
    comps.Clear();
    for (int i = 0; i < count; i++) {
      comps.Add(GameObject.Instantiate(prefab, container));
      if (initer != null) {
        initer(comps.Count - 1, comps[comps.Count - 1]);
      }
    }
  }

  public static Dictionary<K, V> BuildDictFromChildrenComp<K, V>(this GameObject obj, Func<V, K> value2key) where V : Component {
    var comps = obj.GetComponentInChildren<V>(true) as V[];
    var ret = new Dictionary<K, V>();
    foreach (var comp in comps) {
      var key = value2key(comp);
      ret[key] = comp;
    }
    return ret;
  }


  private static float[] closetRectBuffer = new float[4];
  public static Vector3 ClosestPointOnRectEdge(this Rect rect, Vector3 pos) {
    // 将输入坐标转换为2D坐标系
    Vector2 point = new Vector2(pos.x, pos.y);

    // 计算到四条边的最近点
    Vector2 leftEdgePoint = new Vector2(rect.xMin, Mathf.Clamp(point.y, rect.yMin, rect.yMax));
    Vector2 rightEdgePoint = new Vector2(rect.xMax, Mathf.Clamp(point.y, rect.yMin, rect.yMax));
    Vector2 bottomEdgePoint = new Vector2(Mathf.Clamp(point.x, rect.xMin, rect.xMax), rect.yMin);
    Vector2 topEdgePoint = new Vector2(Mathf.Clamp(point.x, rect.xMin, rect.xMax), rect.yMax);

    closetRectBuffer[0] = Vector2.Distance(point, leftEdgePoint);
    closetRectBuffer[1] = Vector2.Distance(point, rightEdgePoint);
    closetRectBuffer[2] = Vector2.Distance(point, bottomEdgePoint);
    closetRectBuffer[3] = Vector2.Distance(point, topEdgePoint);

    // 找出最小距离的索引
    int minIndex = 0;
    for (int i = 1; i < closetRectBuffer.Length; i++) {
      if (closetRectBuffer[i] < closetRectBuffer[minIndex]) {
        minIndex = i;
      }
    }

    // 返回最近点（转换为Vector3）
    switch (minIndex) {
      case 0: return leftEdgePoint;
      case 1: return rightEdgePoint;
      case 2: return bottomEdgePoint;
      case 3: return topEdgePoint;
      default: return Vector3.zero;
    }
  }

  private static Vector2[] cornersA = new Vector2[4];
  private static Vector2[] cornersB = new Vector2[4];
  public static float DistanceToRect(this Rect rectA, Rect rectB) {
    // 检测是否存在重叠（若重叠则距离为0）
    if (rectA.Overlaps(rectB)) return 0f;

    // 计算水平方向间距
    float horizontalGap = Mathf.Max(
        rectB.xMin - rectA.xMax,
        rectA.xMin - rectB.xMax,
        0f
    );

    //Debug.Log($"bMin {rectB.xMin} aMax {rectA.xMax}  aMin {rectA.xMin} bMax {rectB.xMax}");

    // 计算垂直方向间距
    float verticalGap = Mathf.Max(
        rectB.yMin - rectA.yMax,
        rectA.yMin - rectB.yMax,
        0f
    );
    // 对角线距离（勾股定理）
    return Mathf.Sqrt(horizontalGap * horizontalGap + verticalGap * verticalGap);
  }
}
public struct ComponentFinder<T> where T : Component {
  private T m_cache;
  public T Get(Component comp) {
    if (m_cache == null) {
      while (comp != null && !comp.TryGetComponent(out m_cache)) {
        comp = comp.transform.parent;
      }
    }
    return m_cache;
  }
}

public struct SceneSingletonFinder<T> where T : Object {
  private T m_cache;
  public T Get() {
    if (m_cache == null) {
      var find = Object.FindAnyObjectByType<T>();
      Debug.Assert(find != null, $"SceneObjectFinder<{typeof(T).Name}> cant't find Object");
      Debug.Assert(m_cache == null || find == m_cache, $"SceneObjectFinder<{typeof(T).Name}> multi object detected");
      m_cache = find;
    }
    return m_cache;
  }
}

public struct ResourcesLoadCache<T> where T : Object {
  private string path;
  private T res;

  public T Res {
    get {
      if (res == null) {
        res = Resources.Load<T>(path);
      }
      return res;
    }
  }

  public ResourcesLoadCache(string path) {
    this.path = path;
    res = null;
  }

}