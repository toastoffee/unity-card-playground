using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePopupTextRoot : MonoBehaviour {
	private struct Element {
		public RectTransform rect;
		public int index;
		public float lifeTime;
	}

	public Transform container;
	public float space;
	public float speed;
	public float holdTime;
	public float fadeTime;
	public Vector2 pivot;
	public Vector2 anchorMin;
	public Vector2 anchorMax;

	private int m_lastIndex;
	private List<Element> m_elements = new List<Element>();

	private float GetHeight(Element element) {
		var spaceHeight = (m_lastIndex - element.index) * space;
		var fadeHeight = element.lifeTime < holdTime ? 0 : Mathf.Lerp(0, speed * fadeTime, Mathf.InverseLerp(0, fadeTime, element.lifeTime - fadeTime));
		return spaceHeight + fadeHeight;
	}

	public void AddPopup(RectTransform rectTransform) {
		rectTransform.SetParent(container, worldPositionStays: true);
		rectTransform.anchorMax = anchorMax;
		rectTransform.anchorMin = anchorMin;
		rectTransform.pivot = pivot;

		var element = new Element() {
			rect = rectTransform,
			index = m_lastIndex++,
			lifeTime = 0,
		};
		m_elements.Add(element);

		rectTransform.anchoredPosition = Vector2.zero;
		rectTransform.localScale = Vector3.one;
	}

	private void LateUpdate() {
		for (int i = 0; i < m_elements.Count; i++) {
			var element = m_elements[i];

			var height = GetHeight(element);
			var pos = element.rect.anchoredPosition;
			pos.y = height;
			element.rect.anchoredPosition = pos;
			element.lifeTime += Time.deltaTime;

			if (element.lifeTime >= fadeTime + holdTime) {
				Destroy(element.rect.gameObject);
				m_elements.RemoveAt(i);
				i--;
			} else {
				m_elements[i] = element;
			}
		}
	}
}
