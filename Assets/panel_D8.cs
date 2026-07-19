using UnityEngine;
using UnityEngine.UI;

public class HistoricalDataPanel4 : MonoBehaviour
{
    private Canvas historyCanvas;
    private Text titleText;
    private Text descriptionText;
    private Image panelBackground;
    private RectTransform panelRect;
    private Button panelButton;

    private bool isMinimized = false;
    private Vector2 normalSize = new Vector2(700, 500);
    private Vector2 minimizedSize = new Vector2(700, 90);
    private float lastClickTime = 0f;
    private float doubleClickThreshold = 0.3f;
    private const string MINIMIZED_KEY = "HistoricalPanelMinimized";

    private void Start()
    {
        CreateHistoryPanel();
        LoadPanelState();
    }

    private void CreateHistoryPanel()
    {
        // Crear Canvas con mayor sortingOrder
        GameObject canvasObj = new GameObject("HistoryCanvas");
        canvasObj.transform.SetParent(transform);
        historyCanvas = canvasObj.AddComponent<Canvas>();
        historyCanvas.renderMode = RenderMode.ScreenSpaceOverlay;

        // Asegurar que el Canvas esté al frente
        GraphicRaycaster raycaster = canvasObj.AddComponent<GraphicRaycaster>();
        Canvas canvasComponent = canvasObj.GetComponent<Canvas>();
        canvasComponent.sortingOrder = 100;

        // Crear Panel con tamaño fijo aumentado
        GameObject panelObj = new GameObject("HistoryPanel");
        panelObj.transform.SetParent(canvasObj.transform, false);
        panelRect = panelObj.AddComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0, 1);
        panelRect.anchorMax = new Vector2(0, 1);
        panelRect.pivot = new Vector2(0, 1);
        panelRect.anchoredPosition = new Vector2(20, -20);
        panelRect.sizeDelta = normalSize;

        panelBackground = panelObj.AddComponent<Image>();
        panelBackground.color = new Color(0, 0, 0, 0.8f);

        // Agregar botón para detectar clics
        panelButton = panelObj.AddComponent<Button>();
        panelButton.targetGraphic = panelBackground;

        // Crear Título centrado
        GameObject titleObj = new GameObject("TitleText");
        titleObj.transform.SetParent(panelObj.transform, false);
        RectTransform titleRect = titleObj.AddComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0.5f, 1);
        titleRect.anchorMax = new Vector2(0.5f, 1);
        titleRect.pivot = new Vector2(0.5f, 1);
        titleRect.anchoredPosition = new Vector2(0, -20);
        titleRect.sizeDelta = new Vector2(650, 70);

        titleText = titleObj.AddComponent<Text>();
        titleText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        titleText.fontSize = 40;
        titleText.fontStyle = FontStyle.Bold;
        titleText.alignment = TextAnchor.MiddleCenter;
        titleText.color = Color.white;
        titleText.text = "Datos Históricos";
        titleText.verticalOverflow = VerticalWrapMode.Overflow;
        titleText.horizontalOverflow = HorizontalWrapMode.Wrap;

        // Crear Descripción justificada
        GameObject descriptionObj = new GameObject("DescriptionText");
        descriptionObj.transform.SetParent(panelObj.transform, false);
        RectTransform descriptionRect = descriptionObj.AddComponent<RectTransform>();
        descriptionRect.anchorMin = new Vector2(0, 0);
        descriptionRect.anchorMax = new Vector2(1, 1);
        descriptionRect.offsetMin = new Vector2(30, 30);
        descriptionRect.offsetMax = new Vector2(-30, -120);

        descriptionText = descriptionObj.AddComponent<Text>();
        descriptionText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        descriptionText.fontSize = 28;
        descriptionText.alignment = TextAnchor.UpperLeft;
        descriptionText.color = Color.white;
        descriptionText.text = "<b>Fundación:</b> Construida en el siglo XV por orden del inca Pachacútec como residencia real y centro religioso\n\n" +
            "\n\n" +
            "<b>Abandono:</b> Deshabitada drásticamente antes de 1572 debido a la caída del imperio, evitando que los españoles supieran de ella\n\n";
        descriptionText.verticalOverflow = VerticalWrapMode.Overflow;
        descriptionText.horizontalOverflow = HorizontalWrapMode.Wrap;

        // Guardar referencia a descriptionObj para minimizar/maximizar
        descriptionObj.name = "DescriptionText";


    }

    private void LoadPanelState()
    {
        // Cargar estado guardado
        isMinimized = PlayerPrefs.GetInt(MINIMIZED_KEY, 0) == 1;

        if (isMinimized)
        {
            panelRect.sizeDelta = minimizedSize;
            descriptionText.gameObject.SetActive(false);
        }
        else
        {
            panelRect.sizeDelta = normalSize;
            descriptionText.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        // Detectar doble clic en el panel
        if (Input.GetMouseButtonDown(0) && RectTransformUtility.RectangleContainsScreenPoint(panelRect, Input.mousePosition))
        {
            float timeSinceLastClick = Time.time - lastClickTime;

            if (timeSinceLastClick < doubleClickThreshold)
            {
                ToggleMinimize();
            }

            lastClickTime = Time.time;
        }
    }

    private void ToggleMinimize()
    {
        isMinimized = !isMinimized;

        if (isMinimized)
        {
            // Minimizar
            panelRect.sizeDelta = minimizedSize;
            descriptionText.gameObject.SetActive(false);
        }
        else
        {
            // Maximizar
            panelRect.sizeDelta = normalSize;
            descriptionText.gameObject.SetActive(true);
        }

        // Guardar estado
        SavePanelState();


    }

    private void SavePanelState()
    {
        // Guardar estado en PlayerPrefs
        PlayerPrefs.SetInt(MINIMIZED_KEY, isMinimized ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void DisplayHistoricalData(string locationName, string description)
    {
        if (titleText != null && descriptionText != null)
        {
            titleText.text = locationName;
            descriptionText.text = description;
            ShowPanel();
        }
    }

    public void HidePanel()
    {
        if (historyCanvas != null)
        {
            historyCanvas.gameObject.SetActive(false);
        }
    }

    public void ShowPanel()
    {
        if (historyCanvas != null)
        {
            historyCanvas.gameObject.SetActive(true);
        }
    }
}

