using UnityEngine;

public class AttackManager : MonoBehaviour {
    protected MagicalProjectile _projectile;
    protected MagicModel _model;
    protected SpriteRenderer _renderer;
    [SerializeField] protected MagicSpriteSO _spriteStorage;
    protected ParticleSystem.EmissionModule _particleSystemEmssion;
    protected ParticleSystemRenderer _particleSystemRenderer;
    protected int _attackDamage;
    

    public MagicalProjectile Projectile { get { return _projectile; } }
    public MagicModel Model { get { return _model; } }
    public SpriteRenderer Renderer { get { return _renderer; } }
    public MagicSpriteSO SpriteStorage { get { return _spriteStorage; } }
    public ParticleSystem.EmissionModule ParticleSystemEmission { get { return _particleSystemEmssion; } }
    public int AttackDamage { get { return _attackDamage; } }

    protected void Awake() {
        _projectile = gameObject.AddComponent<MagicalProjectile>();
        _model = gameObject.AddComponent<MagicModel>();
        _renderer = GetComponent<SpriteRenderer>();
        _particleSystemEmssion = GetComponentInChildren<ParticleSystem>().emission;
        _particleSystemRenderer = GetComponentInChildren<ParticleSystem>().GetComponent<ParticleSystemRenderer>();
        _projectile.RegisterManager(this);
    }
    public void Init(MagicModel.ElementalAttribute attribute, int level, Transform target, int attackDamage) {
        _model.Init(attribute, level);
        _projectile.RegisterTarget(target);
        _attackDamage = attackDamage;
    }
    public void SetParticleEmissionEnable(bool isEnabled) {
        _particleSystemEmssion.enabled = isEnabled;
    }
    public void SetParticleMaterial(MagicModel.ElementalAttribute attribute) {
        _particleSystemRenderer.material = _spriteStorage.GetMaterial(attribute);
    }
}
