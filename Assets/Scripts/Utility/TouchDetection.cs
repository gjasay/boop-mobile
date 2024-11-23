using UnityEngine;

public class TouchDetection : MonoBehaviour
{
  public static TouchDetection Instance { get; private set; }

  private bool _isTouching;
  private Camera _camera;

  private void Start()
  {
    _camera = Camera.main;
  }

  private void Awake()
  {
    if (Instance == null)
    {
      Instance = this;
      DontDestroyOnLoad(gameObject);
    }
    else
    {
      Destroy(gameObject);
    }
  }

  private void Update()
  {
    _isTouching = DetectTouch();
  }

  public Vector3? GetPosition()
  {
    if (_isTouching)
    {
      Touch touch = Input.GetTouch(0);
      if (_camera) return _camera?.ScreenToWorldPoint(touch.position);
    }
    else
    {
      return null;
    }

    return null;
  }

  private bool DetectTouch()
  {
    if (Input.touchCount <= 0) return false;
    Touch touch = Input.GetTouch(0);
    return touch.phase == TouchPhase.Began;

  }
}