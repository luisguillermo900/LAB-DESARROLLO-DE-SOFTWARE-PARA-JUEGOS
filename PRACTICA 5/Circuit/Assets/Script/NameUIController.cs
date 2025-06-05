using System;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
public class NameUIController : MonoBehaviour
{
    public TextMeshProUGUI playerName;
    public TextMeshProUGUI lapDisplay;
    public Transform target;
    CanvasGroup canvasGroup;
    public Renderer carRend;
    CheckpointManager cpManager;


    // Start is called before the first frame update
    void Start()
    {
        this.transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
         playerName = this.GetComponent<TextMeshProUGUI>();
      //  if (playerName == null)
         //   playerName = GetComponentInChildren<TextMeshProUGUI>();
        canvasGroup = this.GetComponent<CanvasGroup>();

    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (carRend == null) return;
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        bool carInView = GeometryUtility.TestPlanesAABB(planes, carRend.bounds);
        canvasGroup.alpha = carInView ? 1 : 0;
        this.transform.position = Camera.main.WorldToScreenPoint(target.position + Vector3.up * 1.5f);
        if (cpManager==null)
        {
            cpManager = target.GetComponent<CheckpointManager>();
        }
        lapDisplay.text = "Lap: " + cpManager.lap.ToString() + "CP: "+ cpManager.checkpoint;
    }
}
