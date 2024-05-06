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
    public GameObject player;
    public TextMeshProUGUI buffText;
    [SerializeField] ParticleSystem sacrificeEffect = null;


    void Start()
    {
        mainCamera = GameObject.FindWithTag("MainCamera") as GameObject;
        menuCanvas = FindObjectsOfType<Canvas>(true).FirstOrDefault(go => go.CompareTag("MenuCanvas"));
        crossHair = GameObject.FindWithTag("CrossHair") as GameObject;
        closeUpCamera.SetActive(false);
        if (menuCanvas != null) {
            buffText = FindObjectsOfType<TextMeshProUGUI>(true).FirstOrDefault(go => go.CompareTag("BuffText"));
            menuCanvas.gameObject.SetActive(false);
        }
        buffText.text = "";
        player = GameObject.FindGameObjectWithTag("Player");
    }
    public void increaseRandomStat()
    {
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

        }
        else
        {
            gun.SetActive(true);
            sword.SetActive(false);
            PlayerStats.Instance.CurrentWeaponType = Constants.WeaponType.Gun;
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
            player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
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
            player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
            Cursor.lockState = CursorLockMode.Confined;
        }
    }
}
