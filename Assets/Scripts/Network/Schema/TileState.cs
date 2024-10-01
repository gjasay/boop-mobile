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

	[Type(1, "ref", typeof(PositionState))]
	public PositionState position = new PositionState();

	[Type(2, "ref", typeof(NeighborState))]
	public NeighborState neighbor = new NeighborState();

	[Type(3, "array", typeof(ArraySchema<PositionState>))]
	public ArraySchema<PositionState> neighbors = new ArraySchema<PositionState>();

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

	protected event PropertyChangeHandler<NeighborState> __neighborChange;
	public Action OnNeighborChange(PropertyChangeHandler<NeighborState> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.neighbor));
		__neighborChange += __handler;
		if (__immediate && this.neighbor != null) { __handler(this.neighbor, null); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(neighbor));
			__neighborChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<ArraySchema<PositionState>> __neighborsChange;
	public Action OnNeighborsChange(PropertyChangeHandler<ArraySchema<PositionState>> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.neighbors));
		__neighborsChange += __handler;
		if (__immediate && this.neighbors != null) { __handler(this.neighbors, null); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(neighbors));
			__neighborsChange -= __handler;
		};
	}

	protected override void TriggerFieldChange(DataChange change) {
		switch (change.Field) {
			case nameof(gamePiece): __gamePieceChange?.Invoke((GamePieceState) change.Value, (GamePieceState) change.PreviousValue); break;
			case nameof(position): __positionChange?.Invoke((PositionState) change.Value, (PositionState) change.PreviousValue); break;
			case nameof(neighbor): __neighborChange?.Invoke((NeighborState) change.Value, (NeighborState) change.PreviousValue); break;
			case nameof(neighbors): __neighborsChange?.Invoke((ArraySchema<PositionState>) change.Value, (ArraySchema<PositionState>) change.PreviousValue); break;
			default: break;
		}
	}
}

