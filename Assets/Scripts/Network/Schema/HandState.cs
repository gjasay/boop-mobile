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

public partial class HandState : Schema {
#if UNITY_5_3_OR_NEWER
[Preserve] 
#endif
public HandState() { }
	[Type(0, "int8")]
	public sbyte kittens = default(sbyte);

	[Type(1, "int8")]
	public sbyte cats = default(sbyte);

	/*
	 * Support for individual property change callbacks below...
	 */

	protected event PropertyChangeHandler<sbyte> __kittensChange;
	public Action OnKittensChange(PropertyChangeHandler<sbyte> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.kittens));
		__kittensChange += __handler;
		if (__immediate && this.kittens != default(sbyte)) { __handler(this.kittens, default(sbyte)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(kittens));
			__kittensChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<sbyte> __catsChange;
	public Action OnCatsChange(PropertyChangeHandler<sbyte> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.cats));
		__catsChange += __handler;
		if (__immediate && this.cats != default(sbyte)) { __handler(this.cats, default(sbyte)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(cats));
			__catsChange -= __handler;
		};
	}

	protected override void TriggerFieldChange(DataChange change) {
		switch (change.Field) {
			case nameof(kittens): __kittensChange?.Invoke((sbyte) change.Value, (sbyte) change.PreviousValue); break;
			case nameof(cats): __catsChange?.Invoke((sbyte) change.Value, (sbyte) change.PreviousValue); break;
			default: break;
		}
	}
}

