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
	[Type(0, "string")]
	public string type = default(string);

	[Type(1, "int32")]
	public int playerId = default(int);

	[Type(2, "ref", typeof(ArrayCoordinate))]
	public ArrayCoordinate priorCoordinate = new ArrayCoordinate();

	[Type(3, "ref", typeof(TransformPosition))]
	public TransformPosition position = new TransformPosition();

	/*
	 * Support for individual property change callbacks below...
	 */

	protected event PropertyChangeHandler<string> __typeChange;
	public Action OnTypeChange(PropertyChangeHandler<string> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.type));
		__typeChange += __handler;
		if (__immediate && this.type != default(string)) { __handler(this.type, default(string)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(type));
			__typeChange -= __handler;
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

	protected event PropertyChangeHandler<ArrayCoordinate> __priorCoordinateChange;
	public Action OnPriorCoordinateChange(PropertyChangeHandler<ArrayCoordinate> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.priorCoordinate));
		__priorCoordinateChange += __handler;
		if (__immediate && this.priorCoordinate != null) { __handler(this.priorCoordinate, null); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(priorCoordinate));
			__priorCoordinateChange -= __handler;
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

	protected override void TriggerFieldChange(DataChange change) {
		switch (change.Field) {
			case nameof(type): __typeChange?.Invoke((string) change.Value, (string) change.PreviousValue); break;
			case nameof(playerId): __playerIdChange?.Invoke((int) change.Value, (int) change.PreviousValue); break;
			case nameof(priorCoordinate): __priorCoordinateChange?.Invoke((ArrayCoordinate) change.Value, (ArrayCoordinate) change.PreviousValue); break;
			case nameof(position): __positionChange?.Invoke((TransformPosition) change.Value, (TransformPosition) change.PreviousValue); break;
			default: break;
		}
	}
}

