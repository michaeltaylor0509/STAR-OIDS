using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    private static bool milestoneTriggered = false;
    private const long SCORE_MILESTONE = 99999999999999;

    public static void AddScore(int amount)
    {
        // multiplicamos los puntos base (50/15/5) por el multiplicador actual.
        // amount es int, CurrentMultiplier es float -> el resultado es float (decimal)
        // (long) convierte ese decimal a entero grande, que es el tipo de Player.SCORE
        long finalAmount = (long)(amount * ScoreMultiplier.CurrentMultiplier);
        Player.SCORE += finalAmount;
        Debug.Log($"AddScore llamado con: {amount} | Total ahora: {Player.SCORE}");
        UpdateScoreText();

        if (Player.SCORE >= SCORE_MILESTONE && !milestoneTriggered)
        {
            milestoneTriggered = true;
            TriggerMilestoneEvent();
        }
    }

    private static void UpdateScoreText()
    {
        GameObject go = GameObject.FindGameObjectWithTag("UI");
        go.GetComponent<TMP_Text>().text = "Score: " + Player.SCORE;
    }

    private static void TriggerMilestoneEvent()
    {
        Debug.Log("¡Puntuación milestone alcanzada!");
    }
}