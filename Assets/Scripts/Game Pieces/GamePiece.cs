using System.Collections;
using UnityEngine;

public class GamePiece : MonoBehaviour
{
  public string pieceType { private set; get; }
  public int playerID { private set; get; }
  public Vector2Int Coordinate { private set; get; }
  public GameTile GameTilePlacement { private set; get; }
  public bool pieceTypeSet = false;
  public bool clientPlacement = false;

  private void Start() {}

  public void Initialize(GamePieceState state)
  {
    pieceType = state.type;
    playerID = state.playerId;
    this.gameObject.name = state.type + "_" + state.playerId;
    GetComponent<SpriteRenderer>().sprite =
        ResourceManager.Instance.GetSprite(state.type, state.playerId);
    if (state.coordinate.x == -1 || state.coordinate.y == -1) {
      GetComponent<SpriteRenderer>().enabled = false;
    } else {
      SetTilePlacement(GameboardManager.Instance.GameTiles[state.coordinate.x, state.coordinate.y]);
    }
    Coordinate = new Vector2Int(state.coordinate.x, state.coordinate.y);
  }

  public void HandleCoordinateChange(Vector2Schema cur, Vector2Schema prev)
  {
    if (prev == null || cur == null)
      return;

    if (cur?.x == -1 || cur?.y == -1) {
      if (prev?.x == -1 || prev?.y == -1)
        return;
      StartCoroutine(MovePieceToHand(0.5f));
    } else if (prev?.x != -1 || prev?.y != -1) {
      StartCoroutine(MovePieceToTile(GameboardManager.Instance.GameTiles[prev.x, prev.y],
                                     GameboardManager.Instance.GameTiles[cur.x, cur.y], 0.5f));
    } else {
      SetTilePlacement(GameboardManager.Instance.GameTiles[cur.x, cur.y]);
      GetComponent<SpriteRenderer>().enabled = true;
    }
  }

  public void HandleTypeChange(string cur, string prev)
  {
    if (cur == null || prev == null)
      return;
    pieceType = cur;
    GetComponent<SpriteRenderer>().sprite = ResourceManager.Instance.GetSprite(cur, playerID);
    this.gameObject.name = cur + "_" + playerID;
  }

  public void SetTilePlacement(GameTile gameTile)
  {
    if (GameTilePlacement == gameTile)
      return;
    if (GameTilePlacement != null)
      GameTilePlacement.CurrentlyHeldPiece = null;

    GameTilePlacement = gameTile;
    gameTile.CurrentlyHeldPiece = this;
    transform.position = gameTile.transform.position;
    if (!GetComponent<DragHandler>())
      return;
    Destroy(GetComponent<DragHandler>());
  }

  public void ResetTilePlacement(GameTile gameTile)
  {
    if (GameTilePlacement)
      GameTilePlacement.CurrentlyHeldPiece = null;
    GameTilePlacement = null;
  }

  IEnumerator MovePieceToTile(GameTile origin, GameTile destination, float duration)
  {
    if (!origin.CurrentlyHeldPiece)
      yield break;

    float time = 0;
    Vector3 startPosition = origin.CurrentlyHeldPiece.transform.position;
    Vector3 endPosition = destination.transform.position;

    while (time < duration) {
      origin.CurrentlyHeldPiece.transform.position =
          Vector3.Lerp(startPosition, endPosition, time / duration);
      time += Time.deltaTime;
      yield return null;
    }
    transform.position = endPosition;
    origin.CurrentlyHeldPiece?.SetTilePlacement(destination);
  }

  IEnumerator MovePieceToHand(float duration, float delay = 0)
  {
    if (!GameTilePlacement)
      yield break;
    float time = 0;
    Vector3 startPosition = GameTilePlacement.transform.position;
    Vector3 endPosition;
    switch (pieceType) {
      case "kitten" when playerID == NetworkManager.Instance.PlayerId:
        endPosition = GameObject.Find("UIKitten").transform.position;
        break;
      case "cat" when playerID == NetworkManager.Instance.PlayerId:
        endPosition = GameObject.Find("UICat").transform.position;
        break;
      default:
        endPosition = new Vector3(0, 4, 0);
        break;
    }

    yield return new WaitForSeconds(delay);

    while (time < duration) {
      transform.position = Vector3.Lerp(startPosition, endPosition, time / duration);
      time += Time.deltaTime;
      yield return null;
    }

    transform.position = endPosition;
    GetComponent<SpriteRenderer>().enabled = false;
    ResetTilePlacement(GameTilePlacement);
  }
}
