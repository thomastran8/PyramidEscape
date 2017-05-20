using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerUI : MonoBehaviour {
	private int hp;
    private Text potionText;
    public int numPotions;

	[HideInInspector]
	public int curPotions;

	private Image[] icons;
	private Sprite[] heartTypes;
    private GameObject quad;
    private Camera cam;
    // Use this for initialization
    private void Awake() {
        GameManager.UI = this;
    }
		
    void Start () {
		cam = GameObject.Find("Main Camera").GetComponent<Camera>();
		GameObject.FindGameObjectWithTag ("Canvas").GetComponent<Canvas> ().worldCamera = cam;
		curPotions = numPotions; //Set current number of potions to number of potions for that level
        quad = GameObject.FindGameObjectWithTag("Red Flash");
        hp = 3;
        icons = new Image[3];
        heartTypes = new Sprite[2];
      
        for (int i = 0; i < 3; i++) {
            icons[i] = GameObject.Find("Heart" + i.ToString()).GetComponent<Image>();
        }

        heartTypes[0] = icons[0].sprite; //First sprite is an empty heart
        heartTypes[1] = icons[1].sprite; //Second is a full heart

        GameObject potionCount = GameObject.FindGameObjectWithTag("Potion Count");

        potionText = potionCount.GetComponent<Text>();
       
		UpdateUI ();
    }
	
    public void getPotion() {
        curPotions++;
        UpdateUI();
    }

    public void usePotion() {
        curPotions--;
        UpdateUI();
    }
    /*
	 *  Update UI to reflect damage taken
	 */
    void UpdateUI () {
		int temphp = hp;
		for (int i = 0; i < icons.Length; i++) {
			if (temphp > 0) {
				icons [i].sprite = heartTypes [1];
				temphp -= 1;
			}//If health, add a full container
			else {
				icons [i].sprite = heartTypes [0];
			}// no health, empty container
		}
        potionText.text = curPotions.ToString() + "x";

    }

    void Death() {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.useGravity = (false);
        
        StartCoroutine("cameraPanDown");
        StartCoroutine(Fade());
    }

    IEnumerator cameraPanDown() {
		Transform orig = cam.gameObject.transform;
        Transform camera = cam.gameObject.transform;
		while (camera.rotation.eulerAngles.x <= 70) {
            Vector3 oldRot = camera.rotation.eulerAngles;
            camera.rotation = Quaternion.Euler(oldRot.x + 1, oldRot.y, oldRot.z + Random.Range(0f,2f)) ;
            yield return new WaitForSeconds(.03f);
        }
		camera = orig;
		GameObject.FindGameObjectWithTag ("Game Manager").GetComponent<GameManager> ().respawn ();
        yield break;
    }

    public void ShakeCamera(float intensity, float decreasefactor) {
        cam.GetComponent<CameraEffects>().StartShake(intensity, decreasefactor);
    }

	/*
	 * Applies damage to player and changes GUI to reflect damage taken
	 * Returns true if player is killed
	 */
	bool applyDamage(int damage) {
        if (hp <= 0) {
            return false;
        }
        StartCoroutine("Hit");
        hp -= damage;
        UpdateUI ();
		if (hp <= 0) {
            Death();
			this.gameObject.GetComponent<PlayerMovement> ().alive = false;
			return true;
		}

		return false;
	}

    IEnumerator Fade() {
        Renderer rend = quad.GetComponent<Renderer>();
		Renderer orig = rend;
		while (rend.material.color != new Color(0,0,0)) {
            rend.material.color = Color.Lerp(rend.material.color, Color.black, .12f);
            yield return new WaitForSeconds(.01f);
        }
		rend = orig;
        yield break;
    }

    IEnumerator Hit() {
        if (hp <= 0) {
            yield break;
        }//Dont flash if dead
     
        Renderer rend = quad.GetComponent<Renderer>();
        float curAlpha = .9f;
        rend.material.color = new Color (rend.material.color.r, rend.material.color.g, rend.material.color.b, curAlpha);
        while(rend.material.color.a >= 0) {
            curAlpha -= .01f;
            rend.material.color = new Color(rend.material.color.r, rend.material.color.g, rend.material.color.b, curAlpha);
            yield return new WaitForSeconds(.01f);
        }
	

        yield break;
    }
}
