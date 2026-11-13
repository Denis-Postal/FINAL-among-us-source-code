using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class RGBSprite : MonoBehaviour
    {
    [Header("Настройки RGB")]
    [Tooltip("Скорость смены цветов")]
    public float speed = 1f;
    
    [Tooltip("Насыщенность цвета (от 0 до 1)")]
    [Range(0f, 1f)]
    public float saturation = 1f;
    
    [Tooltip("Яркость цвета (от 0 до 1)")]
    [Range(0f, 1f)]
    public float value = 1f;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Вычисляем оттенок (Hue) на основе времени
        float hue = (Time.time * speed) % 1f;
        
        // Преобразуем HSV в RGB и применяем к спрайту
        spriteRenderer.color = Color.HSVToRGB(hue, saturation, value);
    }
}