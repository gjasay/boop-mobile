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

public partial class TileState : Schema {
#if UNITY_5_3_OR_NEWER
[Preserve] 
#endif
public TileState() { }
	[Type(0, "ref", typeof(GamePieceState))]
	public GamePieceState gamePiece = new GamePieceState();

	[Type(1, "int32")]
	public int x = default(int);

	[Type(2, "int32")]
	public int y = default(int);

	/*
	 * Support for individual property change callbacks below...
	 */

	protected event PropertyChangeHandler<GamePieceState> __gamePieceChange;
	public Action OnGamePieceChange(PropertyChangeHandler<GamePieceState> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.gamePiece));
		__gamePieceChange += __handler;
		if (__immediate && this.gamePiece != null) { __handler(this.gamePiece, null); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(gamePiece));
			__gamePieceChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<int> __xChange;
	public Action OnXChange(PropertyChangeHandler<int> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.x));
		__xChange += __handler;
		if (__immediate && this.x != default(int)) { __handler(this.x, default(int)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(x));
			__xChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<int> __yChange;
	public Action OnYChange(PropertyChangeHandler<int> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.y));
		__yChange += __handler;
		if (__immediate && this.y != default(int)) { __handler(this.y, default(int)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(y));
			__yChange -= __handler;
		};
	}

	protected override void TriggerFieldChange(DataChange change) {
		switch (change.Field) {
			case nameof(gamePiece): __gamePieceChange?.Invoke((GamePieceState) change.Value, (GamePieceState) change.PreviousValue); break;
			case nameof(x): __xChange?.Invoke((int) change.Value, (int) change.PreviousValue); break;
			case nameof(y): __yChange?.Invoke((int) change.Value, (int) change.PreviousValue); break;
			default: break;
		}
	}
}

