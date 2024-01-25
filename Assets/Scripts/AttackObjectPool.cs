using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackObjectPool : MonoBehaviour {
    [SerializeField] GameObject _prefab;
    protected int _size = 14;
    protected AttackManager[] _pool;
    protected Vector3 _spawnPosition;

    void Awake() {
        _spawnPosition = FindObjectOfType<PlayerCharacterManager>().transform.position;
        Init();
    }

    public void Init() {
        _pool = new AttackManager[_size];
        for (int i = 0; i < _size; ++i) {
            _pool[i] = Instantiate(_prefab, transform).GetComponent<AttackManager>();
            _pool[i].gameObject.SetActive(false);
        }
    }

    public void EnableObject(MagicModel.ElementalAttribute attribute, int level, Transform target) {
        if (target == null) {
            Debug.Log("NO TARGET!");
            return;
        }
        for (int i = 0; i < _size; ++i) {
            if (_pool[i].gameObject.activeInHierarchy)
                continue;
            _pool[i].gameObject.SetActive(true);
            _pool[i].transform.position = _spawnPosition;
            _pool[i].Init(attribute, level, target);
            return;
        }
    }
    
}
