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

	[Type(1, "int8")]
	public sbyte playerId = default(sbyte);

	[Type(2, "ref", typeof(Vector2Schema))]
	public Vector2Schema coordinate = new Vector2Schema();

	[Type(3, "boolean")]
	public bool isGraduating = default(bool);

	[Type(4, "string")]
	public string outOfBounds = default(string);

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

	protected event PropertyChangeHandler<sbyte> __playerIdChange;
	public Action OnPlayerIdChange(PropertyChangeHandler<sbyte> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.playerId));
		__playerIdChange += __handler;
		if (__immediate && this.playerId != default(sbyte)) { __handler(this.playerId, default(sbyte)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(playerId));
			__playerIdChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<Vector2Schema> __coordinateChange;
	public Action OnCoordinateChange(PropertyChangeHandler<Vector2Schema> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.coordinate));
		__coordinateChange += __handler;
		if (__immediate && this.coordinate != null) { __handler(this.coordinate, null); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(coordinate));
			__coordinateChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<bool> __isGraduatingChange;
	public Action OnIsGraduatingChange(PropertyChangeHandler<bool> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.isGraduating));
		__isGraduatingChange += __handler;
		if (__immediate && this.isGraduating != default(bool)) { __handler(this.isGraduating, default(bool)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(isGraduating));
			__isGraduatingChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<string> __outOfBoundsChange;
	public Action OnOutOfBoundsChange(PropertyChangeHandler<string> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.outOfBounds));
		__outOfBoundsChange += __handler;
		if (__immediate && this.outOfBounds != default(string)) { __handler(this.outOfBounds, default(string)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(outOfBounds));
			__outOfBoundsChange -= __handler;
		};
	}

	protected override void TriggerFieldChange(DataChange change) {
		switch (change.Field) {
			case nameof(type): __typeChange?.Invoke((string) change.Value, (string) change.PreviousValue); break;
			case nameof(playerId): __playerIdChange?.Invoke((sbyte) change.Value, (sbyte) change.PreviousValue); break;
			case nameof(coordinate): __coordinateChange?.Invoke((Vector2Schema) change.Value, (Vector2Schema) change.PreviousValue); break;
			case nameof(isGraduating): __isGraduatingChange?.Invoke((bool) change.Value, (bool) change.PreviousValue); break;
			case nameof(outOfBounds): __outOfBoundsChange?.Invoke((string) change.Value, (string) change.PreviousValue); break;
			default: break;
		}
	}
}

