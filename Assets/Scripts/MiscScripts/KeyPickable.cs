using System.Collections;
using UnityEngine;

public class KeyPickable : MonoBehaviour, IPickable
{
    [SerializeField] BoxCollider2D _triggerCollider;
    [SerializeField] BoxCollider2D _normalCollider;
    private readonly PickableType _pickableType = PickableType.Key;


    public void Picked()
    {
        _triggerCollider.enabled = false;
    }

    public void Throw()
    {
        StartCoroutine(ThrowCoroutine());
    }

    IEnumerator ThrowCoroutine()
    {
        transform.SetParent(null);
        _normalCollider.enabled = true;
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.isKinematic = false;
        Vector3 dir = Quaternion.AngleAxis(45, Vector3.forward) * Vector2.right;
        rigidbody.gravityScale = 4;
        rigidbody.AddForce(dir * 400);
        yield return new WaitForSeconds(0.37f);
        rigidbody.isKinematic = true;
        rigidbody.velocity = Vector2.zero;

        _normalCollider.enabled = false;
        _triggerCollider.enabled = true;
    }

    public PickableType GetPickableType()
    {
        return _pickableType;
    }
}
