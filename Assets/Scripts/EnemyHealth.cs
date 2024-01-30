using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {
    protected int _health;
    protected int _maxHealth = 100;
    protected float _criticalMultiplier = 4f;
    protected EnemyManager.ElementalAttribute _weakness;

    public void Init() {
        _health = _maxHealth;
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
    }
    public void SetWeakness(EnemyManager.ElementalAttribute weakAttribute) {
        _weakness = weakAttribute;
    }
}
