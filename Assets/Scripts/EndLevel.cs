using UnityEngine;
using DG.Tweening;

public class EndLevel : MonoBehaviour
{
    [SerializeField] private GameObject prizesSystem;
    [SerializeField] private RectTransform triangle;   // треугольник — UI элемент
    [SerializeField] private RectTransform xImage;     // контейнер, в пределах которого двигается треугольник
    [SerializeField] private int prizeCount = 5;       // количество призов/секторов
    [SerializeField] private GameSettings game;       // количество призов/секторов
    [SerializeField] private AudioSource audio;       // количество призов/секторов

    private Tween moveTween;
    public bool isMoving = false;

    private float leftX;
    private float rightX;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            if (isMoving)
                StopRoulette();
        }
    }

    public void StartRoulette()
    {
        float halfWidth = xImage.rect.width / 2;
        leftX = -halfWidth;
        rightX = halfWidth;

        prizesSystem.SetActive(true);

        isMoving = true;

        // Сброс позиции треугольника в начало
        triangle.anchoredPosition = new Vector2(leftX, triangle.anchoredPosition.y);

        // Двигаем треугольник туда-сюда бесконечно
        moveTween = triangle.DOAnchorPosX(rightX, 0.5f)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    public void StopRoulette()
    {
        isMoving = false;

        if (moveTween != null && moveTween.IsActive())
            moveTween.Kill();

        float currentX = triangle.anchoredPosition.x;

        // Преобразуем позицию X в индекс приза
        float totalWidth = rightX - leftX;
        float segmentWidth = totalWidth / prizeCount;
        int prizeIndex = Mathf.Clamp(Mathf.FloorToInt((currentX - leftX) / segmentWidth), 0, prizeCount - 1);

        Debug.Log($"Остановился на призе: {prizeIndex}");

        // Выравниваем по центру сектора
        float targetX = leftX + segmentWidth * prizeIndex + segmentWidth / 2f;
        triangle.DOAnchorPosX(targetX, 0.4f).SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                RewardPrize(prizeIndex);
                game.ResetGame();
                prizesSystem.SetActive(false);
                audio.Play();
            });
    }

    private void RewardPrize(int index)
    {
        index += 3;

        Debug.Log($"Выдан приз #{index}");

        int prizeMoney = game.currentLevelEarnings * index;

        Debug.Log(prizeMoney);
        Debug.Log(game.currentLevelEarnings);

        game.AddMoney(prizeMoney - game.currentLevelEarnings);

        // Тут можно показать UI, анимации, эффекты по индексу
        // Пример: если приз 2 — показать х-ик
        if (index == 2)
        {
            // xImage может содержать иконку приза
            Debug.Log("Показать особый приз");
        }
    }

    private void OnDrawGizmos()
    {
        if (xImage == null || prizeCount <= 0) return;

        RectTransform rect = xImage.GetComponent<RectTransform>();

        float width = rect.rect.width;
        float height = rect.rect.height;

        Vector3 bottomLeft = rect.TransformPoint(new Vector3(-width / 2f, -height / 2f, 0f));
        Vector3 topLeft = rect.TransformPoint(new Vector3(-width / 2f, height / 2f, 0f));
        Vector3 bottomRight = rect.TransformPoint(new Vector3(width / 2f, -height / 2f, 0f));
        Vector3 topRight = rect.TransformPoint(new Vector3(width / 2f, height / 2f, 0f));

        // 1. Рамка вокруг xImage
        Gizmos.color = new Color(0f, 1f, 0f, 0.5f);
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);

        // 2. Линии призовых секторов
        float segmentWidth = width / prizeCount;
        Gizmos.color = Color.cyan;

        for (int i = 1; i < prizeCount; i++)
        {
            float x = -width / 2f + i * segmentWidth;
            Vector3 from = rect.TransformPoint(new Vector3(x, -height / 2f, 0f));
            Vector3 to = rect.TransformPoint(new Vector3(x, height / 2f, 0f));
            Gizmos.DrawLine(from, to);
        }

        // 3. Жёлтые линии — границы движения треугольника
        Vector3 left = rect.TransformPoint(new Vector3(-width / 2f, 0f, 0f));
        Vector3 right = rect.TransformPoint(new Vector3(width / 2f, 0f, 0f));

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(left + Vector3.up * height / 2f, left + Vector3.down * height / 2f);
        Gizmos.DrawLine(right + Vector3.up * height / 2f, right + Vector3.down * height / 2f);
    }

}
