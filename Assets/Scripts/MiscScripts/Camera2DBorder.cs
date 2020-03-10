using UnityEngine;

public enum CameraBorder { TopBorder, BottomBorder, RightBorder, LeftBorder };
public class Camera2DBorder : MonoBehaviour
{
    [SerializeField] private Camera2D _camera2D;
    [SerializeField] private CameraBorder _cameraBorder;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            _camera2D.SetFollowPlayer(collision.transform, _cameraBorder);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            _camera2D.SetFollowPlayer(false, _cameraBorder);
        }
    }
}
