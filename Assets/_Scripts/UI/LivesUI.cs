using UnityEngine;
using System.Collections;
using DG.Tweening;
using TMPro;
public class LivesUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentLives;
    [SerializeField] private TextMeshProUGUI updateLives;
    [SerializeField] private Transform livesTextContainer;
    [SerializeField] private float duration;
    [SerializeField] private Ease animationCurve;
    private float containerInitPosition;
    private float moveAmount;


    void Start()
    {
        Canvas.ForceUpdateCanvases();
        currentLives.SetText("3");
        updateLives.SetText("2");
        containerInitPosition = livesTextContainer.localPosition.y;
        moveAmount = currentLives.rectTransform.rect.height;
    }

    public void UpdateLives(int lives)
    {
        updateLives.SetText($"{lives}");
        livesTextContainer.DOLocalMoveY(containerInitPosition + moveAmount, duration).SetEase(animationCurve);
        StartCoroutine(ResetLivesContainer(lives));
    }

    private IEnumerator ResetLivesContainer(int lives)
    {
        yield return new WaitForSeconds(duration);
        currentLives.SetText($"{lives}"); // update the original score
        Vector3 localPosition = livesTextContainer.localPosition;
        livesTextContainer.localPosition = new Vector3(localPosition.x, containerInitPosition, localPosition.z);
    }

}
