using System.Collections.Generic;
using UnityEngine;

public class Pot : MonoBehaviour
{
    List<Turret> turretsPlaced = new List<Turret>();
    List<PowerUp> powerUpPlaced = new List<PowerUp>();
    public bool atLeastOneTurret = false;

    int maxTurretPoint = 3;
    int currentTurretPoint = 0;

    public bool CanBuild(TowerTemplate template)
    {
        return currentTurretPoint + template.towerPoint <= maxTurretPoint;
    }

    public void AddTower(Tower tower)
    {
        currentTurretPoint += tower.template.towerPoint;

        switch (tower.template.placementType)
        {
            case TowerType.powerUp:
                PowerUp powerUp = (PowerUp)tower;
                ApplyModifier(powerUp);
                powerUpPlaced.Add(powerUp);
                break;
            case TowerType.Turret:
                Turret turret = (Turret)tower;
                atLeastOneTurret = true;
                ApplyModifiersToTurret(turret);
                turretsPlaced.Add(turret);
                break;
        }
    }

    public void ApplyModifiersToTurret(Turret turret)
    {
        foreach (PowerUp powerUp in powerUpPlaced)
        {
            turret.Upgrade((PowerUpTemplate)powerUp.template);
        }
    }

    public void ApplyModifier(PowerUp powerUp) 
    {
        foreach (Turret turret in turretsPlaced)
        {
            turret.Upgrade((PowerUpTemplate)powerUp.template);
        }
    }

    private void ResetTurrets()
    {
        foreach (Turret turret in turretsPlaced)
        {
            turret.ResetUpgrade();
        }
    }
}
