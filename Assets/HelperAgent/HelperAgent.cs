using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HelperAgent : MonoBehaviour
{
	public IbmController _IbmController;

	void Start()
	{
		_IbmController = new IbmController();
		_IbmController.StartStt(handleSttResult);
	}

	void handleSttResult(string resultPhrase) {
		Debug.Log("Recognized phrase:" + resultPhrase);		
		_IbmController.MessageWa(resultPhrase);
	}
}
