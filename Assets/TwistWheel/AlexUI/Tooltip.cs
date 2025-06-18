using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using AlexUI.Tooltip;

public class Tooltip : MonoBehaviour, IPointerMoveHandler, IPointerExitHandler {
  [SerializeField]
  private GameObject _represent;

  public string signal;

  //public GameObject represenGameObject => _represent == null ? this == null ? null : gameObject : _represent;

  public GameObject represenGameObject => _represent == null ? gameObject : _represent;

  public void OnPointerMove(PointerEventData eventData) {
    TooltipManager.InstanceNotNull.OnTooltipPointerMove(this);
  }

  public void OnPointerExit(PointerEventData eventData) {
    TooltipManager.InstanceNotNull.OnTooltipPointerOut(this);
  }

  public interface ISignalHandler {
    void OnTooltipSignalIn(string signal, GameObject gameObject);
    void OnTooltipSignalOut(string signal, GameObject gameObject);
  }

  public static void RegisterHandler(ISignalHandler handler) {
    TooltipManager.InstanceNotNull.RegisterHandler(handler);
  }
  public static void CanelHandler(ISignalHandler handler) {
    TooltipManager.InstanceNotNull.CancelHandler(handler);
  }
}
