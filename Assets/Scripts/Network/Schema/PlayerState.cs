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
	[Type(0, "int32")]
	public int id = default(int);

	[Type(1, "string")]
	public string sessionId = default(string);

	[Type(2, "ref", typeof(HandState))]
	public HandState hand = new HandState();

	[Type(3, "float32")]
	public float timer = default(float);

	/*
	 * Support for individual property change callbacks below...
	 */

	protected event PropertyChangeHandler<int> __idChange;
	public Action OnIdChange(PropertyChangeHandler<int> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.id));
		__idChange += __handler;
		if (__immediate && this.id != default(int)) { __handler(this.id, default(int)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(id));
			__idChange -= __handler;
		};
	}

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

	protected event PropertyChangeHandler<float> __timerChange;
	public Action OnTimerChange(PropertyChangeHandler<float> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.timer));
		__timerChange += __handler;
		if (__immediate && this.timer != default(float)) { __handler(this.timer, default(float)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(timer));
			__timerChange -= __handler;
		};
	}

	protected override void TriggerFieldChange(DataChange change) {
		switch (change.Field) {
			case nameof(id): __idChange?.Invoke((int) change.Value, (int) change.PreviousValue); break;
			case nameof(sessionId): __sessionIdChange?.Invoke((string) change.Value, (string) change.PreviousValue); break;
			case nameof(hand): __handChange?.Invoke((HandState) change.Value, (HandState) change.PreviousValue); break;
			case nameof(timer): __timerChange?.Invoke((float) change.Value, (float) change.PreviousValue); break;
			default: break;
		}
	}
}

