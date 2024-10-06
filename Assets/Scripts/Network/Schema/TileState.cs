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

	[Type(1, "ref", typeof(ArrayCoordinate))]
	public ArrayCoordinate arrayPosition = new ArrayCoordinate();

	[Type(2, "ref", typeof(TransformPosition))]
	public TransformPosition position = new TransformPosition();

	[Type(3, "ref", typeof(NeighborState))]
	public NeighborState neighbor = new NeighborState();

	[Type(4, "array", typeof(ArraySchema<ArrayCoordinate>))]
	public ArraySchema<ArrayCoordinate> neighbors = new ArraySchema<ArrayCoordinate>();

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

	protected event PropertyChangeHandler<ArrayCoordinate> __arrayPositionChange;
	public Action OnArrayPositionChange(PropertyChangeHandler<ArrayCoordinate> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.arrayPosition));
		__arrayPositionChange += __handler;
		if (__immediate && this.arrayPosition != null) { __handler(this.arrayPosition, null); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(arrayPosition));
			__arrayPositionChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<TransformPosition> __positionChange;
	public Action OnPositionChange(PropertyChangeHandler<TransformPosition> __handler, bool __immediate = true) {
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

	protected event PropertyChangeHandler<ArraySchema<ArrayCoordinate>> __neighborsChange;
	public Action OnNeighborsChange(PropertyChangeHandler<ArraySchema<ArrayCoordinate>> __handler, bool __immediate = true) {
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
			case nameof(arrayPosition): __arrayPositionChange?.Invoke((ArrayCoordinate) change.Value, (ArrayCoordinate) change.PreviousValue); break;
			case nameof(position): __positionChange?.Invoke((TransformPosition) change.Value, (TransformPosition) change.PreviousValue); break;
			case nameof(neighbor): __neighborChange?.Invoke((NeighborState) change.Value, (NeighborState) change.PreviousValue); break;
			case nameof(neighbors): __neighborsChange?.Invoke((ArraySchema<ArrayCoordinate>) change.Value, (ArraySchema<ArrayCoordinate>) change.PreviousValue); break;
			default: break;
		}
	}
}

