using UnityEngine;

public class AttackManager : MonoBehaviour {
    protected MagicalProjectile _projectile;
    protected MagicModel _model;
    protected SpriteRenderer _renderer;

    public MagicalProjectile Projectile { get { return _projectile; } }
    public MagicModel Model { get { return _model; } }
    public SpriteRenderer Renderer { get { return _renderer; } }

    protected void Awake() {
        _projectile = gameObject.AddComponent<MagicalProjectile>();
        _model = gameObject.AddComponent<MagicModel>();
        _renderer = GetComponent<SpriteRenderer>();
    }
    public void Init(MagicModel.ElementalAttribute attribute, int level, Transform target) {
        _model.Init(attribute, level);
        _projectile.RegisterTarget(target);
    }
}
