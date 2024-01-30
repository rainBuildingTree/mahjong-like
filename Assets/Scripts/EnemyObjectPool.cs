using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObjectPool : MonoBehaviour {
    /* MEMBER VARIABLES *///==================================================
    [SerializeField] GameObject _enemyPrefab;

    const int PoolSize = 30;

    int _currentRound = 0;

    EnemyManager[] _enemyPool;
    int[] _roundLength = new int[3] { 10, 10, 10 };
    EnemyManager.MonsterType[,] _roundData = new EnemyManager.MonsterType[3, 10] { // TEMPORARY ONLY!!!
        {EnemyManager.MonsterType.RedSlime, EnemyManager.MonsterType.None, EnemyManager.MonsterType.None, EnemyManager.MonsterType.GreenSlime, EnemyManager.MonsterType.None, EnemyManager.MonsterType.None, EnemyManager.MonsterType.BlueSlime, EnemyManager.MonsterType.None, EnemyManager.MonsterType.None, EnemyManager.MonsterType.None},
        {EnemyManager.MonsterType.RedSlime, EnemyManager.MonsterType.None, EnemyManager.MonsterType.BlueSlime, EnemyManager.MonsterType.GreenSlime, EnemyManager.MonsterType.None, EnemyManager.MonsterType.GreenSlime, EnemyManager.MonsterType.BlueSlime, EnemyManager.MonsterType.None, EnemyManager.MonsterType.None, EnemyManager.MonsterType.RedSlime },
        {EnemyManager.MonsterType.RedSlime, EnemyManager.MonsterType.RedSlime, EnemyManager.MonsterType.BlueSlime, EnemyManager.MonsterType.GreenSlime, EnemyManager.MonsterType.BlueSlime, EnemyManager.MonsterType.GreenSlime, EnemyManager.MonsterType.BlueSlime, EnemyManager.MonsterType.GreenSlime, EnemyManager.MonsterType.None, EnemyManager.MonsterType.RedSlime }
    };


    /* UNITY EVENT FUNCTIONS *///==================================================
    void Awake() {
        InitializeEnemyPool();
    }



    /* PUBLIC METHODS *///==================================================
    public void PlayRound() {
        if (IsRoundPlaying())
            return;
        StartCoroutine(ProcessRound());
    }
    public Transform ClosestEnemy(Vector2 position) {
        float closestDistance = float.MaxValue;
        int closestEnemyIndex = -1;
        for (int i = 0; i < PoolSize; ++i) {
            if (!_enemyPool[i].gameObject.activeInHierarchy)
                continue;
            float distance = Vector2.Distance(_enemyPool[i].transform.position, position);
            if (distance < closestDistance) {
                closestDistance = distance;
                closestEnemyIndex = i;
            }
        }
        if (closestEnemyIndex < 0)
            return null;
        return _enemyPool[closestEnemyIndex].transform;
    }



    /* PRIVATE METHODS *///==================================================
    void EnableEnemyInPool(EnemyManager.MonsterType monsterType) {
        if (monsterType == EnemyManager.MonsterType.None)
            return;
        for (int i = 0; i < PoolSize; ++i) {
            if (!_enemyPool[i].gameObject.activeInHierarchy) {
                _enemyPool[i].gameObject.SetActive(true);
                _enemyPool[i].SetMonsterType(monsterType);
                _enemyPool[i].InitializeEnemy();
                return;
            }
        }
    }
    bool IsRoundPlaying() {
        for (int i = 0; i < PoolSize; ++i) {
            if (_enemyPool[i].gameObject.activeInHierarchy) {
                return true;
            }
        }
        return false;
    }
    void InitializeEnemyPool() {
        _enemyPool = new EnemyManager[PoolSize];
        for (int i = 0; i < PoolSize; ++i) {
            _enemyPool[i] = Instantiate(_enemyPrefab, transform).GetComponent<EnemyManager>();
            _enemyPool[i].gameObject.SetActive(false);
        }
    }



    /* IENUMERATORS *///==================================================
    IEnumerator ProcessRound() {
        yield return new WaitForEndOfFrame();
        for (int i = 0; i < _roundLength[_currentRound]; ++i) {
            EnemyManager.MonsterType monsterTypeToCreate = _roundData[_currentRound, i];
            EnableEnemyInPool(monsterTypeToCreate);
            yield return new WaitForSeconds(1);
        }

        while (IsRoundPlaying()) {
            yield return new WaitForSeconds(1);
        }

        _currentRound++;
        Debug.Log("Round Finished");
        yield return null;
    }



    /* TEST CODES *///==================================================
    public void SetCurrentRound(int round) {
        _currentRound = round;
        Debug.Log("Current Round is set to: " + _currentRound.ToString());
    }

}
