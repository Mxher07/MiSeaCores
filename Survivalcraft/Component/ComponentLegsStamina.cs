using Engine;
using GameEntitySystem;
using TemplatesDatabase;

namespace Game
{
    public class ComponentLegsStamina : Component
    {
        public SubsystemTime m_subsystemTime;
        public ComponentCreature m_componentCreature;
        public ComponentPlayer m_componentPlayer;
        
        public float m_stamina = 10f;
        public float m_maxStamina = 10f;
        public float m_staminaRegenRate = 0.5f;
        public float m_staminaDepletionRate = 20f;
        public float m_lastStaminaUpdateTime;

        public override void Load(ValuesDictionary valuesDictionary, IdToEntityMap idToEntityMap)
        {
            m_subsystemTime = Project.FindSubsystem<SubsystemTime>(true);
            m_componentCreature = Entity.FindComponent<ComponentCreature>(true);

            m_stamina = valuesDictionary.GetValue<float>("Stamina", 10f);
            m_maxStamina = valuesDictionary.GetValue<float>("MaxStamina", 10f);
            m_staminaRegenRate = valuesDictionary.GetValue<float>("StaminaRegenRate", 0.5f);
            m_staminaDepletionRate = valuesDictionary.GetValue<float>("StaminaDepletionRate", 0.15f);
            m_componentPlayer = Entity.FindComponent<ComponentPlayer>(throwOnError: true);
        }

        public override void Save(ValuesDictionary valuesDictionary, EntityToIdMap entityToIdMap)
        {
            valuesDictionary.SetValue("Stamina", m_stamina);
            valuesDictionary.SetValue("MaxStamina", m_maxStamina);
            valuesDictionary.SetValue("StaminaRegenRate", m_staminaRegenRate);
        }

        public void Update(float dt)
        {
            float gameTime = (float)m_subsystemTime.GameTime;
            
            if (gameTime != m_lastStaminaUpdateTime)
            {
                if (m_componentPlayer.ComponentBody.IsSneaking)
                {
                    m_stamina = MathUtils.Max(m_stamina - m_staminaDepletionRate * dt, 0f);
                }
                else
                {
                    m_stamina = MathUtils.Min(m_stamina + m_staminaRegenRate * dt, m_maxStamina);
                }
                if (m_componentPlayer.ComponentVitalStats.Food <= 0f)
                {

                    m_stamina = MathUtils.Max(m_stamina - m_staminaDepletionRate * 2f * dt, 0f);
                }
                else

                m_lastStaminaUpdateTime = gameTime;
            }
        }

//不会检测移动，正在等待大佬帮忙，休息中...

        public float GetStaminaPercentage()
        {
            return m_stamina / m_maxStamina;
        }

        public bool HasStamina()
        {
            return m_stamina > 0f;
        }

        public void ConsumeStamina(float amount)
        {
            m_stamina = MathUtils.Max(m_stamina - amount, 0f);
        }
    }
}