using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterManager : MonoBehaviour {
    protected Vector2 _playerPosition;
    protected float sinVal;
    

    public int[] ElementalAtk { get; set; }
    protected void Awake() {
        _playerPosition = transform.position;
    }
    protected void Update() {
        sinVal = Mathf.Sin(Time.time * 4f);
        transform.position = _playerPosition + (0.1f * sinVal * Vector2.up);
    }
}
