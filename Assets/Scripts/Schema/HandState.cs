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
	[Type(0, "int32")]
	public int tadpoles = default(int);

	[Type(1, "int32")]
	public int frogs = default(int);

	/*
	 * Support for individual property change callbacks below...
	 */

	protected event PropertyChangeHandler<int> __tadpolesChange;
	public Action OnTadpolesChange(PropertyChangeHandler<int> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.tadpoles));
		__tadpolesChange += __handler;
		if (__immediate && this.tadpoles != default(int)) { __handler(this.tadpoles, default(int)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(tadpoles));
			__tadpolesChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<int> __frogsChange;
	public Action OnFrogsChange(PropertyChangeHandler<int> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.frogs));
		__frogsChange += __handler;
		if (__immediate && this.frogs != default(int)) { __handler(this.frogs, default(int)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(frogs));
			__frogsChange -= __handler;
		};
	}

	protected override void TriggerFieldChange(DataChange change) {
		switch (change.Field) {
			case nameof(tadpoles): __tadpolesChange?.Invoke((int) change.Value, (int) change.PreviousValue); break;
			case nameof(frogs): __frogsChange?.Invoke((int) change.Value, (int) change.PreviousValue); break;
			default: break;
		}
	}
}

