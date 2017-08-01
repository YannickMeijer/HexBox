using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour {

    public List<Card> cardsInHand;
    public int cameraDistance;
    private Camera mainCamera;

	// Use this for initialization
	void Start () {
        mainCamera = GetComponentInParent<Camera>();
        for (int x = 0; x < cardsInHand.Count; x++)
        {
            cardsInHand[x].transform.SetParent(this.transform);
            cardsInHand[x].transform.rotation = this.transform.rotation;
        }
    }
	
	// Update is called once per frame
	void Update () {
        var frustumHeigth = 2.0f * cameraDistance * Mathf.Tan(mainCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
        var frustumWidth = mainCamera.aspect * frustumHeigth;
        transform.localPosition = new Vector3(-frustumWidth/2, -frustumHeigth / 2, cameraDistance); //Origin of Hand is left and down from the camera,at the edge of what's visible
        float frustumWidthDivision = frustumWidth / (cardsInHand.Count + 1);
        for (int x = 0; x < cardsInHand.Count; x++)
        {
            cardsInHand[x].UpdatePosition(new Vector3(frustumWidthDivision * (x + 1), 0, 0), frustumHeigth, cameraDistance);
        }
    }

}
