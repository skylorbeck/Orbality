    using System;
    using System.Collections.Generic;
    using Unity.Mathematics;
    using UnityEngine;
    using UnityEngine.Rendering.Universal;
    using UnityEngine.VFX;

    public class BallAbstraction : MonoBehaviour
    {
        protected Camera _cam;

        [SerializeField] protected Vector3 mousePos;
        [SerializeField] protected Vector3 startingPos = Vector3.zero;
        [SerializeField] protected float speed = 100f;
        [SerializeField] protected float multiplier = 5f;
        [SerializeField] protected float maxLen = 1000f;
        [SerializeField] protected PhysicsSimulator physicsSimulator;
        protected Vector2 _force = Vector2.zero;
        protected float _torque = 0f;
        protected Material _material;
        protected VisualEffect _vfx;
        [SerializeField] protected Material[] _materials;
        [SerializeField] protected VisualEffectAsset[] _visualEffects;
        protected bool _collided;
        protected Vector2 _collisionPoint = Vector2.zero;
        protected Rigidbody2D _rb;
        protected bool _isSimulated = false;
        protected SpriteRenderer _renderer;
        protected int _textureIndex = 0;

        protected void Start()
        {
            _cam = Camera.main;
            _rb = GetComponent<Rigidbody2D>();
            startingPos = transform.position;
            _renderer = GetComponent<SpriteRenderer>();
            _material = _renderer.material;
            _vfx = GetComponent<VisualEffect>();
            _isSimulated = CompareTag("Simulated");
            _textureIndex = SaveManager.Instance.GetBallSkin();
            SetGlow(SaveManager.Instance.GetGlowSkin());
            SetVFX(SaveManager.Instance.GetParticleSkin());

        }

        protected void SetForce()
        {
            _force = Vector2.ClampMagnitude((mousePos - transform.position) * speed * multiplier, maxLen * multiplier);
            physicsSimulator.SetForce(_force, _torque);
        }

        public void SetBallSkin(int skin)
        {
            if (_material == null)
            {
                _renderer = GetComponent<SpriteRenderer>();
                _material = _renderer.material;
            }
            _textureIndex = skin;
            if (_isSimulated) return;

            _renderer.sprite =  Resources.Load<Sprite>("ball/" + _textureIndex);
            _material.SetTexture("_MainTex",_renderer.sprite.texture);
           
        }

        public void SetGlow(int glow)
        {
            if (_renderer == null)
            {
                _renderer = GetComponent<SpriteRenderer>();
            }
            if (_isSimulated) return;
                _renderer.material = _materials[glow];
                _material = _renderer.material;
                SetBallSkin(_textureIndex);
        }

        public void SetVFX(int vfx)
        {
            if (_vfx == null)
            {
                _vfx = GetComponent<VisualEffect>();
            }
            if (_isSimulated) return;
            _vfx.visualEffectAsset = _visualEffects[vfx];
        }

        public Vector2 GetCollisionPoint()
        {
            _collided = false;
            return _collisionPoint;
        }

        public bool Collided()
        {
            return _collided;
        }

        public void setCollided(bool collided)
        {
            _collided = collided;
        }

        public void SetSimulation()
        {
            _isSimulated = CompareTag("Simulated");
            _vfx = GetComponent<VisualEffect>();

            if (_isSimulated)
            {
                _vfx.enabled = false;
                return;
            }

            _material = GetComponent<SpriteRenderer>().material;
            _material.SetFloat("_Score", 10);
            _vfx.SetInt("Multiplier", 5);
            _vfx.SetFloat("Velocity", 5);
        }

        public int GetTextureIndex()
        {
            return _textureIndex;
        }
        
        
        public int GlowCount()
        {
            return _materials.Length;
        }        
        public int VisualEffectCount()
        {
            return _visualEffects.Length;
        }
        
        public enum HitBoxType
        {
            Circle,
            Capsule,
            Box
        }
    }