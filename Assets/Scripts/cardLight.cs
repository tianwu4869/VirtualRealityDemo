using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cardLight : MonoBehaviour {
	
	bool touched = false;
	public GameObject prefab;
	private GameObject prefabClone;
	public int num_minion;
    public bool getPressed;
	public Vector3 direction;
	// Use this for initialization
	void Start () {
		num_minion = 1;
        getPressed = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (this.transform.parent != null) {
			this.transform.eulerAngles = direction;
		}
		if (getPressed) {
			this.GetComponent<Light> ().enabled = true;
			touched = true;
		}
		if (touched == true && this.GetComponent<Light> ().range < 1.1) {
			this.GetComponent<Light> ().range = this.GetComponent<Light> ().range + 1.0f * Time.deltaTime;
		}
		if (this.GetComponent<Light> ().range > 1.1 && prefab != null) {
			GameObject tempObject = GameObject.Find("cat_warrior1");
			while (tempObject != null) {
				num_minion++;
				tempObject = GameObject.Find("cat_warrior" + num_minion);
			}	
			prefabClone = GameObject.Instantiate (prefab, new Vector3 (-27, 34, 10 * (num_minion / 2) * Mathf.Pow(-1, num_minion)), prefab.transform.rotation);
			prefabClone.name = "cat_warrior" + num_minion;
			prefab = null;
			this.gameObject.SetActive (false);
		}	
	}
		
}
