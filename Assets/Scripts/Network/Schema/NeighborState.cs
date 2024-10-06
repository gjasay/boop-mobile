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
	[Type(0, "ref", typeof(ArrayCoordinate))]
	public ArrayCoordinate up = new ArrayCoordinate();

	[Type(1, "ref", typeof(ArrayCoordinate))]
	public ArrayCoordinate down = new ArrayCoordinate();

	[Type(2, "ref", typeof(ArrayCoordinate))]
	public ArrayCoordinate left = new ArrayCoordinate();

	[Type(3, "ref", typeof(ArrayCoordinate))]
	public ArrayCoordinate right = new ArrayCoordinate();

	[Type(4, "ref", typeof(ArrayCoordinate))]
	public ArrayCoordinate upLeft = new ArrayCoordinate();

	[Type(5, "ref", typeof(ArrayCoordinate))]
	public ArrayCoordinate upRight = new ArrayCoordinate();

	[Type(6, "ref", typeof(ArrayCoordinate))]
	public ArrayCoordinate downLeft = new ArrayCoordinate();

	[Type(7, "ref", typeof(ArrayCoordinate))]
	public ArrayCoordinate downRight = new ArrayCoordinate();

	/*
	 * Support for individual property change callbacks below...
	 */

	protected event PropertyChangeHandler<ArrayCoordinate> __upChange;
	public Action OnUpChange(PropertyChangeHandler<ArrayCoordinate> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.up));
		__upChange += __handler;
		if (__immediate && this.up != null) { __handler(this.up, null); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(up));
			__upChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<ArrayCoordinate> __downChange;
	public Action OnDownChange(PropertyChangeHandler<ArrayCoordinate> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.down));
		__downChange += __handler;
		if (__immediate && this.down != null) { __handler(this.down, null); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(down));
			__downChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<ArrayCoordinate> __leftChange;
	public Action OnLeftChange(PropertyChangeHandler<ArrayCoordinate> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.left));
		__leftChange += __handler;
		if (__immediate && this.left != null) { __handler(this.left, null); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(left));
			__leftChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<ArrayCoordinate> __rightChange;
	public Action OnRightChange(PropertyChangeHandler<ArrayCoordinate> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.right));
		__rightChange += __handler;
		if (__immediate && this.right != null) { __handler(this.right, null); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(right));
			__rightChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<ArrayCoordinate> __upLeftChange;
	public Action OnUpLeftChange(PropertyChangeHandler<ArrayCoordinate> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.upLeft));
		__upLeftChange += __handler;
		if (__immediate && this.upLeft != null) { __handler(this.upLeft, null); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(upLeft));
			__upLeftChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<ArrayCoordinate> __upRightChange;
	public Action OnUpRightChange(PropertyChangeHandler<ArrayCoordinate> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.upRight));
		__upRightChange += __handler;
		if (__immediate && this.upRight != null) { __handler(this.upRight, null); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(upRight));
			__upRightChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<ArrayCoordinate> __downLeftChange;
	public Action OnDownLeftChange(PropertyChangeHandler<ArrayCoordinate> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.downLeft));
		__downLeftChange += __handler;
		if (__immediate && this.downLeft != null) { __handler(this.downLeft, null); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(downLeft));
			__downLeftChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<ArrayCoordinate> __downRightChange;
	public Action OnDownRightChange(PropertyChangeHandler<ArrayCoordinate> __handler, bool __immediate = true) {
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
			case nameof(up): __upChange?.Invoke((ArrayCoordinate) change.Value, (ArrayCoordinate) change.PreviousValue); break;
			case nameof(down): __downChange?.Invoke((ArrayCoordinate) change.Value, (ArrayCoordinate) change.PreviousValue); break;
			case nameof(left): __leftChange?.Invoke((ArrayCoordinate) change.Value, (ArrayCoordinate) change.PreviousValue); break;
			case nameof(right): __rightChange?.Invoke((ArrayCoordinate) change.Value, (ArrayCoordinate) change.PreviousValue); break;
			case nameof(upLeft): __upLeftChange?.Invoke((ArrayCoordinate) change.Value, (ArrayCoordinate) change.PreviousValue); break;
			case nameof(upRight): __upRightChange?.Invoke((ArrayCoordinate) change.Value, (ArrayCoordinate) change.PreviousValue); break;
			case nameof(downLeft): __downLeftChange?.Invoke((ArrayCoordinate) change.Value, (ArrayCoordinate) change.PreviousValue); break;
			case nameof(downRight): __downRightChange?.Invoke((ArrayCoordinate) change.Value, (ArrayCoordinate) change.PreviousValue); break;
			default: break;
		}
	}
}

