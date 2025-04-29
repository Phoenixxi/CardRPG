using System.Collections;
using System.Collections.Generic;
using Random = System.Random;
using UnityEngine;
using UnityEngine.UI;
using CardNamespace;

public class EnemyManager : MonoBehaviour
{
    // Managers
    private List<CardClickHandler> cardClickHandler = new List<CardClickHandler>();
    private HandManager handManager;
    public DeckManager deckManager;
    public bool EnemyTurn = false;
    public AttackManager attackManager;
    public float currentHealth;

    private bool shieldStatus = false;
    private bool thornsStatus = false;
    private bool DOTstatus = false;
    private int thornsCount = 0;
    private int DOTcount = 0;
    public bool attackedLastTurn = false;


    public int worldID;
    public bool isBossBattle;
    
    public Button victoryButton;

    // VFX
    public GameObject attackVFX_1;
    public GameObject PsychBarrageVFX;
    public GameObject CelestialPillarVFX;
    Vector3 startVFXLocation = new Vector3(15.6000004f,8.74000001f,-15.71f);

    //  Health bars
    public EnemyHealthBar enemyHealthBar;
    private HealthBar teamHealthBar;
     
    // Enemy health text display
    public Text enemyHealthTotal;
    public Text enemyHealthCurrent;

    public float decDmgPercent = 1f;
    private float thornsPercent = 0f;
    private float DOTamount = 0f;
    public bool asleep = false;

    public NodeController activeNode;

    void Start()
    {
        activeNode = MapManager.Instance.nodes[0].sendCurrentNode();
        worldID = activeNode.thisWorld;
        isBossBattle = activeNode.isBossNode;

        if(worldID == 0 && !isBossBattle)
        {   
            currentHealth = 50;
        }
        else if(worldID == 0 && isBossBattle)
        {
            currentHealth = 55;
        }
        else if(worldID == 1 && !isBossBattle)
        {
            currentHealth = 70;
        }
        else if(worldID == 1 && isBossBattle)
        {
            currentHealth = 75;
        }
        else if(worldID == 2 && !isBossBattle)
        {
            currentHealth = 80;
        }
        else if(worldID == 2 && isBossBattle)
        {
            currentHealth = 100;
        }
        enemyHealthBar.SetMaxHealth(currentHealth);
        enemyHealthTotal.text = currentHealth.ToString();
        enemyHealthCurrent.text = currentHealth.ToString();
        teamHealthBar = FindObjectOfType<HealthBar>();

       
    }

    public void DecreaseEnemyHealth(float health)
    {
        // If enemy dies
        if(currentHealth - health <= 0){
            enemyHealthCurrent.text = "0";
            enemyHealthBar.DecreaseEnemyHealth(health);
        }
        else
        {
            currentHealth -= health;
            enemyHealthCurrent.text = currentHealth.ToString();
            enemyHealthBar.DecreaseEnemyHealth(health);
        }
    }

    private void IncreaseEnemyHealth(float health)
    {
        float maxHealth = enemyHealthBar.GetEnemyMaxHealth();
        if(currentHealth + health >= maxHealth)
        {
            enemyHealthCurrent.text = maxHealth.ToString();
            enemyHealthBar.IncreaseEnemyHealth(health);
        }
        else
        {
            currentHealth += health;
            enemyHealthCurrent.text = currentHealth.ToString();
            enemyHealthBar.IncreaseEnemyHealth(health);
        }
    }

    public void SviurHealthDecrease()
    {
        // If enemy's health is greater than 60%, set it to 60%
        float maxHealth = enemyHealthBar.GetEnemyMaxHealth();
        if(currentHealth > (maxHealth * .6f))
        {
            currentHealth = maxHealth * .6f;
            enemyHealthCurrent.text = currentHealth.ToString();
            enemyHealthBar.SetEnemyHealth();
        }
            
    }

    public void SetCardClickHandler(CardClickHandler handler)
    {
        cardClickHandler.Add(handler);
    }

    public void EnemyTurnStart()
    {
        // Find the CardClickHandler in the scene if not assigned
        //cardClickHandler = FindObjectOfType<CardClickHandler>();
        
        if (cardClickHandler != null)
        {
            foreach(CardClickHandler handler in cardClickHandler){
                handler.ToggleEnemyTurn(true);
            }
        }

        // Start Enemy attack only if enemy is still alive
        if(enemyHealthBar.slider.value > 0)
            Invoke("EnemyAttack", 3f);
        
    }

    public void ToggleSheildStatus(bool status)
    {
        shieldStatus = status;
    }

    public void ToggleThornsStatus(bool status, float percent)
    {
        thornsStatus = status;
        if(status)
        {
            thornsCount = 2;
            thornsPercent += percent;
        }
            
    }

    public void ToggleDOT(bool status, float dot)
    {
        // this is flawed I know
        DOTstatus = status;
        if(status)
        {
            DOTcount = 3;
            DOTamount += dot;
        }
    }

    public void EnemyAttack()
    {
        if(shieldStatus)
        {
            Invoke("EnemyTurnEnd", 3f);
            ToggleSheildStatus(false);
            attackedLastTurn = false;
        }
        else if(asleep)
        {
            Invoke("EnemyTurnEnd", 2f);
            asleep = false;
            attackedLastTurn = false;
        }
        else
        {
            attackedLastTurn = true;
            GameObject vfxInstance;
            ParticleSystem[] particleSystems;
            if(isBossBattle && worldID == 0)
            {
                vfxInstance = Instantiate(PsychBarrageVFX, startVFXLocation, Quaternion.identity);
                particleSystems = vfxInstance.GetComponentsInChildren<ParticleSystem>();
            }
            else if(isBossBattle && worldID == 1)
            {
                 vfxInstance = Instantiate(CelestialPillarVFX, startVFXLocation, Quaternion.identity);
                particleSystems = vfxInstance.GetComponentsInChildren<ParticleSystem>();
            }
            else
            {
                // PUT BACK TO VFX1
                vfxInstance = Instantiate(attackVFX_1, startVFXLocation, Quaternion.identity);
                particleSystems = vfxInstance.GetComponentsInChildren<ParticleSystem>();
            }
            

            // Play all particle systems
            foreach (ParticleSystem ps in particleSystems)
            {
                ps.Play();
            }
            // Destroy the VFX after it finishes playing, also play for 5 seconds
            Destroy(vfxInstance, 5f); 

            // ENEMY DAMAGE AMOUNT
            if(enemyHealthBar.slider.value > 0)
                StartCoroutine(ApplyDamage());

            Invoke("EnemyTurnEnd", 3f);
        }
    }

    private IEnumerator ApplyDamage()
    {
        yield return new WaitForSeconds(2f);
        Random random = new System.Random();

        float dmg = 0;
        int rand;
        //Get damage for Sviur
        if(worldID == 0 && isBossBattle)
        {
            if(teamHealthBar.slider.value > teamHealthBar.GetTeamMaxHealth() * .6f)
                rand = random.Next(1,5);    // Get 1-4 for which ability plays
            else
                rand = random.Next(1,4);     // Get 1-3 for which ability plays (< 60%)

            switch(rand)
            {
                case 3:
                case 1:
                    dmg = 5f * decDmgPercent;
                    break;
                case 2:
                    dmg = 8f * decDmgPercent;
                    break;
                case 4:
                    float maxHealth = teamHealthBar.GetTeamMaxHealth();
                    float teamCurrentHealth = teamHealthBar.slider.value;
                    if(teamCurrentHealth > (maxHealth * .6f))
                    {
                        teamCurrentHealth = maxHealth * .6f;
                        attackManager.teamHealthCurrent.text = teamCurrentHealth.ToString();
                        attackManager.currentHealth = teamCurrentHealth;
                        teamHealthBar.SetTeamHealthSviur();
                    }
                    break;

            }
        }
        //Get damage for Estella
        else if(worldID == 1 && isBossBattle)
        {
            rand = random.Next(1,4);    // Only switch on 3 abilities
            switch(rand)
            {
                case 1:
                    IncreaseEnemyHealth(10);
                    break;
                case 2:
                    dmg = 10f * decDmgPercent;
                    break;
                case 3:
                    dmg = 8f * decDmgPercent;
                    IncreaseEnemyHealth(5);
                    break;
            }
        }
        //Get damage for Final Boss
        else if(worldID == 2 && isBossBattle)
        {
            rand = random.Next(1,3);     // switch between 2 abilities (atk and heal)
            int dmgTemp = 0;
            switch(rand)
            {
                case 1:
                    dmgTemp = random.Next(10,26);
                    dmg = (float)dmgTemp * decDmgPercent;
                    break;
                case 2:
                    dmgTemp = random.Next(1,6);
                    dmg = (float)dmgTemp * decDmgPercent;
                    IncreaseEnemyHealth(10);
                    break;
            }
        }
        //Get damage for regular enemy
        else
        {
            int dmgTemp = random.Next(4,13);
            dmg = (float)dmgTemp * decDmgPercent;
        }
        

        // Check for abilities that damage enemy
        // check for dot
        if(DOTstatus && DOTcount > 0)
        {
            dmg += DOTamount;
            DOTcount--;

            if(DOTcount == 0)
            {
                ToggleDOT(false, 0f);
                DOTamount = 0f;
            }
        }

         // check for thorns
        if(thornsStatus && thornsCount > 0)
        {   
            DecreaseEnemyHealth(dmg * thornsPercent);
            thornsCount--;

            if(thornsCount == 0){
                ToggleThornsStatus(false, 0f);
                thornsPercent = 0f;
            }
        }

        // APPLY DAMAGE TO TEAM
        attackManager.DecreaseTeamHealth(dmg);
        
        // set decrease damage percent back to 1
        decDmgPercent = 1f;
    }

    public void EnemyTurnEnd()
    {
        if (cardClickHandler != null)
        {
            foreach(CardClickHandler handler in cardClickHandler){
                // enemy turn false
                handler.ToggleEnemyTurn(false);
            }
        }
        // empty card list for next turn
        cardClickHandler = new List<CardClickHandler>();
        
        // Turn on black overlay
        handManager.ToggleBlackOverlay();
        handManager.ToggleDiceButton();
        // Draw new cards automatically
        deckManager.DrawTillFill(handManager);
        
    }

    


    public void SetHandManager(HandManager manager)
    {
        handManager = manager;
    }
}