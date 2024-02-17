using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackObjectPool : MonoBehaviour {
    [SerializeField] GameObject _prefab;
    protected int _size = 14;
    protected AttackManager[] _pool;
    protected Vector3 _spawnPosition;
    protected Transform _playerCharacter;
    protected int[] _elementalAtk;
    protected AttackIndicator _attackIndicator;

    protected void Awake() {
        _playerCharacter = FindObjectOfType<PlayerCharacterManager>().transform;
        _attackIndicator = FindObjectOfType<AttackIndicator>();
        Init();
    }

    public void Init() {
        _pool = new AttackManager[_size];
        for (int i = 0; i < _size; ++i) {
            _pool[i] = Instantiate(_prefab, transform).GetComponent<AttackManager>();
            _pool[i].gameObject.SetActive(false);
        }
        _elementalAtk = new int[4];
        for (int i = 0; i < 4; ++i) {
            _elementalAtk[i] = 25;
            if (_attackIndicator != null)
                _attackIndicator.UpdateAttackDamage((MagicModel.ElementalAttribute)i, _elementalAtk[i]);
        }
        
    }
    public void EnableObject(MagicModel.ElementalAttribute attribute, int level, Transform target) {
        if (target == null) {
            return;
        }
        if (target == _playerCharacter) {
            EnhancePlayer(attribute, level, target);
            return;
        }

        _spawnPosition = (Vector2)_playerCharacter.position + Vector2.left;
        for (int i = 0; i < _size; ++i) {
            if (_pool[i].gameObject.activeInHierarchy)
                continue;
            _pool[i].gameObject.SetActive(true);
            _pool[i].transform.position = _spawnPosition;
            Debug.Log(_elementalAtk[(int)attribute]);
            _pool[i].Init(attribute, level, target, _elementalAtk[(int)attribute]);
            return;
        }
    }

    protected void EnhancePlayer(MagicModel.ElementalAttribute attribute, int level, Transform target) {
        _elementalAtk[(int)attribute] += (level < 2) ? 1 : 10;
        _elementalAtk[3] += (level < 2) ? 0 : 4;
        if (_attackIndicator != null)
        {
            _attackIndicator.UpdateAttackDamage(attribute, _elementalAtk[(int)attribute]);
            _attackIndicator.UpdateAttackDamage(MagicModel.ElementalAttribute.None, _elementalAtk[3]);
        }
        return;
    }
}
