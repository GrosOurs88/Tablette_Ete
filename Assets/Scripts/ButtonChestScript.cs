using UnityEngine;
using UnityEngine.UI;

public class ButtonChestScript : MonoBehaviour
{
    public void ChangeColor()
    {
        GetComponent<Image>().color = new Color(0f, 1f, 1f, 0.5f);
        ManagerScript.instance.ButtonSelected = gameObject;
    }
}
