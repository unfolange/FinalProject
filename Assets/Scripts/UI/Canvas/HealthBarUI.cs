using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public enum TargetType { Player, Enemy }
    public TargetType targetType;

    [Header("Referencia del objeto con la vida")]
    public GameObject target;

    [Header("UI")]
    public Image healthBarImage;

    [Header("Flash Settings")]
    public float flashDuration = 0.5f;
    public float flashSpeed = 5f;
    public Color flashColor = Color.white;

    private float currentHealth;
    private float maxHealth;
    private float lastHealth = -1f;

    private bool isFlashing = false;
    private float flashTimer = 0f;
    private Color originalColor;

    void Start()
    {
        if (healthBarImage != null)
            originalColor = healthBarImage.color;
    }

    void Update()
    {
        if (target == null || healthBarImage == null) return;

        UpdateHealthValues();

        float healthPercent = Mathf.Clamp01(currentHealth / maxHealth);

        if (currentHealth != lastHealth)
        {
            healthBarImage.fillAmount = healthPercent;

            StartFlashEffect();
            lastHealth = currentHealth;
        }

        if (isFlashing)
        {
            flashTimer += Time.deltaTime;

            float t = Mathf.PingPong(Time.time * flashSpeed, 1f);
            healthBarImage.color = Color.Lerp(originalColor, flashColor, t);

            if (flashTimer >= flashDuration)
                StopFlashEffect();
        }
    }

    void UpdateHealthValues()
    {
        switch (targetType)
        {
            case TargetType.Player:
                PlayerController player = target.GetComponent<PlayerController>();
                if (player != null)
                {
                    currentHealth = player.health;
                    maxHealth = player.maxHealth;
                }
                break;

            case TargetType.Enemy:
                Enemy enemy = target.GetComponent<Enemy>();
                if (enemy != null)
                {
                    currentHealth = enemy.health;
                    maxHealth = enemy.maxHealth;
                }
                break;
        }
    }

    void StartFlashEffect()
    {
        isFlashing = true;
        flashTimer = 0f;
    }

    void StopFlashEffect()
    {
        isFlashing = false;
        healthBarImage.color = originalColor;
    }
}
