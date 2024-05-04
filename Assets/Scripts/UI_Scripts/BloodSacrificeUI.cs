using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlankStudio.Constants;

public class BloodSacrificeUI : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject closeUpCamera;
    public GameObject crossHair;
    public GameObject menuCanvas;
    public GameObject player;
    [SerializeField] ParticleSystem sacrificeEffect = null;
    

    void Start()
    {
       mainCamera = GameObject.FindWithTag("MainCamera") as GameObject;
       closeUpCamera.SetActive(false);
       menuCanvas.SetActive(false);
       player = GameObject.FindGameObjectWithTag("Player");
    }
    public void increaseRandomStat()
    {

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

        if(!carryGun)
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

        anim.SetBool("carryGun",carryGun); 

        playEffect();
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            mainCamera.SetActive(false);
            closeUpCamera.SetActive(true);
            crossHair.SetActive(false);
            menuCanvas.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            mainCamera.SetActive(true);
            closeUpCamera.SetActive(false);
            crossHair.SetActive(true);
            menuCanvas.SetActive(false);
        }
    }
}
