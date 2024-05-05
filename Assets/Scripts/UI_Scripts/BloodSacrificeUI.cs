using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlankStudio.Constants;
using TMPro;
using System.Linq;
using UnityEngine.UI;

public class BloodSacrificeUI : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject closeUpCamera;
    public GameObject crossHair;
    public GameObject menuCanvas;
    private GameObject player;
    public TextMeshPro buffText;
    [SerializeField] ParticleSystem sacrificeEffect = null;


    void Start()
    {
        mainCamera = GameObject.FindWithTag("MainCamera") as GameObject;
        crossHair = GameObject.FindWithTag("CrossHair") as GameObject;
        closeUpCamera.SetActive(false);
        menuCanvas = FindObjectsOfType<GameObject>(true).FirstOrDefault(go => go.CompareTag("MenuCanvas"));

        if (menuCanvas != null)
            menuCanvas.SetActive(false);
    }

    public void increaseRandomStat()
    {
        int result = PlayerStats.Instance.increaseRandomStat();

        
        if (menuCanvas != null && !menuCanvas.transform.Find("RandomBuff").TryGetComponent<TextMeshPro>(out buffText))
            return;
            
        switch (result)
        {
            case 0:
                print("Increase max health!");
                buffText.text = "Health++";
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
            menuCanvas.SetActive(true);
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
            menuCanvas.SetActive(false);
            other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
            Cursor.lockState = CursorLockMode.Confined;
        }
    }
}
