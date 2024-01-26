using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterManager : MonoBehaviour {
    Vector2 _playerPosition;
    float sinVal;

    void Awake() {
        _playerPosition = transform.position;
    }

    void Update() {
        sinVal = Mathf.Sin(Time.time * 4f);
        transform.position = _playerPosition + (Vector2.up * sinVal * 0.1f);
    }
}
