using UnityEngine;

public class MagicalProjectile : MonoBehaviour {
    protected Vector2 _vectorToTarget;
    protected Vector2 _vectorToRotate;
    protected Vector2 _moveVector;
    protected Transform _target = null;
    protected float _gravitySpeed = 10f;
    protected float _rotateSpeed = 6f;

    protected void FixedUpdate() {
        ChaseTarget();
    }
    protected void OnTriggerEnter2D(Collider2D other) {
        if (other.transform != _target)
            return;
        gameObject.SetActive(false);
    }
    public void RegisterTarget(Transform target) {
        _target = target;
    }
    protected void ChaseTarget() {
        if (_target == null)
            return;
        _vectorToTarget = ((Vector2)_target.position - (Vector2)transform.position).normalized;
        _vectorToRotate = new Vector2(_vectorToTarget.y, -_vectorToTarget.x);
        _moveVector = _vectorToTarget * _gravitySpeed + _vectorToRotate * _rotateSpeed;
        _moveVector *= Time.deltaTime;
        transform.position += (Vector3)_moveVector;
    }
}
