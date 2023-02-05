using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSystem : MonoBehaviour
{
    public GameObject Fairy;
    public GameObject LightSpirit;
    private float[][] costTenacity =
    {
        new float[] { 15.0f, 0.0f, 0.0f, 0.0f, 0.0f }, // nj sl wt mt dd
        new float[] { 40.0f, 0.0f, 0.0f, 0.0f, 0.0f },
        new float[] { 90.0f, 0.0f, 0.0f, 0.0f, 0.0f },
        new float[] { 165.0f, 0.0f, 0.0f, 0.0f, 0.0f },
        new float[] { 265.0f, 0.0f, 0.0f, 0.0f, 0.0f },
        new float[] { 390.0f, 0.0f, 0.0f, 0.0f, 0.0f },

        new float[] { 540.0f, 0.0f, 0.0f, 0.0f, 0.0f },
        new float[] { 715.0f, 0.0f, 0.0f, 15.0f, 0.0f },
        new float[] { 915.0f, 0.0f, 0.0f, 30.0f, 0.0f },
        new float[] { 1140.0f, 0.0f, 0.0f, 50.0f, 0.0f },
        new float[] { 1390.0f, 0.0f, 0.0f, 75.0f, 0.0f },
        new float[] { 1665.0f, 0.0f, 0.0f, 105.0f, 0.0f },
    };
    private float[][] costAttack =
    {
        new float[] { 10.0f, 0.0f, 15.0f, 0.0f, 0.0f },
        new float[] { 30.0f, 0.0f, 40.0f, 0.0f, 0.0f },
        new float[] { 70.0f, 0.0f, 90.0f, 0.0f, 0.0f },
        new float[] { 130.0f, 0.0f, 165.0f, 0.0f, 0.0f },
        new float[] { 210.0f, 0.0f, 265.0f, 0.0f, 0.0f },
        new float[] { 310.0f, 0.0f, 390.0f, 0.0f, 0.0f },

        new float[] { 430.0f, 0.0f, 540.0f, 0.0f, 0.0f },
        new float[] { 570.0f, 0.0f, 715.0f, 15.0f, 0.0f },
        new float[] { 730.0f, 0.0f, 915.0f, 30.0f, 0.0f },
        new float[] { 910.0f, 0.0f, 1140.0f, 50.0f, 0.0f },
        new float[] { 1110.0f, 0.0f, 1390.0f, 75.0f, 0.0f },
        new float[] { 1330.0f, 0.0f, 1665.0f, 105.0f, 0.0f },
    };
    private float[][] costSpeed =
    {
        new float[] { 0.0f, 15.0f, 10.0f, 0.0f, 0.0f },
        new float[] { 0.0f, 40.0f, 30.0f, 0.0f, 0.0f },
        new float[] { 0.0f, 90.0f, 70.0f, 0.0f, 0.0f },
        new float[] { 0.0f, 165.0f, 130.0f, 0.0f, 0.0f },
        new float[] { 0.0f, 265.0f, 210.0f, 0.0f, 0.0f },
        new float[] { 0.0f, 390.0f, 310.0f, 0.0f, 0.0f },

        new float[] { 0.0f, 540.0f, 430.0f, 0.0f, 0.0f },
        new float[] { 0.0f, 715.0f, 570.0f, 0.0f, 15.0f },
        new float[] { 0.0f, 915.0f, 730.0f, 0.0f, 30.0f },
        new float[] { 0.0f, 1140.0f, 910.0f, 0.0f, 50.0f },
        new float[] { 0.0f, 1390.0f, 1110.0f, 0.0f, 75.0f },
        new float[] { 0.0f, 1665.0f, 1330.0f, 0.0f, 105.0f },
    };
    private float[][] costAbsorb =
    {
        new float[] { 5.0f, 10.0f, 0.0f, 0.0f, 0.0f },
        new float[] { 20.0f, 30.0f, 0.0f, 0.0f, 0.0f },
        new float[] { 40.0f, 70.0f, 0.0f, 0.0f, 0.0f },
        new float[] { 85.0f, 130.0f, 0.0f, 0.0f, 0.0f },
        new float[] { 145.0f, 210.0f, 0.0f, 0.0f, 0.0f },
        new float[] { 220.0f, 310.0f, 0.0f, 0.0f, 0.0f },

        new float[] { 310.0f, 430.0f, 0.0f, 0.0f, 0.0f },
        new float[] { 415.0f, 70.0f, 0.0f, 0.0f, 15.0f },
        new float[] { 535.0f, 730.0f, 0.0f, 0.0f, 30.0f },
        new float[] { 670.0f, 910.0f, 0.0f, 0.0f, 50.0f },
        new float[] { 820.0f, 1110.0f, 0.0f, 0.0f, 75.0f },
        new float[] { 985.0f, 1330.0f, 0.0f, 0.0f, 105.0f },
    };
    private float[][] costNewRoot =
    {
        new float[] { 150.0f, 0.0f, 0.0f, 0.0f, 0.0f },     //4th
        new float[] { 400.0f, 0.0f, 0.0f, 0.0f, 0.0f },     //5th
        new float[] { 1000.0f, 0.0f, 0.0f, 0.0f, 0.0f },    //6th
        new float[] { 1900.0f, 0.0f, 0.0f, 0.0f, 150.0f },  //7th
        new float[] { 3100.0f, 0.0f, 0.0f, 0.0f, 400.0f },   //8th
    };
    private float[][] costFairy =
    {
        new float[] { 0.0f, 0.0f, 250.0f, 0.0f, 0.0f },
        new float[] { 0.0f, 0.0f, 350.0f, 0.0f, 0.0f },
        new float[] { 0.0f, 0.0f, 500.0f, 0.0f, 0.0f },
        new float[] { 0.0f, 0.0f, 650.0f, 0.0f, 0.0f },
        new float[] { 0.0f, 0.0f, 950.0f, 0.0f, 0.0f },
        new float[] { 0.0f, 0.0f, 1250.0f, 0.0f, 0.0f },

        new float[] { 0.0f, 0.0f, 1550.0f, 0.0f, 0.0f },
        new float[] { 0.0f, 0.0f, 1900.0f, 30.0f, 0.0f },
        new float[] { 0.0f, 0.0f, 2300.0f, 60.0f, 0.0f },
        new float[] { 0.0f, 0.0f, 2750.0f, 100.0f, 0.0f },
        new float[] { 0.0f, 0.0f, 3250.0f, 150.0f, 0.0f },
        new float[] { 0.0f, 0.0f, 3800.0f, 210.0f, 0.0f },
    };
    private float[][] costLightSpirit =
    {
        new float[] { 200.0f, 0.0f, 0.0f, 15.0f, 0.0f },
        new float[] { 250.0f, 0.0f, 0.0f, 20.0f, 0.0f },
        new float[] { 350.0f, 0.0f, 0.0f, 30.0f, 0.0f },
        new float[] { 500.0f, 0.0f, 0.0f, 45.0f, 0.0f },
        new float[] { 700.0f, 0.0f, 0.0f, 65.0f, 0.0f },
        new float[] { 950.0f, 0.0f, 0.0f, 90.0f, 30.0f },

        new float[] { 1250.0f, 0.0f, 0.0f, 120.0f, 60.0f },
        new float[] { 1600.0f, 0.0f, 0.0f, 155.0f, 95.0f },
        new float[] { 2000.0f, 0.0f, 0.0f, 195.0f, 135.0f },
        new float[] { 2450.0f, 0.0f, 0.0f, 240.0f, 180.0f },
        new float[] { 2950.0f, 0.0f, 0.0f, 290.0f, 230.0f },
        new float[] { 3500.0f, 0.0f, 0.0f, 345.0f, 285.0f },
    };
    private float[][] costRootSpikes =
    {
        new float[] { 0.0f, 250.0f, 0.0f, 15.0f, 0.0f },
        new float[] { 0.0f, 300.0f, 0.0f, 20.0f, 0.0f },
        new float[] { 0.0f, 400.0f, 0.0f, 30.0f, 30.0f },
        new float[] { 0.0f, 550.0f, 0.0f, 45.0f, 60.0f },
        new float[] { 0.0f, 750.0f, 0.0f, 65.0f, 95.0f },
        new float[] { 0.0f, 1000.0f, 0.0f, 90.0f, 135.0f },

        new float[] { 0.0f, 1300.0f, 0.0f, 120.0f, 180.0f },
        new float[] { 0.0f, 1650.0f, 0.0f, 155.0f, 230.0f },
        new float[] { 0.0f, 2050.0f, 0.0f, 195.0f, 285.0f },
        new float[] { 0.0f, 2500.0f, 0.0f, 240.0f, 345.0f },
        new float[] { 0.0f, 3000.0f, 0.0f, 290.0f, 410.0f },
        new float[] { 0.0f, 3550.0f, 0.0f, 345.0f, 480.0f },
    };
    private float[][] costBombBulb =
    {
        new float[] { 0.0f, 0.0f, 500.0f, 0.0f, 30.0f },
        new float[] { 0.0f, 0.0f, 650.0f, 0.0f, 60.0f },
        new float[] { 0.0f, 0.0f, 950.0f, 0.0f, 95.0f },
        new float[] { 0.0f, 0.0f, 1400.0f, 0.0f, 135.0f },
        new float[] { 0.0f, 0.0f, 2000.0f, 0.0f, 180.0f },
        new float[] { 0.0f, 0.0f, 2750.0f, 0.0f, 230.0f },

        new float[] { 0.0f, 0.0f, 3650.0f, 0.0f, 285.0f },
        new float[] { 0.0f, 0.0f, 4700.0f, 0.0f, 345.0f },
        new float[] { 0.0f, 0.0f, 5900.0f, 0.0f, 410.0f },
        new float[] { 0.0f, 0.0f, 7250.0f, 0.0f, 480.0f },
        new float[] { 0.0f, 0.0f, 8750.0f, 0.0f, 555.0f },
        new float[] { 0.0f, 0.0f, 10400.0f, 0.0f, 635.0f },
    };
    public int fairyLevel = 0;
    public int lightSpiritLevel = 0;
    public int rootSpikesLevel = 0;
    public int bombBulbLevel = 0;
    public int pointingRoot = 0;

    public float Cost_NutriJewel = 0.0f;
    public float Cost_SlimeStone = 0.0f;
    public float Cost_ManaWater = 0.0f;
    public float Cost_Meteorite = 0.0f;
    public float Cost_DarkDust = 0.0f;

    private float[] maxHPValue = new float[] { 350.0f, 450.0f, 550.0f, 700.0f, 900.0f, 1100.0f
                                            , 1300.0f, 1500.0f, 1800.0f, 2100.0f, 2400.0f, 2800.0f };//Default 300
    private float[] attackValue = new float[] { 35.0f, 40.0f, 50.0f, 60.0f, 75.0f, 90.0f
                                            , 110.0f, 130.0f, 155.0f, 185.0f, 225.0f, 270.0f };//Default 30
    private float[] speedValue = new float[] { 0.2f, 0.23f, 0.26f, 0.3f, 0.34f, 0.39f
                                            , 0.45f, 0.53f, 0.62f, 0.73f, 0.85f, 1.0f };//Default 0.17f (OnDrag)
    private float[] absorbValue = new float[] { 6.0f, 7.5f, 9.0f, 10.5f, 13.5f, 16.5f
                                                , 21.0f, 27.0f, 36.0f, 46.5f, 60.0f, 75.0f };//Default 1.0f
    private float[] fairyAttackValue = new float[] { 15.0f, 16.0f, 17.0f, 18.0f, 20.0f, 23.0f
                                                    , 27.0f, 32.0f, 38.0f, 45.0f, 53.0f, 65.0f };//Default 15.0f
    private float[] lightSpiritHealingValue = new float[] { 3.0f, 3.1f, 3.2f, 3.3f, 3.5f, 3.8f
                                                            , 4.3f, 5.0f, 6.0f, 7.2f, 8.5f, 10.0f };//Default 3.0f
    private float[] rootSpikesValue_Attack = new float[] { 100.0f, 125.0f, 150.0f, 175.0f, 200.0f, 250.0f
                                                        , 300.0f, 350.0f, 400.0f, 450.0f, 550.0f, 650.0f };//Default 100.0f
    private float[] rootSpikesValue_Size = new float[] { 1.0f, 1.15f, 1.3f, 1.5f, 1.7f, 1.9f
                                                        , 2.15f, 2.4f, 2.7f, 3.1f, 3.6f, 4.3f };//Default 1.0f
    private float[] bombBulbValue_Attack = new float[] { 30.0f, 40.0f, 50.0f, 65.0f, 80.0f, 100.0f
                                                        , 120.0f, 145.0f, 170.0f, 200.0f, 240.0f, 300.0f };//Default 30.0f
    private float[] bombBulbValue_Size = new float[] { 1.25f, 1.35f, 1.45f, 1.55f, 1.7f, 1.9f
                                                        , 2.15f, 2.4f, 2.7f, 3.1f, 3.6f, 4.3f };//Default 1.25f

    private Tree tree;

    // Start is called before the first frame update
    void Start()
    {
        tree = FindObjectOfType<Tree>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            MovePointingRoot(true);
        }
        //Root Information
            //tree.Roots[pointingRoot].GetComponent<Root>().tenacityLevel
            //tree.Roots[pointingRoot].GetComponent<Root>().attackLevel
            //tree.Roots[pointingRoot].GetComponent<Root>().speedLevel
            //tree.Roots[pointingRoot].GetComponent<Root>().absorbLevel

        if (Input.GetKeyDown(KeyCode.Q)) {
            TenacityLevelUp();
        }
        if (Input.GetKeyDown(KeyCode.W)) {
            AttackLevelUp();
        }
        if (Input.GetKeyDown(KeyCode.E)) {
            SpeedLevelUp();
        }
        if (Input.GetKeyDown(KeyCode.R)) {
            //Absorb Level Up
            AbsorbLevelUp();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            //New root
            Debug.Log("2 is pressed");
            NewRoot();
            //RootSpikes.GetComponent<RootSpikesProperties>().damage = 15.0f;
            //RootSpikes.GetComponent<RootSpikesProperties>().size = 1.0f;
            //RootSpikes.GetComponent<RootSpikesProperties>().timeSpawned = GameManager.Instance.gameTime;
            //RootSpikes.GetComponent<RootSpikesProperties>().collided = false;
            //Spawn(RootSpikes, GameObject.FindWithTag("Cursor").transform.position, Vector3.zero);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3)) {
            //New fairy
            Debug.Log("3 is pressed");
            NewFairy();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4)) {
            //New light spirit
            Debug.Log("4 is pressed");
            NewLightSpirit();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5)) {
            //Root Spikes Level Up
            Debug.Log("5 is pressed");
            SpikeLevelUp();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6)) {
            //Bomb Bulb Level Up
            BombBulbLevelUp();
        }
    }
    
    private void Spawn(GameObject gameObj, Vector3 pPos, Vector3 pRot) {
        GameObject newGameObj = Instantiate(gameObj);
        newGameObj.transform.parent = transform;
        newGameObj.transform.position = pPos;
        newGameObj.transform.eulerAngles = pRot;
    }

    public event Action<int> pointingRootUpdate;
    public void MovePointingRoot(bool goRight) {
        //Rotate amongst Roots (0, 1, 2, 0, 1, 2 etc.)
        if (goRight) {
            if (pointingRoot + 1 >= tree.Roots.Count) {
                pointingRoot = 0;
            }
            else {
                pointingRoot++;
            }
        }
        else {
            if (pointingRoot - 1 < 0) {
                pointingRoot = tree.Roots.Count - 1;
            }
            else {
                pointingRoot--;
            }
        }
        //Debug.Log("1 is pressed, choosing new root...");
        Debug.Log("Chose Root, index = " + pointingRoot);
        pointingRootUpdate?.Invoke(pointingRoot);
    }

    public event Action<int> tenacityLevelUpdate;
    public void TenacityLevelUp(bool isFetch = false) {
        //Tenacity Level Up
        Cost_NutriJewel = costTenacity[ tree.Roots[pointingRoot].GetComponent<Root>().tenacityLevel ][0];
        Cost_SlimeStone = costTenacity[ tree.Roots[pointingRoot].GetComponent<Root>().tenacityLevel ][1];
        Cost_ManaWater = costTenacity[ tree.Roots[pointingRoot].GetComponent<Root>().tenacityLevel ][2];
        Cost_Meteorite = costTenacity[ tree.Roots[pointingRoot].GetComponent<Root>().tenacityLevel ][3];
        Cost_DarkDust = costTenacity[ tree.Roots[pointingRoot].GetComponent<Root>().tenacityLevel ][4];
        if (isFetch) return;
        
        if (tree.Roots[pointingRoot].GetComponent<Root>().tenacityLevel < 12) {
            Cost_NutriJewel = costTenacity[ tree.Roots[pointingRoot].GetComponent<Root>().tenacityLevel ][0];
            Cost_SlimeStone = costTenacity[ tree.Roots[pointingRoot].GetComponent<Root>().tenacityLevel ][1];
            Cost_ManaWater = costTenacity[ tree.Roots[pointingRoot].GetComponent<Root>().tenacityLevel ][2];
            Cost_Meteorite = costTenacity[ tree.Roots[pointingRoot].GetComponent<Root>().tenacityLevel ][3];
            Cost_DarkDust = costTenacity[ tree.Roots[pointingRoot].GetComponent<Root>().tenacityLevel ][4];
        }
            
        if (tree.Roots[pointingRoot].GetComponent<Root>().tenacityLevel  < 12 && EnoughResources(Cost_NutriJewel, Cost_SlimeStone, Cost_ManaWater, Cost_Meteorite, Cost_DarkDust)) { //Cost fulfilled
            tree.resAmount[GameResourcesType.Nutri_Jewel] -= Cost_NutriJewel;
            tree.resAmount[GameResourcesType.Slime_Stone] -= Cost_SlimeStone;
            tree.resAmount[GameResourcesType.Mana_Water] -= Cost_ManaWater;
            tree.resAmount[GameResourcesType.Meteorite] -= Cost_Meteorite;
            tree.resAmount[GameResourcesType.Dark_Dust] -= Cost_DarkDust;
                
            tree.Roots[pointingRoot].GetComponent<Root>().HP += (maxHPValue[tree.Roots[pointingRoot].GetComponent<Root>().tenacityLevel] - tree.Roots[pointingRoot].GetComponent<Root>().maxHP);
            tree.Roots[pointingRoot].GetComponent<Root>().maxHP = maxHPValue[tree.Roots[pointingRoot].GetComponent<Root>().tenacityLevel];

            tree.Roots[pointingRoot].GetComponent<Root>().tenacityLevel  += 1;
        }
        //Debug.Log("Q is pressed, Root " + pointingRoot + "\'s Tenacity Level = " + tree.Roots[pointingRoot].GetComponent<Root>().tenacityLevel);
        Debug.Log("PoointingRoot " + pointingRoot + "\'s Tenacity Level = " + tree.Roots[pointingRoot].GetComponent<Root>().tenacityLevel);
        
        Cost_NutriJewel = costTenacity[ tree.Roots[pointingRoot].GetComponent<Root>().tenacityLevel ][0];
        Cost_SlimeStone = costTenacity[ tree.Roots[pointingRoot].GetComponent<Root>().tenacityLevel ][1];
        Cost_ManaWater = costTenacity[ tree.Roots[pointingRoot].GetComponent<Root>().tenacityLevel ][2];
        Cost_Meteorite = costTenacity[ tree.Roots[pointingRoot].GetComponent<Root>().tenacityLevel ][3];
        Cost_DarkDust = costTenacity[ tree.Roots[pointingRoot].GetComponent<Root>().tenacityLevel ][4];
        
        tenacityLevelUpdate?.Invoke(tree.Roots[pointingRoot].GetComponent<Root>().tenacityLevel);
        
    }
    
    public event Action<int> attackLevelUpdate;
    public void AttackLevelUp(bool isFetch = false) {
        Cost_NutriJewel = costAttack[ tree.Roots[pointingRoot].GetComponent<Root>().attackLevel ][0];
        Cost_SlimeStone = costAttack[ tree.Roots[pointingRoot].GetComponent<Root>().attackLevel ][1];
        Cost_ManaWater = costAttack[ tree.Roots[pointingRoot].GetComponent<Root>().attackLevel ][2];
        Cost_Meteorite = costAttack[ tree.Roots[pointingRoot].GetComponent<Root>().attackLevel ][3];
        Cost_DarkDust = costAttack[ tree.Roots[pointingRoot].GetComponent<Root>().attackLevel ][4];
        if (isFetch) return;
        if (tree.Roots[pointingRoot].GetComponent<Root>().attackLevel < 12) {
            Cost_NutriJewel = costAttack[ tree.Roots[pointingRoot].GetComponent<Root>().attackLevel ][0];
            Cost_SlimeStone = costAttack[ tree.Roots[pointingRoot].GetComponent<Root>().attackLevel ][1];
            Cost_ManaWater = costAttack[ tree.Roots[pointingRoot].GetComponent<Root>().attackLevel ][2];
            Cost_Meteorite = costAttack[ tree.Roots[pointingRoot].GetComponent<Root>().attackLevel ][3];
            Cost_DarkDust = costAttack[ tree.Roots[pointingRoot].GetComponent<Root>().attackLevel ][4];
        }
            
        if (tree.Roots[pointingRoot].GetComponent<Root>().attackLevel  < 12 && EnoughResources(Cost_NutriJewel, Cost_SlimeStone, Cost_ManaWater, Cost_Meteorite, Cost_DarkDust)) { //Cost fulfilled
            tree.resAmount[GameResourcesType.Nutri_Jewel] -= Cost_NutriJewel;
            tree.resAmount[GameResourcesType.Slime_Stone] -= Cost_SlimeStone;
            tree.resAmount[GameResourcesType.Mana_Water] -= Cost_ManaWater;
            tree.resAmount[GameResourcesType.Meteorite] -= Cost_Meteorite;
            tree.resAmount[GameResourcesType.Dark_Dust] -= Cost_DarkDust;
                
            tree.Roots[pointingRoot].GetComponent<Root>().attackDamage = attackValue[tree.Roots[pointingRoot].GetComponent<Root>().attackLevel];

            tree.Roots[pointingRoot].GetComponent<Root>().attackLevel  += 1;
        }
        // Debug.Log("W is pressed, Root " + pointingRoot + "\'s Attack Level = " + tree.Roots[pointingRoot].GetComponent<Root>().attackLevel);
        Debug.Log("PointingRoot " + pointingRoot + "\'s Attack Level = " + tree.Roots[pointingRoot].GetComponent<Root>().attackLevel);
        
        Cost_NutriJewel = costAttack[ tree.Roots[pointingRoot].GetComponent<Root>().attackLevel ][0];
        Cost_SlimeStone = costAttack[ tree.Roots[pointingRoot].GetComponent<Root>().attackLevel ][1];
        Cost_ManaWater = costAttack[ tree.Roots[pointingRoot].GetComponent<Root>().attackLevel ][2];
        Cost_Meteorite = costAttack[ tree.Roots[pointingRoot].GetComponent<Root>().attackLevel ][3];
        Cost_DarkDust = costAttack[ tree.Roots[pointingRoot].GetComponent<Root>().attackLevel ][4];
        
        attackLevelUpdate?.Invoke(tree.Roots[pointingRoot].GetComponent<Root>().attackLevel);
        
    }

    public event Action<int> speedLevelUpdate;
    public void SpeedLevelUp(bool isFetch = false) {
        Cost_NutriJewel = costSpeed[ tree.Roots[pointingRoot].GetComponent<Root>().speedLevel ][0];
        Cost_SlimeStone = costSpeed[ tree.Roots[pointingRoot].GetComponent<Root>().speedLevel ][1];
        Cost_ManaWater = costSpeed[ tree.Roots[pointingRoot].GetComponent<Root>().speedLevel ][2];
        Cost_Meteorite = costSpeed[ tree.Roots[pointingRoot].GetComponent<Root>().speedLevel ][3];
        Cost_DarkDust = costSpeed[ tree.Roots[pointingRoot].GetComponent<Root>().speedLevel ][4];

        if (isFetch) return;
        if (tree.Roots[pointingRoot].GetComponent<Root>().speedLevel < 12) {
            Cost_NutriJewel = costSpeed[ tree.Roots[pointingRoot].GetComponent<Root>().speedLevel ][0];
            Cost_SlimeStone = costSpeed[ tree.Roots[pointingRoot].GetComponent<Root>().speedLevel ][1];
            Cost_ManaWater = costSpeed[ tree.Roots[pointingRoot].GetComponent<Root>().speedLevel ][2];
            Cost_Meteorite = costSpeed[ tree.Roots[pointingRoot].GetComponent<Root>().speedLevel ][3];
            Cost_DarkDust = costSpeed[ tree.Roots[pointingRoot].GetComponent<Root>().speedLevel ][4];
        }
            
        if (tree.Roots[pointingRoot].GetComponent<Root>().speedLevel  < 12 && EnoughResources(Cost_NutriJewel, Cost_SlimeStone, Cost_ManaWater, Cost_Meteorite, Cost_DarkDust)) { //Cost fulfilled
            tree.resAmount[GameResourcesType.Nutri_Jewel] -= Cost_NutriJewel;
            tree.resAmount[GameResourcesType.Slime_Stone] -= Cost_SlimeStone;
            tree.resAmount[GameResourcesType.Mana_Water] -= Cost_ManaWater;
            tree.resAmount[GameResourcesType.Meteorite] -= Cost_Meteorite;
            tree.resAmount[GameResourcesType.Dark_Dust] -= Cost_DarkDust;
                
            tree.Roots[pointingRoot].GetComponent<Root>().speed = speedValue[tree.Roots[pointingRoot].GetComponent<Root>().speedLevel];

            tree.Roots[pointingRoot].GetComponent<Root>().speedLevel  += 1;
        }
        // Debug.Log("E is pressed, Root " + pointingRoot + "\'s Speed Level = " + tree.Roots[pointingRoot].GetComponent<Root>().speedLevel);
        Debug.Log("PointingRoot " + pointingRoot + "\'s Speed Level = " + tree.Roots[pointingRoot].GetComponent<Root>().speedLevel);
        
        Cost_NutriJewel = costSpeed[ tree.Roots[pointingRoot].GetComponent<Root>().speedLevel ][0];
        Cost_SlimeStone = costSpeed[ tree.Roots[pointingRoot].GetComponent<Root>().speedLevel ][1];
        Cost_ManaWater = costSpeed[ tree.Roots[pointingRoot].GetComponent<Root>().speedLevel ][2];
        Cost_Meteorite = costSpeed[ tree.Roots[pointingRoot].GetComponent<Root>().speedLevel ][3];
        Cost_DarkDust = costSpeed[ tree.Roots[pointingRoot].GetComponent<Root>().speedLevel ][4];
        
        speedLevelUpdate?.Invoke(tree.Roots[pointingRoot].GetComponent<Root>().speedLevel);
    }

    public event Action<int> absorbLevelUpdate;
    public void AbsorbLevelUp(bool isFetch = false) {
        Cost_NutriJewel = costAbsorb[ tree.Roots[pointingRoot].GetComponent<Root>().absorbLevel ][0];
        Cost_SlimeStone = costAbsorb[ tree.Roots[pointingRoot].GetComponent<Root>().absorbLevel ][1];
        Cost_ManaWater = costAbsorb[ tree.Roots[pointingRoot].GetComponent<Root>().absorbLevel ][2];
        Cost_Meteorite = costAbsorb[ tree.Roots[pointingRoot].GetComponent<Root>().absorbLevel ][3];
        Cost_DarkDust = costAbsorb[ tree.Roots[pointingRoot].GetComponent<Root>().absorbLevel ][4];
        if (isFetch) return;
        if (tree.Roots[pointingRoot].GetComponent<Root>().absorbLevel < 12) {
            Cost_NutriJewel = costAbsorb[ tree.Roots[pointingRoot].GetComponent<Root>().absorbLevel ][0];
            Cost_SlimeStone = costAbsorb[ tree.Roots[pointingRoot].GetComponent<Root>().absorbLevel ][1];
            Cost_ManaWater = costAbsorb[ tree.Roots[pointingRoot].GetComponent<Root>().absorbLevel ][2];
            Cost_Meteorite = costAbsorb[ tree.Roots[pointingRoot].GetComponent<Root>().absorbLevel ][3];
            Cost_DarkDust = costAbsorb[ tree.Roots[pointingRoot].GetComponent<Root>().absorbLevel ][4];
        }
            
        if (tree.Roots[pointingRoot].GetComponent<Root>().absorbLevel  < 12 && EnoughResources(Cost_NutriJewel, Cost_SlimeStone, Cost_ManaWater, Cost_Meteorite, Cost_DarkDust)) { //Cost fulfilled
            tree.resAmount[GameResourcesType.Nutri_Jewel] -= Cost_NutriJewel;
            tree.resAmount[GameResourcesType.Slime_Stone] -= Cost_SlimeStone;
            tree.resAmount[GameResourcesType.Mana_Water] -= Cost_ManaWater;
            tree.resAmount[GameResourcesType.Meteorite] -= Cost_Meteorite;
            tree.resAmount[GameResourcesType.Dark_Dust] -= Cost_DarkDust;
                
            tree.Roots[pointingRoot].GetComponent<Root>().absorb = absorbValue[tree.Roots[pointingRoot].GetComponent<Root>().absorbLevel];

            tree.Roots[pointingRoot].GetComponent<Root>().absorbLevel  += 1;
        }
        // Debug.Log("R is pressed, Root " + pointingRoot + "\'s Absorb Level = " + tree.Roots[pointingRoot].GetComponent<Root>().absorbLevel);
        Debug.Log("PointingRoot " + pointingRoot + "\'s Absorb Level = " + tree.Roots[pointingRoot].GetComponent<Root>().absorbLevel);
        
        Cost_NutriJewel = costAbsorb[ tree.Roots[pointingRoot].GetComponent<Root>().absorbLevel ][0];
        Cost_SlimeStone = costAbsorb[ tree.Roots[pointingRoot].GetComponent<Root>().absorbLevel ][1];
        Cost_ManaWater = costAbsorb[ tree.Roots[pointingRoot].GetComponent<Root>().absorbLevel ][2];
        Cost_Meteorite = costAbsorb[ tree.Roots[pointingRoot].GetComponent<Root>().absorbLevel ][3];
        Cost_DarkDust = costAbsorb[ tree.Roots[pointingRoot].GetComponent<Root>().absorbLevel ][4];
        
        absorbLevelUpdate?.Invoke(tree.Roots[pointingRoot].GetComponent<Root>().absorbLevel);
        
    }

    public event Action newRootUpdate;
    public void NewRoot(bool isFetch = false) {
        Cost_NutriJewel = costNewRoot[Mathf.Clamp(tree.Roots.Count-3, 0, 7)][0];
        Cost_SlimeStone = costNewRoot[Mathf.Clamp(tree.Roots.Count-3, 0, 7)][1];
        Cost_ManaWater = costNewRoot[Mathf.Clamp(tree.Roots.Count-3, 0, 7)][2];
        Cost_Meteorite = costNewRoot[Mathf.Clamp(tree.Roots.Count-3, 0, 7)][3];
        Cost_DarkDust = costNewRoot[Mathf.Clamp(tree.Roots.Count-3, 0, 7)][4];
        if(isFetch) return;
        if (tree.Roots.Count < 8) {
            Cost_NutriJewel = costNewRoot[Mathf.Clamp(tree.Roots.Count-3, 0, 7)][0];
            Cost_SlimeStone = costNewRoot[Mathf.Clamp(tree.Roots.Count-3, 0, 7)][1];
            Cost_ManaWater = costNewRoot[Mathf.Clamp(tree.Roots.Count-3, 0, 7)][2];
            Cost_Meteorite = costNewRoot[Mathf.Clamp(tree.Roots.Count-3, 0, 7)][3];
            Cost_DarkDust = costNewRoot[Mathf.Clamp(tree.Roots.Count-3, 0, 7)][4];
        }

        if (tree.Roots.Count < 8 && EnoughResources(Cost_NutriJewel, Cost_SlimeStone, Cost_ManaWater, Cost_Meteorite, Cost_DarkDust)) { //Cost fulfilled
            tree.resAmount[GameResourcesType.Nutri_Jewel] -= Cost_NutriJewel;
            tree.resAmount[GameResourcesType.Slime_Stone] -= Cost_SlimeStone;
            tree.resAmount[GameResourcesType.Mana_Water] -= Cost_ManaWater;
            tree.resAmount[GameResourcesType.Meteorite] -= Cost_Meteorite;
            tree.resAmount[GameResourcesType.Dark_Dust] -= Cost_DarkDust;
            tree.SpawnRoot();
        }
        
        Cost_NutriJewel = costNewRoot[Mathf.Clamp(tree.Roots.Count-3, 0, 7)][0];
        Cost_SlimeStone = costNewRoot[Mathf.Clamp(tree.Roots.Count-3, 0, 7)][1];
        Cost_ManaWater = costNewRoot[Mathf.Clamp(tree.Roots.Count-3, 0, 7)][2];
        Cost_Meteorite = costNewRoot[Mathf.Clamp(tree.Roots.Count-3, 0, 7)][3];
        Cost_DarkDust = costNewRoot[Mathf.Clamp(tree.Roots.Count-3, 0, 7)][4];
        
        newRootUpdate?.Invoke();
        
    }

    public event Action<int> newFairyUpdate;
    public void NewFairy(bool isFetch = false) {
        Cost_NutriJewel = costFairy[fairyLevel][0];
        Cost_SlimeStone = costFairy[fairyLevel][1];
        Cost_ManaWater = costFairy[fairyLevel][2];
        Cost_Meteorite = costFairy[fairyLevel][3];
        Cost_DarkDust = costFairy[fairyLevel][4];
        if (isFetch) return;
        if (fairyLevel < 12) {
            Cost_NutriJewel = costFairy[fairyLevel][0];
            Cost_SlimeStone = costFairy[fairyLevel][1];
            Cost_ManaWater = costFairy[fairyLevel][2];
            Cost_Meteorite = costFairy[fairyLevel][3];
            Cost_DarkDust = costFairy[fairyLevel][4];
        }
        if (fairyLevel < 12 && EnoughResources(Cost_NutriJewel, Cost_SlimeStone, Cost_ManaWater, Cost_Meteorite, Cost_DarkDust)) { //Cost fulfilled
            tree.resAmount[GameResourcesType.Nutri_Jewel] -= Cost_NutriJewel;
            tree.resAmount[GameResourcesType.Slime_Stone] -= Cost_SlimeStone;
            tree.resAmount[GameResourcesType.Mana_Water] -= Cost_ManaWater;
            tree.resAmount[GameResourcesType.Meteorite] -= Cost_Meteorite;
            tree.resAmount[GameResourcesType.Dark_Dust] -= Cost_DarkDust;
            Fairy.GetComponent<FairyProperties>().CD = 0.5f;
            Fairy.GetComponent<FairyProperties>().attack = fairyAttackValue[fairyLevel];
            Fairy.GetComponent<FairyProperties>().timeSpawned = GameManager.Instance.gameTime;
            Spawn(Fairy, Vector3.zero, Vector3.zero);
                
            foreach (FairyProperties f in FindObjectsOfType<FairyProperties>()) {
                f.attack = fairyAttackValue[fairyLevel];
            }

            Fairy.GetComponent<FairyProperties>().attack = fairyAttackValue[fairyLevel];

            fairyLevel += 1;
        }
        
        Cost_NutriJewel = costFairy[fairyLevel][0];
        Cost_SlimeStone = costFairy[fairyLevel][1];
        Cost_ManaWater = costFairy[fairyLevel][2];
        Cost_Meteorite = costFairy[fairyLevel][3];
        Cost_DarkDust = costFairy[fairyLevel][4];
        
        newFairyUpdate?.Invoke(fairyLevel);
        
    }

    public event Action<int> newLightSpiritLevelUpdate;
    public void NewLightSpirit(bool isFetch = false) {
        Cost_NutriJewel = costLightSpirit[lightSpiritLevel][0];
        Cost_SlimeStone = costLightSpirit[lightSpiritLevel][1];
        Cost_ManaWater = costLightSpirit[lightSpiritLevel][2];
        Cost_Meteorite = costLightSpirit[lightSpiritLevel][3];
        Cost_DarkDust = costLightSpirit[lightSpiritLevel][4];
        if (isFetch) return;
        if (lightSpiritLevel < 12) {
            Cost_NutriJewel = costLightSpirit[lightSpiritLevel][0];
            Cost_SlimeStone = costLightSpirit[lightSpiritLevel][1];
            Cost_ManaWater = costLightSpirit[lightSpiritLevel][2];
            Cost_Meteorite = costLightSpirit[lightSpiritLevel][3];
            Cost_DarkDust = costLightSpirit[lightSpiritLevel][4];
        }
        if (lightSpiritLevel < 12 && EnoughResources(Cost_NutriJewel, Cost_SlimeStone, Cost_ManaWater, Cost_Meteorite, Cost_DarkDust)) { //Cost fulfilled
            tree.resAmount[GameResourcesType.Nutri_Jewel] -= Cost_NutriJewel;
            tree.resAmount[GameResourcesType.Slime_Stone] -= Cost_SlimeStone;
            tree.resAmount[GameResourcesType.Mana_Water] -= Cost_ManaWater;
            tree.resAmount[GameResourcesType.Meteorite] -= Cost_Meteorite;
            tree.resAmount[GameResourcesType.Dark_Dust] -= Cost_DarkDust;
            LightSpirit.GetComponent<LightSpiritProperties>().CD = 0.5f;
            LightSpirit.GetComponent<LightSpiritProperties>().HealAmount = lightSpiritHealingValue[lightSpiritLevel];
            LightSpirit.GetComponent<LightSpiritProperties>().timeSpawned = GameManager.Instance.gameTime;
            Spawn(LightSpirit, Vector3.zero, Vector3.zero);
                
            foreach (LightSpiritProperties s in FindObjectsOfType<LightSpiritProperties>()) {
                s.HealAmount = lightSpiritHealingValue[lightSpiritLevel];
            }
                
            lightSpiritLevel += 1;
        }
        
        
        Cost_NutriJewel = costLightSpirit[lightSpiritLevel][0];
        Cost_SlimeStone = costLightSpirit[lightSpiritLevel][1];
        Cost_ManaWater = costLightSpirit[lightSpiritLevel][2];
        Cost_Meteorite = costLightSpirit[lightSpiritLevel][3];
        Cost_DarkDust = costLightSpirit[lightSpiritLevel][4];
        newLightSpiritLevelUpdate?.Invoke(lightSpiritLevel);
        
    }

    public event Action<int> spikeLevelUpdate;
    public void SpikeLevelUp(bool isFetch = false) {
        Cost_NutriJewel = costLightSpirit[rootSpikesLevel][0];
        Cost_SlimeStone = costLightSpirit[rootSpikesLevel][1];
        Cost_ManaWater = costLightSpirit[rootSpikesLevel][2];
        Cost_Meteorite = costLightSpirit[rootSpikesLevel][3];
        Cost_DarkDust = costLightSpirit[rootSpikesLevel][4];
        if (isFetch) return;
        if (rootSpikesLevel < 12) {
            Cost_NutriJewel = costLightSpirit[rootSpikesLevel][0];
            Cost_SlimeStone = costLightSpirit[rootSpikesLevel][1];
            Cost_ManaWater = costLightSpirit[rootSpikesLevel][2];
            Cost_Meteorite = costLightSpirit[rootSpikesLevel][3];
            Cost_DarkDust = costLightSpirit[rootSpikesLevel][4];
        }
        if (rootSpikesLevel < 12 && EnoughResources(Cost_NutriJewel, Cost_SlimeStone, Cost_ManaWater, Cost_Meteorite, Cost_DarkDust)) { //Cost fulfilled
            tree.resAmount[GameResourcesType.Nutri_Jewel] -= Cost_NutriJewel;
            tree.resAmount[GameResourcesType.Slime_Stone] -= Cost_SlimeStone;
            tree.resAmount[GameResourcesType.Mana_Water] -= Cost_ManaWater;
            tree.resAmount[GameResourcesType.Meteorite] -= Cost_Meteorite;
            tree.resAmount[GameResourcesType.Dark_Dust] -= Cost_DarkDust;

            FindObjectOfType<SkillSystem>().rootSpikes_damage = rootSpikesValue_Attack[rootSpikesLevel];
            FindObjectOfType<SkillSystem>().rootSpikes_size = rootSpikesValue_Size[rootSpikesLevel];
                
            rootSpikesLevel += 1;
        }
        
        Cost_NutriJewel = costLightSpirit[rootSpikesLevel][0];
        Cost_SlimeStone = costLightSpirit[rootSpikesLevel][1];
        Cost_ManaWater = costLightSpirit[rootSpikesLevel][2];
        Cost_Meteorite = costLightSpirit[rootSpikesLevel][3];
        Cost_DarkDust = costLightSpirit[rootSpikesLevel][4];
        
        spikeLevelUpdate?.Invoke(rootSpikesLevel);
        
    }

    public event Action<int> bombBulbLevelUpdate;
    public void BombBulbLevelUp(bool isFetch = false) {
        Cost_NutriJewel = costLightSpirit[bombBulbLevel][0];
        Cost_SlimeStone = costLightSpirit[bombBulbLevel][1];
        Cost_ManaWater = costLightSpirit[bombBulbLevel][2];
        Cost_Meteorite = costLightSpirit[bombBulbLevel][3];
        Cost_DarkDust = costLightSpirit[bombBulbLevel][4];

        if (isFetch) return;
        
        if (bombBulbLevel < 12) {
            Cost_NutriJewel = costLightSpirit[bombBulbLevel][0];
            Cost_SlimeStone = costLightSpirit[bombBulbLevel][1];
            Cost_ManaWater = costLightSpirit[bombBulbLevel][2];
            Cost_Meteorite = costLightSpirit[bombBulbLevel][3];
            Cost_DarkDust = costLightSpirit[bombBulbLevel][4];
        }
        Debug.Log("6 is pressed");
        if (bombBulbLevel < 12 && EnoughResources(Cost_NutriJewel, Cost_SlimeStone, Cost_ManaWater, Cost_Meteorite, Cost_DarkDust)) { //Cost fulfilled
            tree.resAmount[GameResourcesType.Nutri_Jewel] -= Cost_NutriJewel;
            tree.resAmount[GameResourcesType.Slime_Stone] -= Cost_SlimeStone;
            tree.resAmount[GameResourcesType.Mana_Water] -= Cost_ManaWater;
            tree.resAmount[GameResourcesType.Meteorite] -= Cost_Meteorite;
            tree.resAmount[GameResourcesType.Dark_Dust] -= Cost_DarkDust;

            FindObjectOfType<SkillSystem>().bombBulb_damage = bombBulbValue_Attack[bombBulbLevel];
            FindObjectOfType<SkillSystem>().bombBulb_size = bombBulbValue_Size[bombBulbLevel];
                
            bombBulbLevel += 1;
        }
        
        Cost_NutriJewel = costLightSpirit[bombBulbLevel][0];
        Cost_SlimeStone = costLightSpirit[bombBulbLevel][1];
        Cost_ManaWater = costLightSpirit[bombBulbLevel][2];
        Cost_Meteorite = costLightSpirit[bombBulbLevel][3];
        Cost_DarkDust = costLightSpirit[bombBulbLevel][4];
        
        bombBulbLevelUpdate?.Invoke(bombBulbLevel);
        
    }

    private bool EnoughResources(float NJ, float SS, float MW, float MoB, float DD) {
        //Comment these "false"s if need testing
        
        if (tree.resAmount[GameResourcesType.Nutri_Jewel] < NJ)
            return false;
        if (tree.resAmount[GameResourcesType.Slime_Stone] < SS)
            return false;
        if (tree.resAmount[GameResourcesType.Mana_Water] < MW)
            return false;
        if (tree.resAmount[GameResourcesType.Meteorite] < MoB)
            return false;
        if (tree.resAmount[GameResourcesType.Dark_Dust] < DD)
            return false;
        
        return true;
    }
}
