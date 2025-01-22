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

public partial class Vector2Schema : Schema {
#if UNITY_5_3_OR_NEWER
[Preserve] 
#endif
public Vector2Schema() { }
	[Type(0, "int8")]
	public sbyte x = default(sbyte);

	[Type(1, "int8")]
	public sbyte y = default(sbyte);

	/*
	 * Support for individual property change callbacks below...
	 */

	protected event PropertyChangeHandler<sbyte> __xChange;
	public Action OnXChange(PropertyChangeHandler<sbyte> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.x));
		__xChange += __handler;
		if (__immediate && this.x != default(sbyte)) { __handler(this.x, default(sbyte)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(x));
			__xChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<sbyte> __yChange;
	public Action OnYChange(PropertyChangeHandler<sbyte> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.y));
		__yChange += __handler;
		if (__immediate && this.y != default(sbyte)) { __handler(this.y, default(sbyte)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(y));
			__yChange -= __handler;
		};
	}

	protected override void TriggerFieldChange(DataChange change) {
		switch (change.Field) {
			case nameof(x): __xChange?.Invoke((sbyte) change.Value, (sbyte) change.PreviousValue); break;
			case nameof(y): __yChange?.Invoke((sbyte) change.Value, (sbyte) change.PreviousValue); break;
			default: break;
		}
	}
}

