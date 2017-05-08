using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerUI : MonoBehaviour {
	private int hp;
    private Text potionText;
    public int numPotions;
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
        quad = GameObject.FindGameObjectWithTag("Red Flash");
        if (quad == null) {
            Debug.Log("Could not find flash");
        }
        hp = 3;
        icons = new Image[3];
        heartTypes = new Sprite[2];
      
        for (int i = 0; i < 3; i++) {
            icons[i] = GameObject.Find("Heart" + i.ToString()).GetComponent<Image>();
        }

        heartTypes[0] = icons[0].sprite; //First sprite is an empty heart
        heartTypes[1] = icons[1].sprite; //Second is a full heart

        GameObject potionCount = GameObject.FindGameObjectWithTag("Potion Count");
        if (potionCount == null) {
            Debug.Log("Error: Could not find potion count text");
        }

        potionText = potionCount.GetComponent<Text>();
       
		UpdateUI ();
    }
	
    public void getPotion() {
        numPotions++;
        UpdateUI();
    }

    public void usePotion() {
        numPotions--;
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
        potionText.text = numPotions.ToString() + "x";

    }

    public void ShakeCamera(float intensity, float decreasefactor) {
        cam.GetComponent<CameraShake>().StartShake(intensity, decreasefactor);
    }

	/*
	 * Applies damage to player and changes GUI to reflect damage taken
	 * Returns true if player is killed
	 */
	bool applyDamage(int damage) {
        
		Debug.Log ("Player recieved " + damage.ToString () + " damage");
		hp -= damage;
        StartCoroutine("Hit");
		UpdateUI ();
		if (hp <= 0) {
			Debug.Log ("YOU ARE DEAD");
			this.gameObject.GetComponent<PlayerMovement> ().alive = false;
			return true;
		}
		return false;
	}

    IEnumerator Hit() {
        Debug.Log("HIT");
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
