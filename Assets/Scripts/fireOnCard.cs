using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireOnCard : MonoBehaviour {


	public GameObject prefab;
	private GameObject prefabClone;

	public bool getPressed;
	public Vector3 direction;
	// Use this for initialization
	void Start () {
		getPressed = false;
	}

	// Update is called once per frame
	void Update () {
		if (this.transform.parent != null) {
			this.gameObject.transform.GetChild (0).gameObject.SetActive (true);
			this.gameObject.transform.GetChild (0).transform.eulerAngles = new Vector3 (0, 180, 0);
			this.transform.eulerAngles = direction;
		}
		if (getPressed == true && prefab != null) {
			prefab.transform.eulerAngles = direction;
			prefabClone = GameObject.Instantiate (prefab, this.transform.position - new Vector3(0, 1, 0), prefab.transform.rotation);

			prefab = null;
			this.gameObject.SetActive (false);
		}	
	}
}
