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
    public TextMeshProUGUI buffText;
    [SerializeField] ParticleSystem sacrificeEffect = null;


    void Start()
    {
        mainCamera =  GameObject.FindWithTag("MainCamera");
        crossHair = GameObject.FindWithTag("CrossHair");

        closeUpCamera = transform.Find("CloseUpCamera").gameObject;
        closeUpCamera.SetActive(false);

        sacrificeEffect = transform.Find("CFX3_MagicAura_B_Runic").gameObject.GetComponent<ParticleSystem>();
        
        menuCanvas = GameObject.FindWithTag("MenuParent").transform.Find("SacrificeMenu").gameObject;
        buffText = GameObject.FindWithTag("MenuParent").transform.Find("RandomBuff").gameObject.GetComponent<TextMeshProUGUI>();


        // Check if the TextMeshPro component is found
        if (buffText != null)
        {
            print("Gotem");
        }
        else
        {
            // Handle the case where the component is not found
            Debug.LogError("TextMeshPro component not found on other parent GameObject.");
        }
        
        if (menuCanvas != null)
            menuCanvas.SetActive(false);
    }

    // private void GameObject FindChildWithTag(GameObject parent, string tag) 
    // {
    //     GameObject child = null;
 
    //     foreach(Transform transform in parent.transform) 
    //     {
    //         if(transform.CompareTag(tag)) 
    //         {
    //             child = transform.gameObject;
    //             break;
    //         }
    //     }

    //     return child;
    // }    
 
    public void increaseRandomStat()
    {
        int result = PlayerStats.Instance.increaseRandomStat();

        //!menuCanvas.transform.Find("BuffText").TryGetComponent<TextMeshPro>(out buffText)
        //if (menuCanvas != null)
            //return;

        mainCamera =  GameObject.FindWithTag("MainCamera");
        crossHair = GameObject.FindWithTag("CrossHair");

        closeUpCamera = transform.Find("CloseUpCamera").gameObject;
        closeUpCamera.SetActive(false);

        sacrificeEffect = transform.Find("CFX3_MagicAura_B_Runic").gameObject.GetComponent<ParticleSystem>();
        
        menuCanvas = GameObject.FindWithTag("MenuParent").transform.Find("SacrificeMenu").gameObject;
        buffText = GameObject.FindWithTag("MenuParent").transform.Find("RandomBuff").gameObject.GetComponent<TextMeshProUGUI>();
            
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
