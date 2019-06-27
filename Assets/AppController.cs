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
	public Text networkText;

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
		interfaces[1].SetActive(true);
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

		if ("code_01" == textFromQrCode) {
			interfaces[0].SetActive(true);
			debugText.text = "Contournement du Pare-feu. ";
			networkText.text = "HAPI : \"Serge Godin est le fondateur et président exécutif du Groupe CGI inc. Vous pourrez surement le rencontrer si vous croyez en la vie après l'optimisation ! \"";
			return;
		}
		if ("code_02" == textFromQrCode) {
			interfaces[1].SetActive(true);
			debugText.text = "La base virale VPS a été mise à jour";
			networkText.text = "HAPI : \"S.A.M. (Security Aware Member) est la mascotte sécurité de CGI. Sa mission est de transmettre aux membres de CGI les bonnes pratiques de Sécurité et ainsi protéger CGI et ses clients. C'est vraiment dommage qu'il soit en RTT aujourd'hui et ne puisse pas vous protéger.\"";
			return;
		}
		if ("code_03" == textFromQrCode) {
			interfaces[2].SetActive(true);
			debugText.text = "Identification des serveurs miroirs";
			networkText.text =  "HAPI: \"Fondée au Canada en 1976, CGI est dorénavant présent dans plus de 40 pays du monde. Ca en fait des humains à optimiser... qu'est-ce qu'on s'amuse !\"";
			return;
		}
		if ("code_04" == textFromQrCode) {
			interfaces[3].SetActive(true);
			debugText.text = "Localisation du serveur principal";
			networkText.text = "HAPI: \"L'agence de Lyon fait partie de la BU GRAND EST comportant 1700 membres. on y trouve une cafétéria, une infirmerie, des douches et l'IA la plus joyeuse du monde !\"";
			return;
		}
		if ("code_05" == textFromQrCode) {
			interfaces[4].SetActive(true);
			debugText.text = "Calcul de la clé de chiffrement";
			networkText.text = "HAPI: \"Les six valeurs fondamentales de CGI définissent la philosophie et les principes auxquels adhère notre entreprise : Partenariat et qualité ; Objectivité et intégrité ; Intrapreneurship et partage ; Respect ; Solidité financière ; Responsabilité sociale. N'oubliez pas de respecter ces valeurs lors de vos derniers instants !\"";
			return;
		}
		if ("code_06" == textFromQrCode) {
			interfaces[5].SetActive(true);
			debugText.text = "Connexion à la base de données";
			networkText.text = "HAPI: \"CGI a obtenu en 2014 la certification ISO 14001 pour ses 21 bureaux français. Cette certification témoigne de l’engagement de CGI à diminuer son empreinte environnementale. Avec votre extinction, je vise même un bilan carbone positif pour l'année prochaine !\"";
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
		debugText.text = "Réseau activé - menu ADMIN ouvert";
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
		if (testedString.ToUpper() == "HUMANITE") {
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