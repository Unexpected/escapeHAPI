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
	public GameObject adminButton; 
	public List<GameObject> interfaces; 

	public Text debugText;

	// qr code scanner
	public GameObject qrCodeScanButton; 
	public GameObject cameraPlane; 
	private WebCamTexture camTexture;
	private bool cameraOn = false;


	public GameObject inputPassword; 
	public GameObject inputPasswordButton; 
	public GameObject winPanel; 

    // Start is called before the first frame update
    void Start()
    {
		#if PLATFORM_ANDROID
		Application.RequestUserAuthorization(UserAuthorization.WebCam);
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		#endif
		debugText.text = "App Started";
		Debug.Log ("Script has been started");

		adminButton.GetComponent<Button>().interactable = false; 
		adminButton.SetActive(false); 

		foreach (GameObject itf in interfaces) {
			itf.SetActive(false);
		}
		deactivateCamera();
		//HideCodeLogo();
		//HideAdminPanel();
		//HideWinPanel(); 
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
					CheckNetwork();
				}
			} catch(Exception ex) { Debug.LogWarning (ex.Message); }
		}
    }

	public void ReadSomething(string textFromQrCode) {
		Debug.Log("DECODED TEXT FROM QR: " + textFromQrCode);
		debugText.text = textFromQrCode;

		if ("code_01" == textFromQrCode) {
			interfaces[0].SetActive(true);
			debugText.text = "ITF-01 activated";
			return;
		}
		if ("code_02" == textFromQrCode) {
			interfaces[1].SetActive(true);
			debugText.text = "ITF-02 activated";
			return;
		}
		if ("code_03" == textFromQrCode) {
			interfaces[2].SetActive(true);
			debugText.text = "ITF-03 activated";
			return;
		}
		if ("code_04" == textFromQrCode) {
			interfaces[3].SetActive(true);
			debugText.text = "ITF-04 activated";
			return;
		}
		if ("code_05" == textFromQrCode) {
			interfaces[4].SetActive(true);
			debugText.text = "ITF-05 activated";
			return;
		}
		if ("code_06" == textFromQrCode) {
			interfaces[5].SetActive(true);
			debugText.text = "ITF-06 activated";
			return;
		}
		debugText.text = textFromQrCode + " is not a valid QR Code";
	}

	public void CheckNetwork() {
		foreach (GameObject itf in interfaces) {
			if (!itf.activeSelf)
				return;
		}
		adminButton.GetComponent<Button>().interactable = true; 
		adminButton.SetActive(true); 
		debugText.text = "Network activated - accessing admin panel";
	}

	public void toggleCamera() {
		if (cameraOn) {
			deactivateCamera();
		} else {
			activateCamera();
		}
	}

	public void deactivateCamera() {
		Debug.Log("deactivate camera");
		debugText.text = "";
		if (camTexture != null) {
			camTexture.Stop();
		}
		cameraPlane.GetComponent<Renderer>().enabled = false;
		cameraPlane.SetActive(false);

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

		cameraOn = true; 
	}

	public void TestAdminPassword() {
		string testedString = inputPassword.GetComponent<InputField>().text;
		if (testedString == "HUMANITE") {
			debugText.text = "";
			ShowWinPanel();
		} else {
			debugText.text = "Non, ce n'est pas " + testedString;
		}
	}

	public void ShowWinPanel() {
		winPanel.SetActive(true);
	}

}