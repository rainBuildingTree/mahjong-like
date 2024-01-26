using UnityEngine;

public class AimController : MonoBehaviour {
    /* Loaded Components *///==================================================
    RectTransform rectTransform;
    EnemyObjectPool enemyObjectPool;
    Canvas userControlCanvas;
    Camera mainCam;
    protected AttackObjectPool _attackPool;
    protected Card _magicFrom;
    protected MagicModel.ElementalAttribute _attackAttribute;
    protected int _attackLevel;
    protected Transform _playerCharacter;



    /* Cached Variables *///==================================================
    Vector2 cachedMousePosition;
    Vector2 closestTargetPositionOnCanvas;
    Vector2 currentMousePositionOnCanvas;
    Vector2 playerCharacterPosition;
    Transform closestEnemy;
    float closestTargetDistanceOnCanvas;



    /* Member Variables *///==================================================
    float maxAttachDistance;
    protected Transform _target;



    /* Unity Event Functions *///==================================================
    void Awake() {
        rectTransform = GetComponent<RectTransform>();
        userControlCanvas = GetComponentInParent<Canvas>();
        enemyObjectPool = FindObjectOfType<EnemyObjectPool>();
        mainCam = FindObjectOfType<Camera>();
        _attackPool = FindObjectOfType<AttackObjectPool>();
        maxAttachDistance = rectTransform.rect.width;
        _playerCharacter = FindObjectOfType<PlayerCharacterManager>().transform;
        playerCharacterPosition = _playerCharacter.position;
    }
    void Start() {
        gameObject.SetActive(false);
    }
    void OnEnable() {
        Cursor.visible = false;
        JumpToMousePosition();
    }
    void OnDisable() {
        Cursor.visible = true;
        _magicFrom = null;
    }
    void Update() {
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



    /* Private Methods *///==================================================
    void JumpToMousePosition() {
        cachedMousePosition = Input.mousePosition;
        rectTransform.anchoredPosition = cachedMousePosition / userControlCanvas.scaleFactor;
    }
    void AttachToClosestEnemy() {
        closestEnemy = enemyObjectPool.ClosestEnemy(mainCam.ScreenToWorldPoint(cachedMousePosition));
        if (closestEnemy == null) {
            _target = null;
            return;
        }

        closestTargetPositionOnCanvas = mainCam.WorldToScreenPoint(closestEnemy.position) / userControlCanvas.scaleFactor;
        currentMousePositionOnCanvas = cachedMousePosition / userControlCanvas.scaleFactor;
        closestTargetDistanceOnCanvas = Mathf.Abs(Vector2.Distance(closestTargetPositionOnCanvas, currentMousePositionOnCanvas));

        if (closestTargetDistanceOnCanvas < maxAttachDistance) {
            rectTransform.anchoredPosition = closestTargetPositionOnCanvas;
            _target = closestEnemy;
        }
        else {
            _target = null;
        }
    }
    void AttachToPlayerCharacter() {
        if (_playerCharacter == null)
            return;
        closestTargetPositionOnCanvas = mainCam.WorldToScreenPoint(_playerCharacter.position) / userControlCanvas.scaleFactor;
        currentMousePositionOnCanvas = cachedMousePosition / userControlCanvas.scaleFactor;
        closestTargetDistanceOnCanvas = Mathf.Abs(Vector2.Distance(closestTargetPositionOnCanvas, currentMousePositionOnCanvas));
        
        if (closestTargetDistanceOnCanvas < maxAttachDistance) {
            rectTransform.anchoredPosition = closestTargetPositionOnCanvas;
            _target = _playerCharacter;
        }
        
    }

}
