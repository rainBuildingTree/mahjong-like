using System.Collections;
using UnityEngine;

public class MagicalProjectile : MonoBehaviour {
    protected Vector2 _vectorToTarget;
    protected Vector2 _vectorToRotate;
    protected Vector2 _moveVector;
    protected Transform _target = null;
    protected float _gravitySpeed = 10f;
    protected float _rotateSpeed = 5f;
    protected AttackManager _manager;
    protected bool _isDestroyed = false;

    protected void FixedUpdate() {
        ChaseTarget();
    }
    protected void OnTriggerEnter2D(Collider2D other) {
        if (other.transform != _target)
            return;
        if (!_isDestroyed) {
            _isDestroyed = true;
            other.GetComponent<EnemyHealth>().Damage(25, _manager.Model.Attribute);
            StartCoroutine(DelayedDestroy());
        }
    }
    public void RegisterTarget(Transform target) {
        _target = target;
        _manager.SetParticleEmissionEnable(true);
        _manager.Renderer.enabled = true;
        _isDestroyed = false;
    }
    public void RegisterManager(AttackManager manager) {
        _manager = manager;
    }
    protected void ChaseTarget() {
        if (_target == null || !_target.gameObject.activeInHierarchy)
            return;
        _vectorToTarget = ((Vector2)_target.position + Vector2.right * 0.5f - (Vector2)transform.position).normalized;
        _vectorToRotate = new Vector2(_vectorToTarget.y, -_vectorToTarget.x);
        _moveVector = _vectorToTarget * _gravitySpeed + _vectorToRotate * _rotateSpeed;
        _moveVector *= Time.deltaTime;
        float rotZ = Mathf.Atan2(-_vectorToTarget.y, -_vectorToTarget.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotZ - 15f);
        transform.position += (Vector3)_moveVector; 
    }
    IEnumerator DelayedDestroy() {
        _manager.Renderer.enabled = false;
        _manager.SetParticleEmissionEnable(false);
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
