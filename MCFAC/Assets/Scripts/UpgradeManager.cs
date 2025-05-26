using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public enum UpgradeType { 
    WaterCanCapacity,
    WaterCanFireRate,
    WaterCanSideSpread,
    WaterCanRange,
    WaterCanKb,
    GetSeed,
    SpawnRandomSeed,
    IncreasePondGeneration,
    WalkingSpeed,
    WaterCanAutoRefill,
    WaterCanHealsPlants,
    PlantFireRate,
    PlantRange,
    PlantKb,
    WaterCanRefillSpeed,
}

public enum NumberUpgradeType { 
    Value,
    Multiply
}

public enum Rarity { 
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}

[System.Serializable]
public class Upgrade {
    public UpgradeType upgrade_type;
    public string text;
    public NumberUpgradeType num_upgrade_type;
    public float upgrade_value;
    public PlantType plant_type;
    public int times_can_appear = 1;
    public Rarity rarity;
}

public class UpgradeManager : MonoBehaviour
{
    [SerializeField]
    List<Upgrade> upgrades;

    [SerializeField]
    WateringCanManager wcm;

    [SerializeField]
    PlayerController pc;

    [SerializeField]
    List<TextMeshProUGUI> option_texts;

    [SerializeField]
    List<UnityEngine.UI.Button> buttons;

    [SerializeField]
    RectTransform upgrade_menu;

    [SerializeField]
    AnimationCurve ac;

    [SerializeField]
    float time_to_show_upgrades = 2f;

    List<Upgrade> current_upgrade_options;

    Subscription<ShowUpgradesEvent> upgrades_sub;

    // Start is called before the first frame update
    void Start()
    {
        current_upgrade_options = new List<Upgrade>();

        upgrades_sub = EventBus.Subscribe<ShowUpgradesEvent>(_ShowUpgrades);

        SetUpgrades();

        foreach(UnityEngine.UI.Button button in buttons) {
            button.interactable = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void _ShowUpgrades(ShowUpgradesEvent e) {
        Time.timeScale = 0.0f;
        StartCoroutine(LerpScale(upgrade_menu, Vector3.zero, Vector3.one, time_to_show_upgrades, true, ac));

        SetUpgrades();
    }

    public void HideUpgrades() {
        Time.timeScale = 1.0f;
        StartCoroutine(LerpScale(upgrade_menu, Vector3.one, Vector3.zero, time_to_show_upgrades, false, ac));

        EventBus.Publish(new DoneUpgradesEvent());

    }

    IEnumerator LerpScale(RectTransform rt, Vector3 start_scale, Vector3 end_scale, float time, bool clickable, AnimationCurve anim_curve = null) {
        float progress = 0;
        float start_time = Time.unscaledTime;

        while(progress < 1) {
            progress = (Time.unscaledTime - start_time) / time;

            Vector3 new_scale;
            if(anim_curve != null) {
                new_scale = Vector3.Lerp(start_scale, end_scale, anim_curve.Evaluate(progress));
            }
            else { 
                new_scale = Vector3.Lerp(start_scale, end_scale, progress);
            }
            rt.localScale = new_scale;

            yield return null;
        }

        rt.localScale = end_scale;

        foreach(UnityEngine.UI.Button button in buttons) {
            button.interactable = clickable;
        }
    }

    public void SetUpgrades() {
        current_upgrade_options.Clear();

        while(current_upgrade_options.Count < 3) {
            int rand_idx = Random.Range(0, upgrades.Count);

            Upgrade selected_upgrade = upgrades[rand_idx];

            bool is_already_in = false;
            foreach(Upgrade upgrade in current_upgrade_options) { 
                if(upgrade == selected_upgrade) {
                    is_already_in = true;
                    break;
                }
            }
            if(is_already_in == false) { 
                current_upgrade_options.Add(selected_upgrade);
            }
        }

        UpdateUpgradeText();
    }

    public void UpdateUpgradeText() { 
        for(int i = 0; i < current_upgrade_options.Count; ++i) {
            option_texts[i].text = current_upgrade_options[i].text;
        }
    }

    public void OptionClicked(int option_num) {
        foreach(UnityEngine.UI.Button button in buttons) {
            button.interactable = false;
        }

        ProcessUpgrade(current_upgrade_options[option_num]);

        HideUpgrades();
    }

    void ProcessUpgrade(Upgrade upgrade) { 

        switch(upgrade.upgrade_type) {
            case UpgradeType.WaterCanCapacity:
                wcm.UpgradeCapacity(upgrade.num_upgrade_type, upgrade.upgrade_value);
                break;
            case UpgradeType.WaterCanFireRate:
                wcm.UpgradeFireRate(upgrade.num_upgrade_type, upgrade.upgrade_value);
                break;
            case UpgradeType.WaterCanSideSpread:
                wcm.UpgradeSideSpread(upgrade.num_upgrade_type, upgrade.upgrade_value);
                break;
            case UpgradeType.WaterCanRange:
                wcm.UpgradeRange(upgrade.num_upgrade_type, upgrade.upgrade_value);
                break;
            case UpgradeType.WaterCanRefillSpeed:
                wcm.UpgradeRefillRate(upgrade.num_upgrade_type, upgrade.upgrade_value);
                break;
            case UpgradeType.GetSeed:
                EventBus.Publish(new AquireSeedEvent(upgrade.plant_type));
                break;
            case UpgradeType.WalkingSpeed:
                pc.UpgradeWalkingSpeed(upgrade.num_upgrade_type, upgrade.upgrade_value);
                break;
            case UpgradeType.WaterCanAutoRefill:
                wcm.UpgradeAutoRefill(upgrade.num_upgrade_type, upgrade.upgrade_value);
                break;
            default:
                Debug.Log("Upgrade Not Implemented");
                break;
        }

        upgrade.times_can_appear -= 1;

        if(upgrade.times_can_appear <= 0) {
            upgrades.Remove(upgrade);
        }

    }


}
