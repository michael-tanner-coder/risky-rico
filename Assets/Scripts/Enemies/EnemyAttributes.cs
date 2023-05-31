using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

[CreateAssetMenu(fileName = "EnemyAttributes", menuName = "Enemies/Enemy Attributes", order = 1)]
public class EnemyAttributes : AttributeSet, IResetOnExitPlay
{
    #region Combat Attributes
    [Header("MOVEMENT")]
    [SerializeField] private ModifiableAttribute _moveSpeed;
    public ModifiableAttribute MoveSpeed => _moveSpeed;
    
    [SerializeField] private ModifiableAttribute _rotationFrequency;
    public ModifiableAttribute RotationFrequency => _rotationFrequency;

    [SerializeField] private ModifiableAttribute _swerveAmplitude;
    public ModifiableAttribute SwerveAmplitude => _swerveAmplitude;
    
    [SerializeField] private ModifiableAttribute _swerveFrequency;
    public ModifiableAttribute SwerveFrequency => _swerveFrequency;

    [Header("HEALTH")]
    [SerializeField] private ModifiableAttribute _xHealth;
    public ModifiableAttribute XHealth => _xHealth;

    [SerializeField] private ModifiableAttribute _yHealth;
    public ModifiableAttribute YHealth => _yHealth;

    [Header("STAT TUNING")]
    [Tooltip("How quickly the enemy can move")]
    [Range(0f, 10f)]
    [SerializeField] private float _baseMoveSpeed;

    [Tooltip("How often the enemy will rotate (rotations per second)")]
    [Range(0f, 10f)]
    [SerializeField] private float _baseRotationFrequency;

    [Tooltip("How far the enemy will swerve")]
    [Range(0f, 10f)]
    [SerializeField] private float _baseSwerveAmplitude;

    [Tooltip("How often the enemy will swerve")]
    [Range(0f, 10f)]
    [SerializeField] private float _baseSwerveFrequency;
    
    [Tooltip("How many hits the enemy can take on the x-axis")]
    [Range(1, 3)]
    [SerializeField] private int _baseXHealth;
    
    [Range(1, 3)]
    [Tooltip("How many hits the enemy can take on the y-axis")]
    [SerializeField] private int _baseYHealth;
    #endregion

    #region Visuals and Sound
    [Header("GRAPHICS")]
    [SerializeField] private Sprite _attackAnimation;
    public Sprite AttackAnimation => _attackAnimation;

    [SerializeField] private Sprite _deathAnimation; 
    public Sprite DeathAnimation => _deathAnimation;
    
    
    [Header("SOUNDS")]
    [SerializeField] private AudioClip _spawnCry;
    public AudioClip SpawnCry => _spawnCry;

    [SerializeField] private AudioClip _deathCry;
    public AudioClip DeathCry => _deathCry;
    #endregion

    #region Methods
    void OnValidate()
    {
        SetBaseValuesOnValidate();
        
        foreach(ModifiableAttribute attribute in _attributes)
        {
            attribute.CalculateValue();
        }
    }

    public void ResetAttributeList() 
    {
        _attributes.Clear();

        _attributes.Add(_moveSpeed);
        _attributes.Add(_rotationFrequency);
        _attributes.Add(_xHealth);
        _attributes.Add(_yHealth);
        _attributes.Add(_swerveAmplitude);
        _attributes.Add(_swerveFrequency);

        InitAttributes();
    }

    public void ResetOnExitPlay()
    {
        ResetAttributeList();
    }

    public void OnEnable()
    {
        ResetAttributeList();
    }

    public void ApplyModifier(Modifier mod)
    {
        switch(mod.TargetAttribute)
        {
            case AttributeType.MOVEMENT_SPEED:
                _moveSpeed.AddModifier(mod);
                break;

            case AttributeType.ROTATION_FREQUENCY:
                _rotationFrequency.AddModifier(mod);
                break;

            case AttributeType.HEALTH:
                _xHealth.AddModifier(mod);
                _yHealth.AddModifier(mod);
                break;
        }
    }

    public void RemoveModifier(Modifier mod)
    {
        switch(mod.TargetAttribute)
        {
            case AttributeType.MOVEMENT_SPEED:
                _moveSpeed.RemoveModifier(mod);
                break;

            case AttributeType.ROTATION_FREQUENCY:
                _rotationFrequency.RemoveModifier(mod);
                break;

            case AttributeType.HEALTH:
                _xHealth.RemoveModifier(mod);
                _yHealth.RemoveModifier(mod);
                break;
        }
    }

    public void SetBaseValuesOnValidate() 
    {
        _moveSpeed.SetBaseValue(_baseMoveSpeed);
        _rotationFrequency.SetBaseValue(_baseRotationFrequency);
        _swerveAmplitude.SetBaseValue(_baseSwerveAmplitude);
        _swerveFrequency.SetBaseValue(_baseSwerveFrequency);
        _xHealth.SetBaseValue((float) _baseXHealth);
        _yHealth.SetBaseValue((float) _baseYHealth);
    }
    #endregion
}
