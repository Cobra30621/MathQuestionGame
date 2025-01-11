using Feedback;
using Managers;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Characters.Ally
{
    public class AllyHealthHandler : MonoBehaviour
    {
        [SerializeField]
        private int _maxHealth;
        [SerializeField]
        private int _currentHealth;

        public UnityEvent<int> OnHealthChange = new UnityEvent<int>();
        public UnityEvent<int> OnDeath = new UnityEvent<int>();

        [Required]
        public IFeedback AddHealthFeedback;
        [Required]
        public IFeedback SubHealthFeedback;


        private void Awake()
        {
            OnHealthChange.AddListener((value) =>
            {
                UIManager.Instance.InformationCanvas.SetHealthText(value, _maxHealth);
            });
        }

        public void Init(int maxHealth)
        {
            _maxHealth = maxHealth;
            _currentHealth = maxHealth;
            UpdateHealth();
        }

        public void SetHealth(int health)
        {
            _currentHealth = Mathf.Clamp(health, 0, _maxHealth);
            UpdateHealth();
            CheckDeath();
        }

        public void AddHealth(int add)
        {
            if (add >= 0)
            {
                AddHealthFeedback.Play();
            }
            else
            {
                SubHealthFeedback.Play();
            }
            
            SetHealth(_currentHealth + add);
        }

        
        private void UpdateHealth()
        {
            OnHealthChange.Invoke(_currentHealth);
        }

        private void CheckDeath()
        {
            if (_currentHealth <= 0)
            {
                OnDeath.Invoke(_currentHealth);
            }
        }

        public AllyHealthData GetAllyHealthData()
        {
            var allyData = new AllyHealthData(_maxHealth);
            allyData.SetHealth(_currentHealth, _maxHealth);

            return allyData;
        }

        public void SetAllyHealthData(AllyHealthData data)
        {
            _maxHealth = data.MaxHealth;
            SetHealth(data.CurrentHealth);
        }

        public void HealByPercent(float percent)
        {
            int healAmount = Mathf.CeilToInt(_maxHealth * percent);
            AddHealth(healAmount);
        }
    }
}