using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffects : MonoBehaviour {

    private Vector3 originalPosition;


    private void Start() {
        originalPosition = transform.localPosition;
    }
    public void StartShake(float intensity, float decreaseFactor) {
        StartCoroutine(Shake(intensity, decreaseFactor));
    }

    IEnumerator Shake(float intensity, float decreaseFactor) {
        float shake = 1;
        while (shake >= 0) {
            this.transform.localPosition = originalPosition + (Random.insideUnitSphere * intensity);
            shake -= Time.deltaTime * decreaseFactor;
            yield return new WaitForSeconds(.1f);
        }
        this.transform.localPosition = originalPosition;
       yield break;

    }
}
