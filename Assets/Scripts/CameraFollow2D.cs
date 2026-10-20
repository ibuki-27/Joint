using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _smoothSpeed = 5f;

    private void LateUpdate()
    {
        if (_target == null) return;

        Vector3 targetPosition = new Vector3(_target.position.x, _target.position.y, transform.position.z);

        transform.position = Vector3.Lerp(transform.position, targetPosition, _smoothSpeed * Time.deltaTime);
    }
}
