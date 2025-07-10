using UnityEngine;

public class SingularStar : MonoBehaviour
{
    public GameObject starFull;
    private Animator starAnimator;
    private bool isActive = false;

    private void Awake()
    {
        starAnimator = starFull.GetComponent<Animator>();
        starFull.SetActive(false);
    }

    public void activateStar()
    {
        if (isActive) return;

        starFull.SetActive(true);
        starAnimator.SetTrigger("StarActivation");
        isActive = true;
    }

    public void resetStar()
    {
        isActive = false;
        starFull.SetActive(false);
    }

    [ContextMenu("Testuj aktywacjê gwiazdki")]
    public void DebugActivateStar()
    {
        Debug.Log("Test: Aktywacja gwiazdki z menu kontekstowego");
        activateStar();
    }
}


