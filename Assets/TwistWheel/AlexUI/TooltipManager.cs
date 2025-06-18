using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AlexUI.Tooltip {
  using Tooltip = global::Tooltip;

  public class TooltipManager : MonoSingleton<TooltipManager> {
    private Tooltip current;
    private SceneSingletonFinder<GraphicRaycaster> rayCasterGetter;
    private SceneSingletonFinder<EventSystem> eventSystemGetter;
    private List<RaycastResult> raycastResults = new();

    private List<Tooltip.ISignalHandler> handlers = new();

    protected override void Awake() {
      base.Awake();
    }

    public void OnTooltipPointerMove(Tooltip tooltip) {
      if (tooltip == null || tooltip == current) {
        return;
      }

      _PutDownTooltip(current);
      _PutOnTooltip(tooltip);
      current = tooltip;
    }

    public void OnTooltipPointerOut(Tooltip tooltip) {
      if (current.IsCShrapNull() || tooltip != current) {
        return;
      }
      _PutDownTooltip(current);
      current = null;
    }

    private void _PutDownTooltip(Tooltip tooltip) {
      if (tooltip.IsCShrapNull()) {
        return;
      }
      handlers.RemoveAll(x => x == null);
      foreach (var handler in handlers) {
        handler.OnTooltipSignalOut(tooltip.signal, tooltip.represenGameObject);
      }
    }
    private void _PutOnTooltip(Tooltip tooltip) {
      if (tooltip == null) {
        return;
      }

      handlers.RemoveAll(x => x == null);
      foreach (var handler in handlers) {
        handler.OnTooltipSignalIn(tooltip.signal, tooltip.represenGameObject);
      }
    }
    private void Update() {
      var needPutDown = true;
      if (!current.IsCShrapNull()) {
        do {
          if (current == null) {
            break;
          }
          var screenRect = (current.transform as RectTransform).GetScreenRect();
          if (!screenRect.Contains(Input.mousePosition)) {
            break;
          }

          var rayCaster = rayCasterGetter.Get();
          var es = eventSystemGetter.Get();
          var pointerData = new PointerEventData(es);
          pointerData.position = Input.mousePosition;

          raycastResults.Clear();
          rayCaster.Raycast(pointerData, raycastResults);
          if (raycastResults.Count > 0 && raycastResults[0].gameObject != current.gameObject) {
            break;
          }
          needPutDown = false;
        } while (false);

        if (needPutDown) {
          _PutDownTooltip(current);
          current = null;
        }
      }
    }

    public void RegisterHandler(Tooltip.ISignalHandler handler) {
      handlers.Add(handler);
    }
    public void CancelHandler(Tooltip.ISignalHandler handler) {
      handlers.Remove(handler);
    }
  }
}
