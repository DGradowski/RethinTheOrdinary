using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class StarsPanel : MonoBehaviour
{
    [Tooltip("Gwiazdki i progi punktowe")]
    public StarProgress[] stars;

    public void addSccoreToStar(int starIndex, int amount)
    {
        stars[starIndex].addPoints(amount);
    }

    public void resetStars()
    {
        foreach (var star in stars)
        {
            star.resetStars();
        }
    }

    [ContextMenu("Dodaj 20 pkt do kazdej gwiazdki")]
    public void debugAddPointsToAllStars()
    {
        foreach (var star in stars)
        {
            star.addPoints(20);
        }
    }

    [ContextMenu("Resetuj gwiazdki")]
    public void debugResetStars()
    {
        resetStars();
    }

    [ContextMenu("Aktywuj wszystkie gwiazdki")]
    public void debugActivateAllStars()
    {
        foreach (var star in stars)
        {
            star.addPoints(star.scoreThreshold);
        }
    }

    [ContextMenu("Dodaj 25 pkt do 2 gwiazdki")]
    public void debugAddPointsToSecondStar()
    {
        stars[1].addPoints(25);
    }
}

[System.Serializable]
public class StarProgress
{
    public SingularStar star;
    public int scoreThreshold;
    public int currentScore = 0;

    public void addPoints(int amount)
    {
        currentScore += amount;
        if (currentScore >= scoreThreshold)
        {
            star.activateStar();
        }
    }
    public void resetStars()
    {
        currentScore = 0;
        star.resetStar();
    }
}
