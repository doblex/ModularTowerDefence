using UnityEngine;

public class Tower : MonoBehaviour
{
    [HideInInspector] public TowerTemplate template;


    public virtual void Initialize(TowerTemplate template)
    {
        this.template = template;
    }
}
