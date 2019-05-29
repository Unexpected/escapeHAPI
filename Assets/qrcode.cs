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

public class qrcode : MonoBehaviour
{

	private WebCamTexture camTexture;

	public Text debugText;

	public GameObject plane;

	// Start is called before the first frame update
	void Start() {
		#if PLATFORM_ANDROID
		Application.RequestUserAuthorization(UserAuthorization.WebCam);
		#endif
		debugText.text = "Started";
		Debug.Log ("Script has been started");
		WebCamDevice[] devices = WebCamTexture.devices;
		camTexture = new WebCamTexture(WebCamTexture.devices[0].name);
		if (camTexture != null) {
			camTexture.Play();
		} else {
			Debug.Log ("camTexture is null");
		}

		plane.GetComponent<Renderer>().material.mainTexture = camTexture;
	}

    // Update is called once per frame
    void Update()
    {
		try {
			IBarcodeReader barcodeReader = new BarcodeReader ();
			// decode the current frame
			if (camTexture != null && camTexture.didUpdateThisFrame) {
				plane.GetComponent<Renderer>().material.mainTexture = camTexture;
			}

			var result = barcodeReader.Decode(camTexture.GetPixels32(), camTexture.width, camTexture.height);
			if (result != null) {
				Debug.Log("DECODED TEXT FROM QR: " + result.Text);
				debugText.text = result.Text;
			}
		} catch(Exception ex) { Debug.LogWarning (ex.Message); }
	}

	void OnGui() {
		// drawing the camera on screen
		// GUI.DrawTexture (screenRect, camTexture, ScaleMode.ScaleToFit);
		// do the reading — you might want to attempt to read less often than you draw on the screen for performance sake
    }
}
	