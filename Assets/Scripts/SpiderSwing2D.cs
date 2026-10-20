using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpringJoint2D))]

public class SpiderSwing2D : MonoBehaviour
{
    [Header("設定")]
    [SerializeField] private LayerMask _grappleLayer;  // 糸をつけることができるレイヤー
    [SerializeField] private float _maxDistance = 15f; // 糸の最大距離

    [SerializeField] private LineRenderer _lineRenderer;

    [SerializeField] private float _swingForce = 10f;

    private Rigidbody2D _rigidbody2D;
    private SpringJoint2D _springJoint2D;
    private Camera _mainCamera;
    private PlayerInput _playerInput;
    private InputAction _shootWebAction;
    private InputAction _lineScaleAction;
    private InputAction _moveAction;
    private InputAction _releaseAction;

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
        _lineRenderer.positionCount = 2;
        _lineRenderer.enabled = false;
        _moveAction = _playerInput.actions["Move"];
        _moveAction.Enable();
        _releaseAction = _playerInput.actions["Release"];
        _releaseAction.Enable();
    }

    void Update()
    {
        if (_shootWebAction.WasPressedThisFrame())
        {
            ShootWeb();
        }

        if (_releaseAction.WasPressedThisFrame())
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

        if (_releaseAction.WasPressedThisFrame())
        {
            ReleaseWeb();
        }
    }

    private void FixedUpdate()
    {
        if (!_springJoint2D.enabled) return;

        float moveInput = _moveAction.ReadValue<float>();

        _rigidbody2D.AddForce(Vector2.right * moveInput * _swingForce);
    }

    private void LateUpdate()
    {
        if (!_springJoint2D.enabled) return;

        _lineRenderer.SetPosition(0, transform.position);
        _lineRenderer.SetPosition(1, _springJoint2D.connectedAnchor);
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
            _lineRenderer.enabled = true;
        }
    }

    /// <summary>
    /// 糸を無効化する処理
    /// </summary>

    private void ReleaseWeb()
    {
        _springJoint2D.enabled = false;  // 糸を無効化
        _lineRenderer.enabled = false;
    }
}
