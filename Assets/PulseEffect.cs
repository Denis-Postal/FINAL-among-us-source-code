using UnityEngine;

public class PulseEffect : MonoBehaviour
{
    [Header("Настройки пульсации")]
    [Tooltip("Скорость пульсации")]
    [SerializeField] private float speed = 2f;

    [Tooltip("Аплитуда (насколько сильно объект увеличивается/уменьшается)")]
    [SerializeField] private float scaleAmount = 0.2f;

    private Vector3 originalScale;

    void Start()
    {
        // Запоминаем исходный размер объекта
        originalScale = transform.localScale;
    }

    void Update()
    {
        // Вычисляем коэффициент масштаба с помощью функции синуса
        float scaleFactor = 1f + Mathf.Sin(Time.time * speed) * scaleAmount;
        
        // Применяем новый масштаб к объекту
        transform.localScale = originalScale * scaleFactor;
    }
}