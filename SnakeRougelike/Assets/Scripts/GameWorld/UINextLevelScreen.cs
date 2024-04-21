using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UINextLevelScreen : MonoBehaviour
{
    private static UINextLevelScreen instance;

    public static UINextLevelScreen Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        // Ensure only one instance of the WaveController exists
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [SerializeField] private GameObject m_Background;
    
    [SerializeField] private UISnakeVisualiserController m_SnakeVisualiser;
    [SerializeField] private UIPowerupVisualierController m_PowerupVisualiser;

    [SerializeField] List<UIPurchasePart> m_PurchasePartButtons;
    [SerializeField] private UIPartInfo m_ToolTip;

    private bool m_UpgradeScreenOpen = false;
    private bool m_PuchasePanelOptionsLocked = false;
    public bool IsUpgradeScreenOpen()
    {
        return m_UpgradeScreenOpen;
    }

    public void UpdateSnakeVisualiser()
    {
        m_SnakeVisualiser.UpdateSnakeVisualiser();
    }

    public void OpenNextLevelScreen()
    {
        m_Background.SetActive(true);
        gameObject.transform.position = new Vector3(0, 0, gameObject.transform.position.z);
        m_UpgradeScreenOpen = true;

        if (!m_PuchasePanelOptionsLocked)
        {
            foreach (UIPurchasePart puchaseButton in m_PurchasePartButtons)
            {
                puchaseButton.SetNewPurchaseOption();
            }
        }

        UpdateSnakeVisualiser();
    }

    public void CloseNextLevelScreen()
    {
        m_Background.SetActive(false);
        gameObject.transform.position = new Vector3(10000, 10000, gameObject.transform.position.z);
        m_UpgradeScreenOpen = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        CloseNextLevelScreen();
    }

    public UIPartInfo GetToolTip()
    {
        return m_ToolTip;
    }

    public void SetPuchasePannelLock(bool InIsLocked)
    {
        m_PuchasePanelOptionsLocked = InIsLocked;
    }

    public void AddNewPowerupVisualiser(ModifierBase InModifier)
    {
        m_PowerupVisualiser.AddNewVisualiser(InModifier);
    }

    private void Update()
    {
        //DEBUG KEY
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            GameManager.Instance.AddMoney();
        }
    }
}
