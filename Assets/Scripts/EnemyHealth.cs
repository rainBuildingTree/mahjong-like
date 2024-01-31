using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {
    protected int _health;
    protected int _maxHealth = 100;
    protected float _criticalMultiplier = 4f;
    protected EnemyManager.ElementalAttribute _weakness;
    protected EnemyManager _manager;

    protected void Awake() {
        _manager = GetComponent<EnemyManager>();
    }
    public void Init() {
        _health = _maxHealth;
        _manager.TmpText.text = _health.ToString();
    }

    public void Damage(int amount, MagicModel.ElementalAttribute elementalAttribute) {
        if ((int)_weakness == (int)elementalAttribute || elementalAttribute == MagicModel.ElementalAttribute.None) {
            _health -= (int)(amount * _criticalMultiplier);
        }
        else {
            _health -= amount;
        }
        if (_health <= 0)
            gameObject.SetActive(false);
        _manager.TmpText.text = _health.ToString();
    }
    public void SetWeakness(EnemyManager.ElementalAttribute weakAttribute) {
        _weakness = weakAttribute;
    }
}
