using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Xianxiao;
using static UnityEngine.UI.GridLayoutGroup;

namespace Xianxiao
{
    public abstract class AbstractCommandable : MonoBehaviour, ISelectable,IDamageable
    {
        [field: SerializeField] public int CurrentHealth { get; private set; }
        [field: SerializeField] public int MaxHealth { get; private set; }
        public Transform Transform => transform;
        [field: SerializeField] public Owner Owner { get; set; }
        [field: SerializeField] public bool IsVisible { get; private set; } = true;
        [field: SerializeField] public BaseCommand[] AvailableCommands { get; private set; }
        [SerializeField] private DecalProjector decalProjector;
        [SerializeField] protected Transform VisionTransform;
        [field: SerializeField] public AbstractUnitSO UnitSO { get; private set; }
        public delegate void HealthUpdatedEvent(AbstractCommandable commandable, int lastHealth, int newHealth);
        public event HealthUpdatedEvent OnHealthUpdated;
        private BaseCommand[] initialCommands;
        private Renderer[] renderers = Array.Empty<Renderer>();
        private ParticleSystem[] particleSystems = Array.Empty<ParticleSystem>();

        protected virtual void Awake()
        {
            renderers = GetComponentsInChildren<Renderer>();
            particleSystems = GetComponentsInChildren<ParticleSystem>();

        }

        protected virtual void Start()
        {
            if (UnitSO.SightConfig != null && VisionTransform != null)
            {
                float size = UnitSO.SightConfig.SightRadius * 2;
                VisionTransform.localScale = new Vector3(size, size, size);
                VisionTransform.gameObject.SetActive(Owner == Owner.Player1);
            }

            CurrentHealth = UnitSO.Health;
            MaxHealth = UnitSO.Health;
            initialCommands = AvailableCommands;
        }

        public void Select()
        {
            if (decalProjector != null)
            {
                decalProjector.gameObject.SetActive(true);
            }

            Bus<UnitSelectedEvent>.Raise(new UnitSelectedEvent(this));
        }

        public void Deselect()
        {
            if (decalProjector != null)
            {
                decalProjector.gameObject.SetActive(false);
            }
            SetCommandOverrides(null);
            Bus<UnitDeselectedEvent>.Raise(new UnitDeselectedEvent(this));
        }
        public void SetCommandOverrides(BaseCommand[] commands)
        {
          if(commands == null || commands.Length == 0)
          {
            AvailableCommands = initialCommands;
          }
          else
          {
            AvailableCommands = commands;
          }  
          Bus<UnitSelectedEvent>.Raise(new UnitSelectedEvent(this));
        }
        public void TakeDamage(int damage)
        {
            int lastHealth = CurrentHealth;
            CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0, CurrentHealth);

            OnHealthUpdated?.Invoke(this, lastHealth, CurrentHealth);
            if (CurrentHealth == 0)
            {
                Die();
            }
        }

        public void Die()
        {
            Destroy(gameObject);
        }

        public void Heal(int amount)
        {
            int lastHealth = CurrentHealth;
            CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, MaxHealth);
            OnHealthUpdated?.Invoke(this, lastHealth, CurrentHealth);
        }
        public void SetVisible(bool isVisible)
        {
            if (isVisible == IsVisible) return;

            IsVisible = isVisible;

            if (IsVisible)
            {
                OnGainVisibility();
            }
            else
            {
                OnLoseVisibility();
            }
        }

        private void OnGainVisibility()
        {
            foreach (Renderer renderer in renderers)
            {
                renderer.enabled = true;
            }

            foreach (ParticleSystem particleSystem in particleSystems)
            {
                particleSystem.gameObject.SetActive(true);
            }
        }

        private void OnLoseVisibility()
        {
            foreach (Renderer renderer in renderers)
            {
                renderer.enabled = false;
            }

            foreach (ParticleSystem particleSystem in particleSystems)
            {
                particleSystem.gameObject.SetActive(false);
            }
        }

    }
}

