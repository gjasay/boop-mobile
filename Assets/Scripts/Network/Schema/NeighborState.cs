// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 2.0.35
// 

using Colyseus.Schema;
using Action = System.Action;
#if UNITY_5_3_OR_NEWER
using UnityEngine.Scripting;
#endif

public partial class NeighborState : Schema {
#if UNITY_5_3_OR_NEWER
[Preserve] 
#endif
public NeighborState() { }
	[Type(0, "ref", typeof(PositionState))]
	public PositionState up = new PositionState();

	[Type(1, "ref", typeof(PositionState))]
	public PositionState down = new PositionState();

	[Type(2, "ref", typeof(PositionState))]
	public PositionState left = new PositionState();

	[Type(3, "ref", typeof(PositionState))]
	public PositionState right = new PositionState();

	[Type(4, "ref", typeof(PositionState))]
	public PositionState upLeft = new PositionState();

	[Type(5, "ref", typeof(PositionState))]
	public PositionState upRight = new PositionState();

	[Type(6, "ref", typeof(PositionState))]
	public PositionState downLeft = new PositionState();

	[Type(7, "ref", typeof(PositionState))]
	public PositionState downRight = new PositionState();

	/*
	 * Support for individual property change callbacks below...
	 */

	protected event PropertyChangeHandler<PositionState> __upChange;
	public Action OnUpChange(PropertyChangeHandler<PositionState> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.up));
		__upChange += __handler;
		if (__immediate && this.up != null) { __handler(this.up, null); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(up));
			__upChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<PositionState> __downChange;
	public Action OnDownChange(PropertyChangeHandler<PositionState> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.down));
		__downChange += __handler;
		if (__immediate && this.down != null) { __handler(this.down, null); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(down));
			__downChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<PositionState> __leftChange;
	public Action OnLeftChange(PropertyChangeHandler<PositionState> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.left));
		__leftChange += __handler;
		if (__immediate && this.left != null) { __handler(this.left, null); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(left));
			__leftChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<PositionState> __rightChange;
	public Action OnRightChange(PropertyChangeHandler<PositionState> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.right));
		__rightChange += __handler;
		if (__immediate && this.right != null) { __handler(this.right, null); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(right));
			__rightChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<PositionState> __upLeftChange;
	public Action OnUpLeftChange(PropertyChangeHandler<PositionState> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.upLeft));
		__upLeftChange += __handler;
		if (__immediate && this.upLeft != null) { __handler(this.upLeft, null); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(upLeft));
			__upLeftChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<PositionState> __upRightChange;
	public Action OnUpRightChange(PropertyChangeHandler<PositionState> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.upRight));
		__upRightChange += __handler;
		if (__immediate && this.upRight != null) { __handler(this.upRight, null); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(upRight));
			__upRightChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<PositionState> __downLeftChange;
	public Action OnDownLeftChange(PropertyChangeHandler<PositionState> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.downLeft));
		__downLeftChange += __handler;
		if (__immediate && this.downLeft != null) { __handler(this.downLeft, null); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(downLeft));
			__downLeftChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<PositionState> __downRightChange;
	public Action OnDownRightChange(PropertyChangeHandler<PositionState> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.downRight));
		__downRightChange += __handler;
		if (__immediate && this.downRight != null) { __handler(this.downRight, null); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(downRight));
			__downRightChange -= __handler;
		};
	}

	protected override void TriggerFieldChange(DataChange change) {
		switch (change.Field) {
			case nameof(up): __upChange?.Invoke((PositionState) change.Value, (PositionState) change.PreviousValue); break;
			case nameof(down): __downChange?.Invoke((PositionState) change.Value, (PositionState) change.PreviousValue); break;
			case nameof(left): __leftChange?.Invoke((PositionState) change.Value, (PositionState) change.PreviousValue); break;
			case nameof(right): __rightChange?.Invoke((PositionState) change.Value, (PositionState) change.PreviousValue); break;
			case nameof(upLeft): __upLeftChange?.Invoke((PositionState) change.Value, (PositionState) change.PreviousValue); break;
			case nameof(upRight): __upRightChange?.Invoke((PositionState) change.Value, (PositionState) change.PreviousValue); break;
			case nameof(downLeft): __downLeftChange?.Invoke((PositionState) change.Value, (PositionState) change.PreviousValue); break;
			case nameof(downRight): __downRightChange?.Invoke((PositionState) change.Value, (PositionState) change.PreviousValue); break;
			default: break;
		}
	}
}

