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

public partial class BoardState : Schema {
#if UNITY_5_3_OR_NEWER
[Preserve] 
#endif
public BoardState() { }
	[Type(0, "array", typeof(ArraySchema<GamePieceState>))]
	public ArraySchema<GamePieceState> tadpoles = new ArraySchema<GamePieceState>();

	[Type(1, "array", typeof(ArraySchema<GamePieceState>))]
	public ArraySchema<GamePieceState> frogs = new ArraySchema<GamePieceState>();

	/*
	 * Support for individual property change callbacks below...
	 */

	protected event PropertyChangeHandler<ArraySchema<GamePieceState>> __tadpolesChange;
	public Action OnTadpolesChange(PropertyChangeHandler<ArraySchema<GamePieceState>> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.tadpoles));
		__tadpolesChange += __handler;
		if (__immediate && this.tadpoles != null) { __handler(this.tadpoles, null); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(tadpoles));
			__tadpolesChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<ArraySchema<GamePieceState>> __frogsChange;
	public Action OnFrogsChange(PropertyChangeHandler<ArraySchema<GamePieceState>> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.frogs));
		__frogsChange += __handler;
		if (__immediate && this.frogs != null) { __handler(this.frogs, null); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(frogs));
			__frogsChange -= __handler;
		};
	}

	protected override void TriggerFieldChange(DataChange change) {
		switch (change.Field) {
			case nameof(tadpoles): __tadpolesChange?.Invoke((ArraySchema<GamePieceState>) change.Value, (ArraySchema<GamePieceState>) change.PreviousValue); break;
			case nameof(frogs): __frogsChange?.Invoke((ArraySchema<GamePieceState>) change.Value, (ArraySchema<GamePieceState>) change.PreviousValue); break;
			default: break;
		}
	}
}

