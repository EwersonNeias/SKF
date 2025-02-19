using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{


    void Update()
    {
        GetComponent<Image>().fillAmount -= Time.deltaTime * 0.1f;
    }

}
