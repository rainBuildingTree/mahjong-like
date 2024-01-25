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



    /* Cached Variables *///==================================================
    Vector2 cachedMousePosition;
    Vector2 closestEnemyPositionOnCanvas;
    Vector2 currentMousePositionOnCanvas;
    Transform closestEnemy;
    float closestEnemyDistanceOnCanvas;



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

        closestEnemyPositionOnCanvas = mainCam.WorldToScreenPoint(closestEnemy.position) / userControlCanvas.scaleFactor;
        currentMousePositionOnCanvas = cachedMousePosition / userControlCanvas.scaleFactor;
        closestEnemyDistanceOnCanvas = Mathf.Abs(Vector2.Distance(closestEnemyPositionOnCanvas, currentMousePositionOnCanvas));

        if (closestEnemyDistanceOnCanvas < maxAttachDistance) {
            rectTransform.anchoredPosition = closestEnemyPositionOnCanvas;
            _target = closestEnemy;
        }
        else {
            _target = null;
        }
    }

}
