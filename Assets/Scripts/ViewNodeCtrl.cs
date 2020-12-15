using UnityEngine;
using UnityEngine.UI;

public class ViewNodeCtrl : MonoBehaviour
{
    [SerializeField] private Image[] enemyImages;
    
    public void ActivateImage(int num)
    {
        num = Mathf.Clamp(num, 0, enemyImages.Length - 1);
        enemyImages[num].enabled = true;
    }
}
