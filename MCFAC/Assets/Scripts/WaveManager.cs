using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public enum EnemyType { 
    Basic,
    Speed,
    StrongBasic,
    Tank,
    Boss
}

[System.Serializable]
public class EnemyToPrefab {
    public EnemyType enemy_type;
    public GameObject prefab;
}

[System.Serializable]
public class EnemyGroup {
    public EnemyType enemy_type;
    public int amount = 0;
}

[System.Serializable]
public class WaveEnemies {
    public List<EnemyGroup> enemies;
}

public class WaveManager : MonoBehaviour
{
    Subscription<DoneUpgradesEvent> upgrade_sub;

    int wave_num = 0;

    [SerializeField]
    List<WaveEnemies> waves;

    [SerializeField]
    List<EnemyToPrefab> enemy_prefabs;

    [SerializeField]
    List<BoxCollider2D> spawn_regions;

    Dictionary<EnemyType, GameObject> enemy_prefabs_dict;

    [SerializeField]
    float pause_before_and_after_round = 5;

    List<GameObject> current_enemies;

    [SerializeField]
    bool has_planted_seed = false;
    [SerializeField]
    bool has_grown_seed = false;

    [SerializeField]
    TextMeshProUGUI wave_text;


    // Start is called before the first frame update
    void Start()
    {
        enemy_prefabs_dict = new Dictionary<EnemyType, GameObject>();
        foreach(EnemyToPrefab enemy_prefab in enemy_prefabs) {
            enemy_prefabs_dict[enemy_prefab.enemy_type] = enemy_prefab.prefab;
        }

        current_enemies = new List<GameObject>();

        upgrade_sub = EventBus.Subscribe<DoneUpgradesEvent>(_OnDoneUpgrades);

        StartCoroutine(TutorialSequence());
    }

    void _OnDoneUpgrades(DoneUpgradesEvent e) {
        StartCoroutine(WaveLogic());
    }

    IEnumerator TutorialSequence() {
        yield return null;

        List<PlantType> starting_plant_types = new List<PlantType>();
        starting_plant_types.Add(PlantType.YellowFlower);
        starting_plant_types.Add(PlantType.PurpleFlower);
        starting_plant_types.Add(PlantType.RedFlower);
        starting_plant_types.Add(PlantType.GreenCactus);

        EventBus.Publish(new AquireSeedEvent(starting_plant_types[Random.Range(0, starting_plant_types.Count)]));

        while(has_planted_seed == false) {
            yield return new WaitForSeconds(1f);
        }

        while(has_grown_seed == false) {
            yield return new WaitForSeconds(1f);
        }

        Debug.Log("Finished Tutorial");

        StartCoroutine(WaveLogic());
    }

    public void HasPlantedSeed() { 
        has_planted_seed = true;
    }

    public void HasGrownSeed() { 
        has_grown_seed = true;
    }


    IEnumerator WaveLogic() {
        yield return new WaitForSeconds(pause_before_and_after_round);

        // Spawn the enemies
        current_enemies.Clear();
        WaveEnemies wave_enemies = waves[wave_num];

        BoxCollider2D spawn_region = spawn_regions[Random.Range(0, spawn_regions.Count)];

        foreach(EnemyGroup enemy_group in wave_enemies.enemies) { 
            for(int i = 0; i < enemy_group.amount; ++i) {
                Vector3 spawn_pos = GetValidSpawnLocation(spawn_region);
                current_enemies.Add(Instantiate(enemy_prefabs_dict[enemy_group.enemy_type], spawn_pos, Quaternion.identity));
            }
        }


        while(true) {
            // Could be implemented better
            foreach(GameObject enemy in current_enemies) { 
                if(enemy == null) {
                    current_enemies.Remove(enemy);
                    break;
                }
            }

            if(current_enemies.Count == 0) {
                yield return new WaitForSeconds(pause_before_and_after_round);
                FinishWave();
                yield break;
            }

            yield return null;
        }
    }

    void FinishWave() { 
        wave_num++;
        wave_text.text = "WAVE: " + wave_num.ToString();
        if(wave_num >= waves.Count) { 
            EventBus.Publish(new GameOverEvent(true));
            return;
        }


        Debug.Log("Going to the next wave");
        EventBus.Publish(new ShowUpgradesEvent());
    
    }

    Vector3 GetValidSpawnLocation(BoxCollider2D spawn_area) {
        Bounds bounds = spawn_area.bounds;
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);
        return new Vector3(x, y, 0);
    }


    // Update is called once per frame
    void Update()
    {
        
    }

}
