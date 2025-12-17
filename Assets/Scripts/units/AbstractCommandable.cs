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
        [field: SerializeField] public BaseCommand[] AvailableCommands { get; private set; }
        [SerializeField] private DecalProjector decalProjector;
        [field: SerializeField] public AbstractUnitSO UnitSO { get; private set; }
        public delegate void HealthUpdatedEvent(AbstractCommandable commandable, int lastHealth, int newHealth);
        public event HealthUpdatedEvent OnHealthUpdated;
        private BaseCommand[] initialCommands;

        protected virtual void Start()
        {
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
    }
}

