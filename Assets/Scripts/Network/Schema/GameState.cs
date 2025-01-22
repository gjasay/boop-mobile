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

public partial class GameState : Schema {
#if UNITY_5_3_OR_NEWER
[Preserve] 
#endif
public GameState() { }
	[Type(0, "ref", typeof(PlayerState))]
	public PlayerState playerOne = new PlayerState();

	[Type(1, "ref", typeof(PlayerState))]
	public PlayerState playerTwo = new PlayerState();

	[Type(2, "array", typeof(ArraySchema<TileState>))]
	public ArraySchema<TileState> tiles = new ArraySchema<TileState>();

	[Type(3, "array", typeof(ArraySchema<GamePieceState>))]
	public ArraySchema<GamePieceState> gamePieces = new ArraySchema<GamePieceState>();

	[Type(4, "ref", typeof(Vector2Schema))]
	public Vector2Schema boardSize = new Vector2Schema();

	[Type(5, "int8")]
	public sbyte currentPlayer = default(sbyte);

	[Type(6, "int8")]
	public sbyte winner = default(sbyte);

	/*
	 * Support for individual property change callbacks below...
	 */

	protected event PropertyChangeHandler<PlayerState> __playerOneChange;
	public Action OnPlayerOneChange(PropertyChangeHandler<PlayerState> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.playerOne));
		__playerOneChange += __handler;
		if (__immediate && this.playerOne != null) { __handler(this.playerOne, null); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(playerOne));
			__playerOneChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<PlayerState> __playerTwoChange;
	public Action OnPlayerTwoChange(PropertyChangeHandler<PlayerState> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.playerTwo));
		__playerTwoChange += __handler;
		if (__immediate && this.playerTwo != null) { __handler(this.playerTwo, null); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(playerTwo));
			__playerTwoChange -= __handler;
		};
	}

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

	protected event PropertyChangeHandler<ArraySchema<GamePieceState>> __gamePiecesChange;
	public Action OnGamePiecesChange(PropertyChangeHandler<ArraySchema<GamePieceState>> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.gamePieces));
		__gamePiecesChange += __handler;
		if (__immediate && this.gamePieces != null) { __handler(this.gamePieces, null); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(gamePieces));
			__gamePiecesChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<Vector2Schema> __boardSizeChange;
	public Action OnBoardSizeChange(PropertyChangeHandler<Vector2Schema> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.boardSize));
		__boardSizeChange += __handler;
		if (__immediate && this.boardSize != null) { __handler(this.boardSize, null); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(boardSize));
			__boardSizeChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<sbyte> __currentPlayerChange;
	public Action OnCurrentPlayerChange(PropertyChangeHandler<sbyte> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.currentPlayer));
		__currentPlayerChange += __handler;
		if (__immediate && this.currentPlayer != default(sbyte)) { __handler(this.currentPlayer, default(sbyte)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(currentPlayer));
			__currentPlayerChange -= __handler;
		};
	}

	protected event PropertyChangeHandler<sbyte> __winnerChange;
	public Action OnWinnerChange(PropertyChangeHandler<sbyte> __handler, bool __immediate = true) {
		if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
		__callbacks.AddPropertyCallback(nameof(this.winner));
		__winnerChange += __handler;
		if (__immediate && this.winner != default(sbyte)) { __handler(this.winner, default(sbyte)); }
		return () => {
			__callbacks.RemovePropertyCallback(nameof(winner));
			__winnerChange -= __handler;
		};
	}

	protected override void TriggerFieldChange(DataChange change) {
		switch (change.Field) {
			case nameof(playerOne): __playerOneChange?.Invoke((PlayerState) change.Value, (PlayerState) change.PreviousValue); break;
			case nameof(playerTwo): __playerTwoChange?.Invoke((PlayerState) change.Value, (PlayerState) change.PreviousValue); break;
			case nameof(tiles): __tilesChange?.Invoke((ArraySchema<TileState>) change.Value, (ArraySchema<TileState>) change.PreviousValue); break;
			case nameof(gamePieces): __gamePiecesChange?.Invoke((ArraySchema<GamePieceState>) change.Value, (ArraySchema<GamePieceState>) change.PreviousValue); break;
			case nameof(boardSize): __boardSizeChange?.Invoke((Vector2Schema) change.Value, (Vector2Schema) change.PreviousValue); break;
			case nameof(currentPlayer): __currentPlayerChange?.Invoke((sbyte) change.Value, (sbyte) change.PreviousValue); break;
			case nameof(winner): __winnerChange?.Invoke((sbyte) change.Value, (sbyte) change.PreviousValue); break;
			default: break;
		}
	}
}

