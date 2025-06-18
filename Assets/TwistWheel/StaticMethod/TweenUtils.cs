using UnityEngine;
#if DOTWEEN
using DG.Tweening;
#endif

public static class TweenUtils {
#if DOTWEEN
	public static Tween FadeUIObjViaCanvasGroup(this GameObject gameObject, float time, float alpha = 0) {
		var rect = gameObject.transform as RectTransform;
		Debug.Assert(rect != null, "it seems not a ui obj");
		var canvasGroup = gameObject.GetComponent<CanvasGroup>();
		if (canvasGroup == null) {
			canvasGroup = gameObject.AddComponent<CanvasGroup>();
		}
		var tween = canvasGroup.DOFade(alpha, time);
		return tween;
	}
#endif
}
