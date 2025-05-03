using UnityEngine;
using utilities;

public class TurretContructionManager : MonoBehaviour
{
    [SerializeField] LayerMask placableLayer;
    [SerializeField,Layer] int towerLayer;

    [SerializeField] Material validMaterial;
    [SerializeField] Material notValidMaterial;
    [SerializeField] Material PlacedRangeMaterial;

    Pot closestPot;
    TowerTemplate CurrentSelectedTower;
    GameObject towerSpawning;
    bool isValidPosition = false;


    private void Start()
    {
        GameManager.UIManagerInstance.onTurretSelected += OnTurretSelected;
    }

    private void Update()
    {
        Checks();
        SpawnInWorld();
        PlaceTurret();
    }

    private void Checks()
    {
        if (towerSpawning != null)
            closestPot = GameManager.Instance.GetCloserPot(towerSpawning.transform.position).GetComponent<Pot>();
    }

    private void SpawnInWorld()
    {
        if (CurrentSelectedTower == null) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, placableLayer))
        {
            Vector3 newPos = Vector3.zero;

            if ((hit.transform.gameObject.CompareTag("Pot") || hit.transform.gameObject.CompareTag("Turret")) 
                && (closestPot.atLeastOneTurret || CurrentSelectedTower.placementType == TowerType.Turret )
                && hit.normal == Vector3.up
                )
            {
                newPos = hit.transform.position;
                isValidPosition = closestPot.CanBuild(CurrentSelectedTower);
            }
            else
            {
                newPos = hit.point;
                isValidPosition = false;
            }


            if (towerSpawning == null)
            {
                towerSpawning = Instantiate(CurrentSelectedTower.objectPrefab, newPos, Quaternion.identity);
                towerSpawning.GetComponent<Tower>().Initialize(CurrentSelectedTower);

                if (CurrentSelectedTower.placementType == TowerType.Turret)
                {
                    towerSpawning.GetComponent<Turret>().isRotationDisabled = true;
                }
            }

            float heightDifference = towerSpawning.GetComponent<BoxCollider>().bounds.extents.y;

            towerSpawning.transform.position = newPos + new Vector3(0, hit.collider.bounds.extents.y + heightDifference, 0);

            if (isValidPosition)
            {
                ReplaceAllMaterials(towerSpawning, validMaterial);
            }
            else
            {
                ReplaceAllMaterials(towerSpawning, notValidMaterial);
            }
        }
    }

    private void PlaceTurret()
    {
        if (Input.GetMouseButtonDown(0) && isValidPosition)
        {
            if (GameManager.Instance.PurchaseTurret(CurrentSelectedTower))
            {
                GameObject placedTurret = Instantiate(CurrentSelectedTower.objectPrefab, towerSpawning.transform.position, towerSpawning.transform.rotation);

                Tower tower = placedTurret.GetComponent<Tower>();

                tower.Initialize(CurrentSelectedTower);

                closestPot.AddTower(tower);

                placedTurret.layer = towerLayer;

                tower.template.isSelected = false;
                OnTurretSelected(null);
            }
        }
    }

    public static void ReplaceAllMaterials(GameObject root, Material newMaterial)
    {
        Renderer[] renderers = root.GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            // Assign the material to all sub-meshes
            Material[] newMaterials = new Material[renderer.sharedMaterials.Length];
            for (int i = 0; i < newMaterials.Length; i++)
            {
                newMaterials[i] = newMaterial;
            }
            renderer.materials = newMaterials;
        }
    }

    private void OnTurretSelected(TowerTemplate tower)
    {
        Destroy(towerSpawning);
        isValidPosition = false;
        CurrentSelectedTower = tower;

        Debug.Log("Tower changed to " + tower);
    }
}
