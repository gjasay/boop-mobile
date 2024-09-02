using UnityEngine;

public class GamePiece : MonoBehaviour
{
  private void Start()
  {
    Destroy(GetComponent<PieceDragging>());
  }
}
