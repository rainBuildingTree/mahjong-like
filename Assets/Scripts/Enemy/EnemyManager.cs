using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyManager : MonoBehaviour {
    /* ENUMS *///==================================================
    public enum MonsterType { RedSlime, GreenSlime, BlueSlime, NormalSlime, None }
    public enum ElementalAttribute { Pyro, Anemo, Hydro, None }



    /* MEMBER VARIABLES *///==================================================
    [SerializeField] Sprite[] _sprites;

    SpriteRenderer _spriteRenderer;
    [SerializeField] SpriteRenderer _hitSpriteRenderer;
    EnemyMovement _movement;
    EnemyHealth _health;
    TMP_Text _tmpText;
    DamageIndicator _damageIndicator;

    MonsterType _monsterType = MonsterType.None;
    ElementalAttribute _elementalAttribute = ElementalAttribute.None;

    public TMP_Text TmpText { get { return _tmpText; } }
    public SpriteRenderer HitSpriteRenderer { get { return _hitSpriteRenderer; } }
    public EnemyMovement Movement { get { return _movement; } }
    public DamageIndicator DamageIndicator { get { return _damageIndicator; } }
    


    /* UNITY EVENT FUNCTIONS *///==================================================
    void Awake() {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _movement = GetComponent<EnemyMovement>();
        _health = gameObject.AddComponent<EnemyHealth>();
        _tmpText = GetComponentInChildren<TMP_Text>();
        _damageIndicator = FindObjectOfType<DamageIndicator>();
        _hitSpriteRenderer.enabled = false;
    }

    



    /* PUBLIC METHODS *///==================================================
    public void SetMonsterType(MonsterType monsterType) {
        _monsterType = monsterType;
        LoadEnemyData();
        LoadEnemySprite();
    }
    public void InitializeEnemy() {
        _health.Init();
        _movement.InitializeMovement();
    }



    /* PRIVATE METHODS *///==================================================
    void LoadEnemyData() {
        switch (_monsterType) {
            case MonsterType.RedSlime:
                _health.SetWeakness(ElementalAttribute.Pyro);
                break;
            case MonsterType.GreenSlime:
                _health.SetWeakness(ElementalAttribute.Anemo);
                break;
            case MonsterType.BlueSlime:
                _health.SetWeakness(ElementalAttribute.Hydro);
                break;
            default:
                _health.SetWeakness(ElementalAttribute.None);
                break;
        }
    }
    void LoadEnemySprite() {
        _spriteRenderer.sprite = _sprites[(int)_monsterType];
    }

}
