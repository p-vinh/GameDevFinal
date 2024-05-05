using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlankStudio.Constants;
using TMPro;
using System.Linq;

public class BloodSacrificeUI : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject closeUpCamera;
    public GameObject crossHair;
    public Canvas menuCanvas;
    private GameObject player;
    public TextMeshProUGUI buffText;
    [SerializeField] ParticleSystem sacrificeEffect = null;


    void Start()
    {
        mainCamera = GameObject.FindWithTag("MainCamera") as GameObject;
        menuCanvas = FindObjectsOfType<Canvas>(true).FirstOrDefault(go => go.CompareTag("MenuCanvas"));
        crossHair = GameObject.FindWithTag("CrossHair") as GameObject;
        closeUpCamera.SetActive(false);

        if (menuCanvas != null)
            menuCanvas.gameObject.SetActive(false);
    }
    public void increaseRandomStat()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        buffText = GameObject.Find("BuffText").GetComponent<TextMeshProUGUI>();
        int result = PlayerStats.Instance.increaseRandomStat();
        switch (result)
        {
            case 0:
                print("Increase max health!");
                buffText.text = "Max health++";
                break;
            case 1:
                print("Increase speed");
                buffText.text = "Speed++";
                break;
            case 2:
                print("Increase damage");
                buffText.text = "Damage++";
                break;
        }

        Invoke("resetBuffText", 2f);

        playEffect();
    }

    private void resetBuffText()
    {
        buffText.text = "";
    }

    private void playEffect()
    {
        sacrificeEffect.Play();
    }

    public void changeWeapons()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Animator anim = player.GetComponent<Animator>();
        bool carryGun = player.GetComponent<Movement>().carryGun;
        GameObject sword = player.GetComponent<Movement>().sword;
        GameObject gun = player.GetComponent<Movement>().gun;

        player.GetComponent<Movement>().carryGun = !carryGun; //update movement script
        carryGun = !carryGun; //update variable here

        if (!carryGun)
        {
            gun.SetActive(false);
            sword.SetActive(true);
            PlayerStats.Instance.CurrentWeaponType = Constants.WeaponType.Sword;
            PlayerStats.Instance.CurrentWeapon = new Weapon("Sword", PlayerStats.Instance.CurrentWeapon.Damage, PlayerStats.Instance.CurrentWeapon.Range, PlayerStats.Instance.CurrentWeapon.Speed);

        }
        else
        {
            gun.SetActive(true);
            sword.SetActive(false);
            PlayerStats.Instance.CurrentWeaponType = Constants.WeaponType.Gun;
            PlayerStats.Instance.CurrentWeapon = new Weapon("Gun", PlayerStats.Instance.CurrentWeapon.Damage, PlayerStats.Instance.CurrentWeapon.Range, PlayerStats.Instance.CurrentWeapon.Speed);
        }

        anim.SetBool("carryGun", carryGun);

        playEffect();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            mainCamera.SetActive(false);
            closeUpCamera.SetActive(true);
            crossHair.SetActive(false);
            menuCanvas.gameObject.SetActive(true);
            other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            mainCamera.SetActive(true);
            closeUpCamera.SetActive(false);
            crossHair.SetActive(true);
            menuCanvas.gameObject.SetActive(false);
            other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
            Cursor.lockState = CursorLockMode.Confined;
        }
    }
}
