using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiteStateEngine : MonoBehaviour {
	[SerializeField]
	private List<LiteState> _registerStates = new();

	private Dictionary<Type, LiteState> m_stateDict = new();
	private Stack<LiteState> m_stack = new();
	public LiteState frontState => m_stack.Count > 0 ? m_stack.Peek() : null;

	private void Awake() {
		InitStateEngine();
		if (_registerStates.Count == 0) {
			return;
		}
		var defaultState = _registerStates[0];
		foreach (var state in m_stateDict.Values) {
			state.SetStataeActive(state == defaultState);
		}
		AddTop(defaultState.GetType());
	}

	private void InitStateEngine() {
		foreach (var state in _registerStates) {
			m_stateDict[state.GetType()] = state;
			state.InitByStateEngine(this);
		}
	}

	public void AddTop<T>() where T : LiteState {
		AddTop(typeof(T));
	}

	public void AddTop(Type type) {
		var prev = frontState;
		if (frontState != null) {
			frontState.OnExit();
			frontState.SetStataeActive(false);
		}
		if (m_stateDict.TryGetValue(type, out var state)) {
			m_stack.Push(state);
			state.SetStataeActive(true);
			state.OnEnter();
		} else {
			Debug.LogError($"State of type {type} not found in registered states.");
		}
	}

	public void ReplaceTop<T>() where T : LiteState {
		ReplaceTop(typeof(T));
	}
	public void ReplaceTop(Type type) {
		var top = m_stack.Peek();
		if (top.GetType() == type) {
			Debug.LogError($"State of type {type} is already on top of the stack.");
			return;
		}
		top.OnExit();
		top.SetStataeActive(false);
		m_stack.Pop();
		if (m_stateDict.TryGetValue(type, out var state)) {
			m_stack.Push(state);
			state.SetStataeActive(true);
			state.OnEnter();
		} else {
			Debug.LogError($"State of type {type} not found in registered states.");
		}
	}

	public void RemoveTop() {
		var cnt = m_stack.Count;
		if (cnt <= 1) {
			Debug.Log("not enough state");
			return;
		}
		var top = m_stack.Pop();
		top.OnExit();
		top.SetStataeActive(false);
		var nextState = m_stack.Peek();
		if (nextState != null) {
			nextState.SetStataeActive(true);
			nextState.OnResume();
		}
	}
}
