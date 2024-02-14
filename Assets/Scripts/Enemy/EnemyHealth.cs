using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {
    protected int _health;
    protected int _maxHealth = 100;
    protected float _criticalMultiplier = 4f;
    protected EnemyManager.ElementalAttribute _weakness;
    protected EnemyManager _manager;

    public EnemyManager.ElementalAttribute Weakness { get { return _weakness; } }

    protected void Awake() {
        _manager = GetComponent<EnemyManager>();
    }
    public void Init() {
        _health = _maxHealth;
        _manager.HitSpriteRenderer.enabled = false;
        _manager.TmpText.text = _health.ToString();
    }

    public void Damage(int amount, MagicModel.ElementalAttribute elementalAttribute) {
        int finalDamage = 0;
        if (((int)_weakness == (int)elementalAttribute || elementalAttribute == MagicModel.ElementalAttribute.None) && _weakness != EnemyManager.ElementalAttribute.None) {
            finalDamage = (int)(amount * _criticalMultiplier);
        }
        else {
            finalDamage = amount;
        }
        _health -= finalDamage;
        _manager.TmpText.text = _health.ToString();
        StartCoroutine(ProcessHitVisual(finalDamage));
        if (_health <= 0)
            gameObject.SetActive(false);
    }
    public void SetWeakness(EnemyManager.ElementalAttribute weakAttribute) {
        _weakness = weakAttribute;
    }
    public void SetMaxHealth(int maxHealth) {
        _maxHealth = maxHealth;
    }
    IEnumerator ProcessHitVisual(int damageAmount) {
        _manager.HitSpriteRenderer.enabled = true;
        _manager.Movement.Knockback();
        _manager.DamageIndicator.ShowDamage(transform.position, damageAmount);
        yield return new WaitForSeconds(0.1f);
        _manager.HitSpriteRenderer.enabled = false;
    }
}
