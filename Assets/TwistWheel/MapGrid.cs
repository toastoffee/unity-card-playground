using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MapGrid<TElement> : MonoBehaviour where TElement : MapGridElement {
  public enum PlaneDir {
    XY,
    XZ,
  }

  [Serializable]
  public class Config {
    public Vector2Int cellCnt;
    public Vector2 cellSize;
    public PlaneDir planeDir = PlaneDir.XY;
  }

  private class Cell {
    public int elementId = -1; // -1表示空
    public Vector2Int position;
  }

  private class Runtime {
    public Config config;
    public Cell[,] cells;
    public Dictionary<int, TElement> elements = new Dictionary<int, TElement>();
    public int nextElementId;
  }

  [SerializeField]
  private Config _config;

  public Config config => _config;

  private Grid m_gridComp;
  private Runtime m_runtime;

  protected virtual void Awake() {
    InitializeGrid();
  }

  private void InitializeGrid() {
    m_gridComp = gameObject.AddComponent<Grid>();

    Vector3 cellSize;
    if (_config.planeDir == PlaneDir.XY) {
      cellSize = new Vector3(_config.cellSize.x, _config.cellSize.y, 0);
    } else {
      cellSize = new Vector3(_config.cellSize.x, 0, _config.cellSize.y);
    }

    m_gridComp.cellSize = cellSize;

    m_runtime = new Runtime {
      config = _config,
      cells = new Cell[_config.cellCnt.x, _config.cellCnt.y],
      nextElementId = 0
    };

    // 初始化所有单元格
    for (int x = 0; x < _config.cellCnt.x; x++) {
      for (int y = 0; y < _config.cellCnt.y; y++) {
        m_runtime.cells[x, y] = new Cell {
          position = new Vector2Int(x, y)
        };
      }
    }
  }

  /// <summary>
  /// 尝试添加元素到网格
  /// </summary>
  public bool TryAddElement(Vector2Int rootPosition, TElement element, out int elementId) {
    elementId = -1;

    // 获取元素实际占用的所有网格坐标
    var occupiedCells = GetOccupiedCells(rootPosition, element);

    // 检查所有单元格是否有效
    if (!ValidateCells(occupiedCells)) return false;

    // 分配元素ID并记录
    elementId = m_runtime.nextElementId++;
    m_runtime.elements.Add(elementId, element);

    // 标记单元格占用
    foreach (var cell in occupiedCells) {
      m_runtime.cells[cell.x, cell.y].elementId = elementId;
    }

    return true;
  }

  /// <summary>
  /// 移除指定元素
  /// </summary>
  public void RemoveElement(int elementId) {
    if (!m_runtime.elements.ContainsKey(elementId)) return;

    // 清除所有关联的单元格
    for (int x = 0; x < m_runtime.config.cellCnt.x; x++) {
      for (int y = 0; y < m_runtime.config.cellCnt.y; y++) {
        if (m_runtime.cells[x, y].elementId == elementId) {
          m_runtime.cells[x, y].elementId = -1;
        }
      }
    }

    m_runtime.elements.Remove(elementId);
  }

  /// <summary>
  /// 检查指定位置是否可放置元素
  /// </summary>
  public bool CheckPlacementValid(Vector2Int rootPosition, TElement element) {
    return ValidateCells(GetOccupiedCells(rootPosition, element));
  }

  /// <summary>
  /// 获取指定单元格上的元素
  /// </summary>
  public TElement GetElementAtCell(Vector2Int cellPosition) {
    if (!IsCellValid(cellPosition)) return null;
    var elementId = m_runtime.cells[cellPosition.x, cellPosition.y].elementId;
    return elementId != -1 ? m_runtime.elements[elementId] : null;
  }

  /// <summary>
  /// 世界坐标转网格坐标
  /// </summary>
  public Vector2Int WorldToCellPosition(Vector3 worldPosition) {
    Vector3Int cellPos = m_gridComp.WorldToCell(worldPosition);
    if (config.planeDir == PlaneDir.XY) {
      return new Vector2Int(cellPos.x, cellPos.y);
    } else {
      return new Vector2Int(cellPos.x, cellPos.z);
    }
  }

  /// <summary>
  /// 网格坐标转世界坐标（返回单元格中心点）
  /// </summary>
  public Vector3 CellToWorldPosition(Vector2Int cellPosition) {
    var corrected = config.planeDir == PlaneDir.XY ? (Vector3Int)cellPosition : new Vector3Int(cellPosition.x, 0, cellPosition.y);
    return m_gridComp.CellToWorld(corrected) + m_gridComp.cellSize / 2f;
  }

  private IEnumerable<Vector2Int> GetOccupiedCells(Vector2Int rootPosition, TElement element) {
    foreach (var cell in element.GetVolumeCells()) {
      yield return cell + rootPosition;
    }
  }

  private bool ValidateCells(IEnumerable<Vector2Int> cells) {
    foreach (var cell in cells) {
      if (!IsCellValid(cell)) return false;
      if (m_runtime.cells[cell.x, cell.y].elementId != -1) return false;
    }
    return true;
  }

  private bool IsCellValid(Vector2Int cell) {
    return cell.x >= 0 && cell.x < m_runtime.config.cellCnt.x &&
           cell.y >= 0 && cell.y < m_runtime.config.cellCnt.y;
  }
}

public abstract class MapGridElement {
  /// <summary>
  /// 获取元素占用的相对单元格坐标（相对于根位置）
  /// </summary>
  public abstract IEnumerable<Vector2Int> GetVolumeCells();
}

public class RectMapGridElement : MapGridElement {
  public Vector2Int size = Vector2Int.one;
  public override IEnumerable<Vector2Int> GetVolumeCells() {
    for (int y = 0; y < size.y; y++) {
      for (int x = 0; x < size.x; x++) {
        yield return new Vector2Int(x, y);
      }
    }
  }
}