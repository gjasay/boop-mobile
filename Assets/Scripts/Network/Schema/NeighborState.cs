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
	[Type(0, "ref", typeof(Vector2Schema))]
	public Vector2Schema up = new Vector2Schema();

	[Type(1, "ref", typeof(Vector2Schema))]
	public Vector2Schema down = new Vector2Schema();

	[Type(2, "ref", typeof(Vector2Schema))]
	public Vector2Schema left = new Vector2Schema();

	[Type(3, "ref", typeof(Vector2Schema))]
	public Vector2Schema right = new Vector2Schema();

	[Type(4, "ref", typeof(Vector2Schema))]
	public Vector2Schema upLeft = new Vector2Schema();

	[Type(5, "ref", typeof(Vector2Schema))]
	public Vector2Schema upRight = new Vector2Schema();

	[Type(6, "ref", typeof(Vector2Schema))]
	public Vector2Schema downLeft = new Vector2Schema();

	[Type(7, "ref", typeof(Vector2Schema))]
	public Vector2Schema downRight = new Vector2Schema();

	/*
	 * Support for individual property change callbacks below...
	 */

	protected event PropertyChangeHandler<Vector2Schema> __upChange;
	public Action OnUpChange(PropertyChangeHandler<Vector2Schema> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.up));
		__upChange += __handler;
		if (__immediate && this.up != null) { __handler(this.up, null); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(up));
			__upChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<Vector2Schema> __downChange;
	public Action OnDownChange(PropertyChangeHandler<Vector2Schema> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.down));
		__downChange += __handler;
		if (__immediate && this.down != null) { __handler(this.down, null); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(down));
			__downChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<Vector2Schema> __leftChange;
	public Action OnLeftChange(PropertyChangeHandler<Vector2Schema> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.left));
		__leftChange += __handler;
		if (__immediate && this.left != null) { __handler(this.left, null); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(left));
			__leftChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<Vector2Schema> __rightChange;
	public Action OnRightChange(PropertyChangeHandler<Vector2Schema> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.right));
		__rightChange += __handler;
		if (__immediate && this.right != null) { __handler(this.right, null); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(right));
			__rightChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<Vector2Schema> __upLeftChange;
	public Action OnUpLeftChange(PropertyChangeHandler<Vector2Schema> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.upLeft));
		__upLeftChange += __handler;
		if (__immediate && this.upLeft != null) { __handler(this.upLeft, null); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(upLeft));
			__upLeftChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<Vector2Schema> __upRightChange;
	public Action OnUpRightChange(PropertyChangeHandler<Vector2Schema> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.upRight));
		__upRightChange += __handler;
		if (__immediate && this.upRight != null) { __handler(this.upRight, null); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(upRight));
			__upRightChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<Vector2Schema> __downLeftChange;
	public Action OnDownLeftChange(PropertyChangeHandler<Vector2Schema> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.downLeft));
		__downLeftChange += __handler;
		if (__immediate && this.downLeft != null) { __handler(this.downLeft, null); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(downLeft));
			__downLeftChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<Vector2Schema> __downRightChange;
	public Action OnDownRightChange(PropertyChangeHandler<Vector2Schema> __handler, bool __immediate = true) {
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
			case nameof(up): __upChange?.Invoke((Vector2Schema) change.Value, (Vector2Schema) change.PreviousValue); break;
			case nameof(down): __downChange?.Invoke((Vector2Schema) change.Value, (Vector2Schema) change.PreviousValue); break;
			case nameof(left): __leftChange?.Invoke((Vector2Schema) change.Value, (Vector2Schema) change.PreviousValue); break;
			case nameof(right): __rightChange?.Invoke((Vector2Schema) change.Value, (Vector2Schema) change.PreviousValue); break;
			case nameof(upLeft): __upLeftChange?.Invoke((Vector2Schema) change.Value, (Vector2Schema) change.PreviousValue); break;
			case nameof(upRight): __upRightChange?.Invoke((Vector2Schema) change.Value, (Vector2Schema) change.PreviousValue); break;
			case nameof(downLeft): __downLeftChange?.Invoke((Vector2Schema) change.Value, (Vector2Schema) change.PreviousValue); break;
			case nameof(downRight): __downRightChange?.Invoke((Vector2Schema) change.Value, (Vector2Schema) change.PreviousValue); break;
			default: break;
		}
	}
}

