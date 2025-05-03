
using UnityEngine;
using UnityEngine.UIElements;

public class TowerTemplate : ScriptableObject
{
    [Header("Tower Descriptors")]
    public TowerType placementType;
    public string objectName;
    public string description;
    public int cost;
    public int towerPoint;

    [Header("Model")]
    public GameObject objectPrefab;

    [HideInInspector] public VisualElement container;
    [HideInInspector] public bool isSelected;
    [HideInInspector] public bool isPurchasable;

    public void SetSelected(bool _isSelected)
    {
        isSelected = _isSelected;
        if (container != null)
        {
            container.EnableInClassList("selected", isSelected);
        }
    }

    public void SetContainer(VisualElement _container)
    {
        container = _container;
        isSelected = false;
    }

    public void SetPurchasable()
    {
        isPurchasable = GameManager.Instance.CanPurchase(cost);

        container.EnableInClassList("purchasable", isPurchasable);
    }
}
