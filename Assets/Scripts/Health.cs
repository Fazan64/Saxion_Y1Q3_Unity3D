using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    public delegate void OnHealthChangedHandler(Health sender, int oldValue, int newValue);
    public event OnHealthChangedHandler OnHealthChanged;

    public delegate void OnDeathHandler(Health sender);
    public event OnDeathHandler OnDeath;

    [SerializeField] [Range(0, 100)] int _health = 1;
    [SerializeField] bool destroyOnDeath = true;
    [SerializeField] bool canBeReduced   = true;

    public int health {
        get {
            return _health;
        }
        set {
            SetHealth(value);
        }
    }

    public Health SetHealth(int newHealth) {

        if (!canBeReduced && newHealth < _health) return this;
        if (newHealth < 0) newHealth = 0;
        if (newHealth == _health) return this;

        int oldValue = _health;
        _health = newHealth;
        if (OnHealthChanged != null) {
            OnHealthChanged.Invoke(this, oldValue, newHealth);
        }

        if (_health <= 0) {

            if (OnDeath != null) {
                OnDeath.Invoke(this);
            }
            if (destroyOnDeath) Destroy(gameObject);
        }

        return this;
    }

    public Health SetDestroyOnDeath(bool newDestroyOnDeath) {

        destroyOnDeath = newDestroyOnDeath;
        return this;
    }

    public Health SetCanBeReduced(bool newCanBeReduced) {

        canBeReduced = newCanBeReduced;
        return this;
    }
}
