using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISnakeVisualiserController : MonoBehaviour
{
    [SerializeField] private GameObject m_Visualiser;

    private List<UISnakeVisualiser> m_VisualisersList = new List<UISnakeVisualiser>();

    public void UpdateSnakeVisualiser()
    {
        GameObject player = GameManager.Instance.GetPlayer();
        if (player == null)
            return;

        SnakeBodyController bodyController = player.GetComponent<SnakeBodyController>();
        if (bodyController == null)
            return;

        Dictionary<EBodyPart, int> collection = bodyController.GetPartCollection();

        foreach (KeyValuePair<EBodyPart, int> item in collection)
        {
            if (item.Value == 0)
                continue;

            bool alreadyExists = false;

            foreach (UISnakeVisualiser visualiser in m_VisualisersList)
            {
                if (visualiser.GetCurrentPart() == item.Key)
                {
                    visualiser.UpdateAmount(item.Value);
                    alreadyExists = true;
                    break;
                }
            }

            if (alreadyExists)
                continue;

            GameObject newVisualiserObj = Instantiate(m_Visualiser, this.transform);
            if(newVisualiserObj.TryGetComponent<UISnakeVisualiser>(out UISnakeVisualiser newVisualiser))
            {
                newVisualiser.SetVisualiser(item.Key, item.Value);
            }

            m_VisualisersList.Add(newVisualiser);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
