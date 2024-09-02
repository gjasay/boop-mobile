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

public partial class GamePieceState : Schema {
#if UNITY_5_3_OR_NEWER
[Preserve] 
#endif
public GamePieceState() { }
	[Type(0, "ref", typeof(PositionState))]
	public PositionState position = new PositionState();

	[Type(1, "int32")]
	public int playerId = default(int);

	/*
	 * Support for individual property change callbacks below...
	 */

	protected event PropertyChangeHandler<PositionState> __positionChange;
	public Action OnPositionChange(PropertyChangeHandler<PositionState> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.position));
		__positionChange += __handler;
		if (__immediate && this.position != null) { __handler(this.position, null); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(position));
			__positionChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<int> __playerIdChange;
	public Action OnPlayerIdChange(PropertyChangeHandler<int> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.playerId));
		__playerIdChange += __handler;
		if (__immediate && this.playerId != default(int)) { __handler(this.playerId, default(int)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(playerId));
			__playerIdChange -= __handler;
		};
	}

	protected override void TriggerFieldChange(DataChange change) {
		switch (change.Field) {
			case nameof(position): __positionChange?.Invoke((PositionState) change.Value, (PositionState) change.PreviousValue); break;
			case nameof(playerId): __playerIdChange?.Invoke((int) change.Value, (int) change.PreviousValue); break;
			default: break;
		}
	}
}

