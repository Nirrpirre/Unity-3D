using UnityEngine;

public class WeaponSwitch : MonoBehaviour
{
    public GameObject[] weapons;
    private int selectedWeapon = 0;

    public GameObject rifleUIManager;  // Assign your rifle UI manager GameObject here
    public GameObject gunUIManager;    // Assign your gun UI manager GameObject here

    void Start()
    {
        SelectWeapon(selectedWeapon);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchToWeapon(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchToWeapon(1);

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f)
        {
            selectedWeapon = (selectedWeapon + 1) % weapons.Length;
            SelectWeapon(selectedWeapon);
        }
        else if (scroll < 0f)
        {
            selectedWeapon = (selectedWeapon - 1 + weapons.Length) % weapons.Length;
            SelectWeapon(selectedWeapon);
        }
    }

    void SwitchToWeapon(int index)
    {
        if (index >= 0 && index < weapons.Length)
        {
            selectedWeapon = index;
            SelectWeapon(selectedWeapon);
        }
    }

    void SelectWeapon(int index)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].SetActive(i == index);
        }

        // Show/hide the UI managers based on selected weapon
        rifleUIManager.SetActive(index == 0);
        gunUIManager.SetActive(index == 1);
    }

    // Optional: expose selected weapon index if needed elsewhere
    public int GetSelectedWeaponIndex()
    {
        return selectedWeapon;
    }
}
