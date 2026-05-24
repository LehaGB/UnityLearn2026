using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Header("Настройки цикла")]
    [Tooltip("Длительность полного цикла дня и ночи в реальных секундах")]
    public float dayDurationInSeconds = 120f; // 2 минуты на полный цикл по умолчанию

    [Tooltip("Начальный угол поворота солнца (в градусах по оси X). 0 - полдень, 180 - полночь")]
    [Range(0, 360)]
    public float startAngleX = 0f;

    [Header("Отладка (необязательно)")]
    [Tooltip("Включите, чтобы видеть текущее время цикла в консоли")]
    public bool showDebugInfo = false;

    // Приватные переменные
    private Light directionalLight;
    private float currentTime = 0f;

    void Start()
    {
        // Получаем компонент света
        directionalLight = GetComponent<Light>();

        if (directionalLight == null)
        {
            Debug.LogError("DayNightCycle: На объекте нет компонента Light! Скрипт не будет работать.");
            enabled = false; // Отключаем скрипт
            return;
        }

        // Устанавливаем начальное вращение
        transform.rotation = Quaternion.Euler(startAngleX, 0f, 0f);
        currentTime = startAngleX / 360f; // Переводим градусы в прогресс (0-1)

        // Обновляем свет сразу при старте
        UpdateLight();
    }

    void Update()
    {
        if (dayDurationInSeconds <= 0f)
        {
            Debug.LogWarning("DayNightCycle: Длительность дня должна быть больше 0!");
            return;
        }

        // Прибавляем время (как часто сменяется день/ночь)
        // Делим Time.deltaTime на полную длительность, чтобы получить прогресс за кадр
        float timeProgress = Time.deltaTime / dayDurationInSeconds;
        currentTime += timeProgress;

        // Зацикливаем время (0 - начало, 1 - полный цикл)
        if (currentTime >= 1f)
        {
            currentTime -= 1f;
        }

        // Конвертируем прогресс (0-1) в угол поворота по оси X (0-360 градусов)
        float angleX = currentTime * 360f;
        transform.rotation = Quaternion.Euler(angleX, 0f, 0f);

        // (Опционально) Меняем цвет света в зависимости от времени суток
        UpdateLight();

        // Отладка
        if (showDebugInfo)
        {
            Debug.Log($"Время цикла: {currentTime * 100f:F1}% | Угол: {angleX:F1}°");
        }
    }

    // Дополнительный метод для визуального улучшения (меняет цвет света)
    void UpdateLight()
    {
        if (directionalLight == null) return;

        // Определяем время суток (0..1 от полуночи до полуночи)
        float timeOfDay = currentTime;

        // Немного меняем цвет в зависимости от времени:
        // Ночь - холодный синеватый, День - теплый желтый, Вечер/Утро - оранжевый
        if (timeOfDay < 0.25f) // От полуночи до утра (0° - 90°)
        {
            // Холодная ночь (синий)
            directionalLight.color = Color.Lerp(Color.blue, Color.white, timeOfDay * 4f);
            directionalLight.intensity = Mathf.Lerp(0.2f, 1f, timeOfDay * 4f);
        }
        else if (timeOfDay < 0.5f) // Утро - полдень (90° - 180°)
        {
            // Переход к теплому дню
            float t = (timeOfDay - 0.25f) * 4f;
            directionalLight.color = Color.Lerp(Color.white, Color.yellow, t);
            directionalLight.intensity = Mathf.Lerp(1f, 1.2f, t);
        }
        else if (timeOfDay < 0.75f) // Полдень - вечер (180° - 270°)
        {
            // Переход к закату
            float t = (timeOfDay - 0.5f) * 4f;
            directionalLight.color = Color.Lerp(Color.yellow, new Color(1f, 0.5f, 0f), t); // Оранжевый
            directionalLight.intensity = Mathf.Lerp(1.2f, 0.7f, t);
        }
        else // Вечер - полночь (270° - 360°)
        {
            // Переход к ночи
            float t = (timeOfDay - 0.75f) * 4f;
            directionalLight.color = Color.Lerp(new Color(1f, 0.5f, 0f), Color.blue, t);
            directionalLight.intensity = Mathf.Lerp(0.7f, 0.2f, t);
        }
    }

    // Публичный метод для внешнего управления (например, из UI)
    public void SetCycleTime(float normalizedTime)
    {
        currentTime = Mathf.Clamp01(normalizedTime);
        transform.rotation = Quaternion.Euler(currentTime * 360f, 0f, 0f);
        UpdateLight();
    }
}
