using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpringJoint2D))]

public class SpiderSwing2D : MonoBehaviour
{
    [Header("設定")]
    [SerializeField] private LayerMask _grappleLayer;  // 糸をつけることができるレイヤー
    [SerializeField] private float _maxDistance = 15f; // 糸の最大距離

    private Rigidbody2D _rigidbody2D;
    private SpringJoint2D _springJoint2D;
    private Camera _mainCamera;
    private PlayerInput _playerInput;
    private InputAction _shootWebAction;
    private InputAction _lineScaleAction;

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _springJoint2D = GetComponent<SpringJoint2D>();
        _mainCamera = Camera.main;
        _springJoint2D.enabled = false; // 初期状態では糸を無効化
        _playerInput = GetComponent<PlayerInput>();
        _shootWebAction = _playerInput.actions["MouseLeft"];
        _shootWebAction.Enable();
        _lineScaleAction = _playerInput.actions["Scale"];
        _lineScaleAction.Enable();
    }

    void Update()
    {
        if (_shootWebAction.WasCompletedThisFrame())
        {
            ShootWeb();
        }
        else if (_shootWebAction.WasReleasedThisFrame())
        {
            ReleaseWeb();
        }
        if (_springJoint2D.enabled)
        {
            var scroll = _lineScaleAction.ReadValue<Vector2>();
            if (Mathf.Abs(scroll.y) > 0.01f)
            {
                _springJoint2D.distance = Mathf.Clamp(_springJoint2D.distance - scroll.y * Time.deltaTime * 10f, 1f, _maxDistance); // 糸の距離を制限
            }
        }
    }

    private void ShootWeb()
    {
        if (Mouse.current == null) return;  // マウスが存在しない場合は処理を中断
        var mPos = Mouse.current.position.ReadValue();  // マウスの位置を取得
        Vector2 mousePosition = _mainCamera.ScreenToWorldPoint(mPos); // マウスの位置を取得
        Vector2 firePoint = transform.position; // 発射位置を取得
        Vector2 direction = (mousePosition - firePoint).normalized; // 発射方向を計算

        RaycastHit2D hit = Physics2D.Raycast(firePoint, direction, _maxDistance, _grappleLayer); // レイキャストを飛ばす

        if (hit.collider != null)
        {
            _springJoint2D.enabled = true;                                    // 糸を有効化
            _springJoint2D.connectedAnchor = hit.point;                       // 糸の接続点を設定
            _springJoint2D.distance = Vector2.Distance(firePoint, hit.point); // 糸の距離を設定
        }
    }

    /// <summary>
    /// 糸を無効化する処理
    /// </summary>

    private void ReleaseWeb()
    {
        _springJoint2D.enabled = false;  // 糸を無効化
    }
}
