using UnityEngine;
using UnityEngine.UI;

public class ButtonStarScript : MonoBehaviour
{
    public bool isSelected = false;
    public Color normalColor;
    public Color SelectedColor;
    public int starIndex;

    private ManagerScript managerScript;

    private void Start()
    {
        managerScript = ManagerScript.instance;
        GetComponent<Image>().color = normalColor;
    }

    public void SelectStar()
    {
        isSelected = !isSelected;

        if(isSelected)
        {
            SwapColor(SelectedColor);
        }

        else
        {
            SwapColor(normalColor);
        }

        if (managerScript.firstStarSelected == null)
        {
            managerScript.SelectFirstStar(gameObject);            
        }

        else
        {
            if (managerScript.firstStarSelected != gameObject)
            {
                managerScript.SelectSecondStar(gameObject);
                managerScript.MatchStars();
            }
            else
            {
                managerScript.ResetFirstStar();
            }
        }
    }

    public void ResetStar()
    {
        isSelected = false;
        SwapColor(normalColor);
    }

    public void SwapColor(Color _color)
    {
        GetComponent<Image>().color = _color;
    }
}
