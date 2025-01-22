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
	[Type(0, "ref", typeof(Vector2Schema))]
	public Vector2Schema coordinate = new Vector2Schema();

	[Type(1, "ref", typeof(NeighborState))]
	public NeighborState neighbor = new NeighborState();

	[Type(2, "array", typeof(ArraySchema<Vector2Schema>))]
	public ArraySchema<Vector2Schema> neighbors = new ArraySchema<Vector2Schema>();

	/*
	 * Support for individual property change callbacks below...
	 */

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

	protected event PropertyChangeHandler<ArraySchema<Vector2Schema>> __neighborsChange;
	public Action OnNeighborsChange(PropertyChangeHandler<ArraySchema<Vector2Schema>> __handler, bool __immediate = true) {
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
			case nameof(coordinate): __coordinateChange?.Invoke((Vector2Schema) change.Value, (Vector2Schema) change.PreviousValue); break;
			case nameof(neighbor): __neighborChange?.Invoke((NeighborState) change.Value, (NeighborState) change.PreviousValue); break;
			case nameof(neighbors): __neighborsChange?.Invoke((ArraySchema<Vector2Schema>) change.Value, (ArraySchema<Vector2Schema>) change.PreviousValue); break;
			default: break;
		}
	}
}

