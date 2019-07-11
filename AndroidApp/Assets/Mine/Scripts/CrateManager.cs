using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateManager : MonoBehaviour
{
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TouchHandler();
    }

    private void TouchHandler()
    {
        if (Input.touchCount > 0)
        {
            Debug.Log("A tocar");
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Debug.Log("Touchphase began");
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.ScreenToWorldPoint(touch.position), Camera.main.transform.forward, out hit, 100))
                {
                    Debug.Log("fez ray");
                    CrateManager thisoneoverhere = hit.transform.GetComponent<CrateManager>();
                    if (thisoneoverhere)
                    {
                        Debug.Log("Acertou na cena");
                    }
                }
            }
        }
    }
}
