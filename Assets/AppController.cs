using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif

using ZXing;
using ZXing.QrCode;

public class AppController : MonoBehaviour
{

	public Text debugText;

	public GameObject qr1; 
	public GameObject qr2; 
	public GameObject qr3; 

	// qr code scanner
	public GameObject qrCodeScanButton; 
	public GameObject cameraPlane; 
	public GameObject closeButton; 
	private WebCamTexture camTexture;
	private bool cameraOn = false;


	public GameObject adminButton; 
	public GameObject inputPassword; 
	public GameObject inputPasswordButton; 

	public GameObject codeLogoButton; 
	public GameObject codeLogoCloseButton; 
	public GameObject codeLogoImage; 

	public GameObject winPanel; 

    // Start is called before the first frame update
    void Start()
    {
		#if PLATFORM_ANDROID
		Application.RequestUserAuthorization(UserAuthorization.WebCam);
		#endif
		debugText.text = "App Started";
		Debug.Log ("Script has been started");

		deactivateCamera();
		HideCodeLogo();
		HideAdminPanel();
		HideWinPanel(); 
	}

    // Update is called once per frame
    void Update()
    {
		if (cameraOn) {
			try {
				IBarcodeReader barcodeReader = new BarcodeReader ();
				// decode the current frame
				if (camTexture != null && camTexture.didUpdateThisFrame) {
					cameraPlane.GetComponent<Renderer>().material.mainTexture = camTexture;
				}

				var result = barcodeReader.Decode(camTexture.GetPixels32(), camTexture.width, camTexture.height);
				if (result != null) {
					ReadSomething(result.Text);
				}
			} catch(Exception ex) { Debug.LogWarning (ex.Message); }
		}
    }

	public void ReadSomething(string textFromQrCode) {
		Debug.Log("DECODED TEXT FROM QR: " + textFromQrCode);
		debugText.text = textFromQrCode;

		if (!qr1.GetComponent<Button>().interactable) {
			qr1.GetComponent<Button>().interactable = true; 
			return;
		}

		if (!qr2.GetComponent<Button>().interactable) {
			qr2.GetComponent<Button>().interactable = true; 
			return;
		}

		if (!qr3.GetComponent<Button>().interactable) {
			qr3.GetComponent<Button>().interactable = true; 
			return;
		}

		debugText.text = "No more QR Codes needed !";

	}

	public void deactivateCamera() {
		Debug.Log("deactivate camera");
		debugText.text = "";
		if (camTexture != null) {
			camTexture.Stop();
		}
		cameraPlane.GetComponent<Renderer>().enabled = false;
		cameraPlane.SetActive(false);

		//closeButton.GetComponent<Renderer>().enabled = false;
		closeButton.SetActive(false);

		cameraOn = false;
	}

	public void activateCamera() {
		Debug.Log("activate camera");
		WebCamDevice[] devices = WebCamTexture.devices;
		camTexture = new WebCamTexture(WebCamTexture.devices[0].name);
		camTexture.Play();
		cameraPlane.SetActive(true);
		cameraPlane.GetComponent<Renderer>().enabled = true;
		cameraPlane.GetComponent<Renderer>().material.mainTexture = camTexture;

		closeButton.SetActive(true);
		cameraOn = true; 
	}

	public void ShowCodeLogo() {
		Debug.Log("show code logo");

		codeLogoButton.GetComponent<Button>().interactable = false; 

		codeLogoCloseButton.SetActive(true); 
		codeLogoImage.SetActive(true);
	}

	public void HideCodeLogo() {
		codeLogoButton.GetComponent<Button>().interactable = true; 
		codeLogoCloseButton.SetActive(false); 
		codeLogoImage.SetActive(false);
	}

	private void HideAdminPanel() {
		if (inputPassword.activeSelf) {
			ToggleAdminPanel();
		}
	}

	public void ToggleAdminPanel() {
		inputPassword.SetActive(!inputPassword.activeSelf);
		inputPasswordButton.SetActive(!inputPasswordButton.activeSelf);
	}

	public void HideWinPanel() {
		winPanel.SetActive(false);
	}

	public void TestAdminPassword() {
		string testedString = inputPassword.GetComponent<InputField>().text;
		if (testedString == "youpi") {
			ShowWinPanel();
		} else {
			debugText.text = "Non, ce n'est pas " + testedString;
		}
	}

	public void ShowWinPanel() {
		winPanel.SetActive(true);
	}

}