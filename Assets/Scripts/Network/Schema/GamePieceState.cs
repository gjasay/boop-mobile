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
	[Type(0, "ref", typeof(TileState))]
	public TileState tile = new TileState();

	[Type(1, "int32")]
	public int playerId = default(int);

	/*
	 * Support for individual property change callbacks below...
	 */

	protected event PropertyChangeHandler<TileState> __tileChange;
	public Action OnTileChange(PropertyChangeHandler<TileState> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.tile));
		__tileChange += __handler;
		if (__immediate && this.tile != null) { __handler(this.tile, null); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(tile));
			__tileChange -= __handler;
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
			case nameof(tile): __tileChange?.Invoke((TileState) change.Value, (TileState) change.PreviousValue); break;
			case nameof(playerId): __playerIdChange?.Invoke((int) change.Value, (int) change.PreviousValue); break;
			default: break;
		}
	}
}

