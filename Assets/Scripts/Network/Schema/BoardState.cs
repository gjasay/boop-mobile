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
	[Type(0, "array", typeof(ArraySchema<TileState>))]
	public ArraySchema<TileState> tiles = new ArraySchema<TileState>();

	[Type(1, "int32")]
	public int width = default(int);

	[Type(2, "int32")]
	public int height = default(int);

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

	protected event PropertyChangeHandler<int> __widthChange;
	public Action OnWidthChange(PropertyChangeHandler<int> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.width));
		__widthChange += __handler;
		if (__immediate && this.width != default(int)) { __handler(this.width, default(int)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(width));
			__widthChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<int> __heightChange;
	public Action OnHeightChange(PropertyChangeHandler<int> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.height));
		__heightChange += __handler;
		if (__immediate && this.height != default(int)) { __handler(this.height, default(int)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(height));
			__heightChange -= __handler;
		};
	}

	protected override void TriggerFieldChange(DataChange change) {
		switch (change.Field) {
			case nameof(tiles): __tilesChange?.Invoke((ArraySchema<TileState>) change.Value, (ArraySchema<TileState>) change.PreviousValue); break;
			case nameof(width): __widthChange?.Invoke((int) change.Value, (int) change.PreviousValue); break;
			case nameof(height): __heightChange?.Invoke((int) change.Value, (int) change.PreviousValue); break;
			default: break;
		}
	}
}

