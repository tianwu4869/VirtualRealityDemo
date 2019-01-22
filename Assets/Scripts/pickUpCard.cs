using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class pickUpCard : MonoBehaviour {

	SteamVR_TrackedObject trackedObj;
	SteamVR_Controller.Device device;
	Vector3 ori_pos;
	Vector3 ori_rot;
	bool check_pos;
	bool check_draw;
	GameObject newCard;
	GameObject randomCard;
	int randomNumber;
	public GameObject prefab0;
	public GameObject prefab1;
	int num_minions;
	public Vector3 deckTop;
	public bool[] slot = {false, false, false, false, false};

	// Use this for initialization
	void Awake () {
		trackedObj = GetComponent<SteamVR_TrackedObject>();
		check_pos = false;
		check_draw = false;
		ori_rot = new Vector3(90, 90, 0);
		ori_pos = new Vector3(-47.47f, 38.57f, 2.3f);
		randomCard = null;
		num_minions = 0;
	}

	// Update is called once per frame
	void FixedUpdate () {
		device = SteamVR_Controller.Input((int)trackedObj.index);
		if (device.GetTouchUp (SteamVR_Controller.ButtonMask.Trigger)) {
			GameObject canvas = GameObject.Find ("drawingCard_warning");
			canvas.transform.GetChild (0).gameObject.SetActive (false);
		}
		if(this.gameObject.transform.Find("drawnCard") && check_draw == true) //controller is holding a card drawn from deck
		{
			if (randomCard.tag == "spell") {
				randomCard.gameObject.GetComponent<fireOnCard> ().direction = this.gameObject.transform.eulerAngles;
			}else if(randomCard.tag == "minion"){
				randomCard.gameObject.GetComponent<cardLight> ().direction = this.gameObject.transform.eulerAngles;
			}
			if(device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
			{
				for(int i = 0; i < 5; i++)
				{
					if(slot[i] == true)
					{
						randomCard.transform.position = new Vector3 (-47.47f, 38.57f, 2.3f - i);
						randomCard.transform.parent = null;
						randomCard.transform.eulerAngles = ori_rot;
						if (randomCard.gameObject.tag == "spell") {
							randomCard.gameObject.transform.GetChild (0).gameObject.SetActive (false);
						}	
						check_draw = false; 

						check_pos = false;	
						slot [i] = false;
						break;	
					}
				}    
			}

			if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
			{
				
				if (randomCard.gameObject.tag == "minion" ) {
					
					if (num_minions == 3) {
						GameObject canvas = GameObject.Find ("Minions_num_warning");
						canvas.transform.GetChild (0).gameObject.SetActive (true);
					}else{
						randomCard.gameObject.GetComponent<cardLight>().getPressed = true;
						check_draw = false;
						check_pos = false;
						GameObject tempObject = GameObject.Find("cat_warrior1");
						for (int i = 1; tempObject != null; i++) {
							num_minions = i;
							tempObject = GameObject.Find("cat_warrior" + i);
						}	
						Debug.Log (num_minions);
					}
				}


				else if (randomCard.gameObject.tag == "spell" ) {
					randomCard.gameObject.GetComponent<fireOnCard> ().getPressed = true;
					randomCard.gameObject.GetComponent<fireOnCard> ().direction = this.gameObject.transform.eulerAngles;
					check_draw = false;
					check_pos = false;
				}
					
			}
			Debug.Log (string.Format("{0} {1} {2} {3} {4}", slot [0],slot [1],slot [2],slot [3],slot [4]));
		}
	}

	void OnTriggerStay(Collider col)
	{
		//Debug.Log ("Colliding");
		device = SteamVR_Controller.Input((int)trackedObj.index);

		if (col.tag == "spell") {
			col.gameObject.GetComponent<fireOnCard> ().direction = this.gameObject.transform.eulerAngles;
		}else if(col.tag == "minion"){
			col.gameObject.GetComponent<cardLight> ().direction = this.gameObject.transform.eulerAngles;
		}
		if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
		{
			
			if(col.tag == "deck")
			{
				if (check_draw == false) {
					if (slot [0] == true || slot [1] == true || slot [2] == true || slot [3] == true || slot [4] == true) {
						Debug.Log ("Drew a card.");
						GameObject tempPrefab = prefab0;
						randomNumber = (int)(Random.value * 10 / 5);
						switch (randomNumber) {
						case 0:
							tempPrefab = prefab0;
							break;
						case 1:
							tempPrefab = prefab1;
							break;
						default:
							break;
						}
						randomCard = GameObject.Instantiate (tempPrefab, deckTop, col.transform.rotation);
						randomCard.gameObject.name = "drawnCard";
						randomCard.gameObject.transform.SetParent (this.gameObject.transform);
						check_draw = true;
						check_pos = true;

					}
					else {
						GameObject canvas = GameObject.Find ("drawingCard_warning");
						canvas.transform.GetChild (0).gameObject.SetActive (true);
					}
				} 
			}
			else
			{
				if(check_pos == false){
					ori_pos = col.gameObject.transform.position;
					col.attachedRigidbody.isKinematic = true;
					col.gameObject.transform.SetParent(this.gameObject.transform);
					check_pos = true;
					slot[Mathf.RoundToInt(2.3f - col.gameObject.transform.position.z) ] = true;

				}
			}
		}

		if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
		{
			GameObject canvas = GameObject.Find ("Minions_num_warning");
			canvas.transform.GetChild (0).gameObject.SetActive (false);
			canvas = GameObject.Find ("drawingCard_warning");
			canvas.transform.GetChild (0).gameObject.SetActive (false);
			if (check_pos == true) {
				for(int i = 0; i < 5; i++)
				{
					if(slot[i] == true)
					{
						Debug.Log ("check_draw: " + check_draw);
						//					if (check_draw == true) {
						//							Debug.Log (randomCard.tag);
						//							randomCard.transform.position = new Vector3 (-47.47f, 38.57f, 2.3f - i);
						//							randomCard.transform.parent = null;
						//							randomCard.transform.eulerAngles = ori_rot;
						//							check_draw = false; 
						//					}
						if(col.tag == "spell" || col.tag == "minion") {
							col.gameObject.transform.position = new Vector3(-47.47f, 38.57f, 2.3f - i);
							col.gameObject.transform.parent = null;
							col.gameObject.transform.eulerAngles = ori_rot;
							if (col.gameObject.tag == "spell") {
								col.gameObject.transform.GetChild (0).gameObject.SetActive (false);
							}	
							check_pos = false;	
							slot [i] = false;
							break;	

						}

					}
				}
			}
			    
			Debug.Log (string.Format("{0} {1} {2} {3} {4}", slot [0],slot [1],slot [2],slot [3],slot [4]));
		}

		if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
		{
			
			col.attachedRigidbody.isKinematic = true;
			//col.gameObject.transform.SetParent(this.gameObject.transform);
			if (col.gameObject.tag == "minion") {
				if (num_minions == 3) {
					GameObject canvas = GameObject.Find ("Minions_num_warning");
					canvas.transform.GetChild (0).gameObject.SetActive (true);
				} else {
					col.gameObject.GetComponent<cardLight>().getPressed = true;
					check_pos = false;
					slot[Mathf.RoundToInt(2.3f - ori_pos.z)] = true;
					GameObject tempObject = GameObject.Find("cat_warrior1");
					for (int i = 1; tempObject != null; i++) {
						num_minions = i;
						tempObject = GameObject.Find("cat_warrior" + i);
					}	
					Debug.Log (num_minions);
				}

			}

			else if (col.gameObject.tag == "spell") {
				col.gameObject.GetComponent<fireOnCard> ().getPressed = true;
				col.gameObject.GetComponent<fireOnCard> ().direction = this.gameObject.transform.eulerAngles;
				check_pos = false;
				slot[Mathf.RoundToInt(2.3f - ori_pos.z)] = true;
			}
				
			Debug.Log (string.Format("{0} {1} {2} {3} {4}", slot [0],slot [1],slot [2],slot [3],slot [4]));
		}
	}
}