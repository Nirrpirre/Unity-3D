using UnityEngine;
using TMPro;

public class AmmoUI : MonoBehaviour
{
    public TextMeshProUGUI ammoText;
    public GunController gunController;

    void Update()
    {
        if (gunController != null && ammoText != null)
        {
            ammoText.text = gunController.currentAmmo + " / " + gunController.maxAmmo;
        }
    }
}
