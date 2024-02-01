using UnityEngine;

public class AimController : MonoBehaviour {
    protected RectTransform _rectTransform;
    protected EnemyObjectPool _enemyObjectPool;
    protected Canvas _userControlCanvas;
    protected Camera _mainCam;
    protected AttackObjectPool _attackPool;
    protected Card _magicFrom;
    protected MagicModel.ElementalAttribute _attackAttribute;
    protected int _attackLevel;
    protected Transform _playerCharacter;
    protected float _maxAttachDistance;
    protected Transform _target;

    private Vector2 cachedMousePosition;
    private Vector2 closestTargetPositionOnCanvas;
    private Vector2 currentMousePositionOnCanvas;
    private Vector2 playerCharacterPosition;
    private Transform closestEnemy;
    private float closestTargetDistanceOnCanvas;

    protected void Awake() {
        _rectTransform = GetComponent<RectTransform>();
        _userControlCanvas = GetComponentInParent<Canvas>();
        _enemyObjectPool = FindObjectOfType<EnemyObjectPool>();
        _mainCam = FindObjectOfType<Camera>();
        _attackPool = FindObjectOfType<AttackObjectPool>();
        _maxAttachDistance = _rectTransform.rect.width;
        _playerCharacter = FindObjectOfType<PlayerCharacterManager>().transform;
        playerCharacterPosition = _playerCharacter.position;
    }
    protected void Start() {
        gameObject.SetActive(false);
    }
    protected void OnEnable() {
        Cursor.visible = false;
        JumpToMousePosition();
    }
    protected void OnDisable() {
        Cursor.visible = true;
        _magicFrom = null;
    }
    protected void Update() {
        JumpToMousePosition();
        AttachToClosestEnemy();
        AttachToPlayerCharacter();
    }

    public void RegisterMagicCard(Card card) {
        _magicFrom = card;
        _attackAttribute = (MagicModel.ElementalAttribute)card.Attribute;
        _attackLevel = (_magicFrom is MergedCard) ? 2 : 1;
    }
    public bool IsTargetFound() {
        _attackPool.EnableObject(_attackAttribute, _attackLevel, _target);
        return _target != null;
    }

    protected void JumpToMousePosition() {
        cachedMousePosition = Input.mousePosition;
        _rectTransform.anchoredPosition = cachedMousePosition / _userControlCanvas.scaleFactor;
    }
    protected void AttachToClosestEnemy() {
        closestEnemy = _enemyObjectPool.ClosestEnemy(_mainCam.ScreenToWorldPoint(cachedMousePosition));
        if (closestEnemy == null) {
            _target = null;
            return;
        }

        closestTargetPositionOnCanvas = _mainCam.WorldToScreenPoint(closestEnemy.position) / _userControlCanvas.scaleFactor;
        currentMousePositionOnCanvas = cachedMousePosition / _userControlCanvas.scaleFactor;
        closestTargetDistanceOnCanvas = Mathf.Abs(Vector2.Distance(closestTargetPositionOnCanvas, currentMousePositionOnCanvas));

        if (closestTargetDistanceOnCanvas < _maxAttachDistance) {
            _rectTransform.anchoredPosition = closestTargetPositionOnCanvas;
            _target = closestEnemy;
        }
        else {
            _target = null;
        }
    }
    protected void AttachToPlayerCharacter() {
        if (_playerCharacter == null)
            return;
        closestTargetPositionOnCanvas = _mainCam.WorldToScreenPoint(_playerCharacter.position) / _userControlCanvas.scaleFactor;
        currentMousePositionOnCanvas = cachedMousePosition / _userControlCanvas.scaleFactor;
        closestTargetDistanceOnCanvas = Mathf.Abs(Vector2.Distance(closestTargetPositionOnCanvas, currentMousePositionOnCanvas));
        
        if (closestTargetDistanceOnCanvas < _maxAttachDistance) {
            _rectTransform.anchoredPosition = closestTargetPositionOnCanvas;
            _target = _playerCharacter;
        }
        
    }

}
