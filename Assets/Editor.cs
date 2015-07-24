using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Editor : MonoBehaviour {

	public LevelManager levelManager;
	public Camera camera;
	public Transform mouseHover;

	public GameObject saveUI;
	public GameObject loadUI;

	public Text saveText;
	public Text loadText;

	public GameObject activePopup;

	public string textFieldVal = "";

	// Use this for initialization
	void Start () {
		saveUI.SetActive(false);
		loadUI.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 mPos = Input.mousePosition;
		
		if(mPos.x < 100 || activePopup != null)
			return;

		Vector3 world = camera.ScreenToWorldPoint(mPos);
		Vector2 tPos = new Vector2(Mathf.Round(world.x/levelManager.tileSize), Mathf.Round(world.y/levelManager.tileSize));

		mouseHover.position = new Vector3(tPos.x, tPos.y, 0);

		if (Input.GetMouseButton(0)){
			levelManager.placeTile(tPos, LevelManager.TileTypes.white, true);
		}
        
        if (Input.GetMouseButton(1)){
			levelManager.removeTile(tPos);
        }
	}

	// Button callbacks
	public void openSaveUI(){
		if(activePopup != null){
			return;
		}
		saveUI.SetActive(true);
		activePopup = saveUI;
	}

	public void openLoadUI(){
		if(activePopup != null){
			return;
		}
		loadUI.SetActive(true);
		activePopup = loadUI;
	}

	// LITERALLY THE APOALYPSE
	public void clear(){
		if(activePopup != null){
			return;
		}
		levelManager.clear();
	}

	public void doSave(){
		levelManager.save(saveText.text);
	}

	public void doLoad(){
		levelManager.load(loadText.text);
	}

	public void textFieldChange(string val){
		textFieldVal = val;
		Debug.Log("Changed field: ");
		Debug.Log(val);
	}

	public void closePopup(){
		if(activePopup == null){
			return;
		}
		activePopup.SetActive(false);
		activePopup = null;
	}
}
