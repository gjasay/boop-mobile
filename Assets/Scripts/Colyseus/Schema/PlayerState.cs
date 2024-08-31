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

public partial class PlayerState : Schema {
#if UNITY_5_3_OR_NEWER
[Preserve] 
#endif
public PlayerState() { }
	[Type(0, "string")]
	public string sessionId = default(string);

	[Type(1, "ref", typeof(HandState))]
	public HandState hand = new HandState();

	/*
	 * Support for individual property change callbacks below...
	 */

	protected event PropertyChangeHandler<string> __sessionIdChange;
	public Action OnSessionIdChange(PropertyChangeHandler<string> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.sessionId));
		__sessionIdChange += __handler;
		if (__immediate && this.sessionId != default(string)) { __handler(this.sessionId, default(string)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(sessionId));
			__sessionIdChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<HandState> __handChange;
	public Action OnHandChange(PropertyChangeHandler<HandState> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.hand));
		__handChange += __handler;
		if (__immediate && this.hand != null) { __handler(this.hand, null); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(hand));
			__handChange -= __handler;
		};
	}

	protected override void TriggerFieldChange(DataChange change) {
		switch (change.Field) {
			case nameof(sessionId): __sessionIdChange?.Invoke((string) change.Value, (string) change.PreviousValue); break;
			case nameof(hand): __handChange?.Invoke((HandState) change.Value, (HandState) change.PreviousValue); break;
			default: break;
		}
	}
}

