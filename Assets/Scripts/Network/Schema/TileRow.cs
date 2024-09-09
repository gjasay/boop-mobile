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

public partial class TileRow : Schema {
#if UNITY_5_3_OR_NEWER
[Preserve] 
#endif
public TileRow() { }
	[Type(0, "array", typeof(ArraySchema<TileState>))]
	public ArraySchema<TileState> tiles = new ArraySchema<TileState>();

	/*
	 * Support for individual property change callbacks below...
	 */

	protected event PropertyChangeHandler<ArraySchema<TileState>> __tilesChange;
	public Action OnTilesChange(PropertyChangeHandler<ArraySchema<TileState>> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.tiles));
		__tilesChange += __handler;
		if (__immediate && this.tiles != null) { __handler(this.tiles, null); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(tiles));
			__tilesChange -= __handler;
		};
	}

	protected override void TriggerFieldChange(DataChange change) {
		switch (change.Field) {
			case nameof(tiles): __tilesChange?.Invoke((ArraySchema<TileState>) change.Value, (ArraySchema<TileState>) change.PreviousValue); break;
			default: break;
		}
	}
}

