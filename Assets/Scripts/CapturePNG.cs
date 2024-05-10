using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapturePNG : MonoBehaviour
{

   [SerializeField]
   private string filePath = "D:/";

   [SerializeField]
   private string fileName = "Capture.png";
   
   private IEnumerator Capture()
   {
        yield return new WaitForEndOfFrame();
        zzTransparencyCapture.captureScreenshot(filePath+fileName);
        Debug.Log("PNG Captured: "+fileName+" at Path:"+ filePath);
   }

   private void Update()
   {
      if(Input.GetKeyDown(KeyCode.C))
      {
         StartCoroutine(Capture());
      }
   }
}
