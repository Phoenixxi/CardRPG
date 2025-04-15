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

    
    public Button victoryButton;
    public Button lossButton;

    // VFX
    public GameObject attackVFX_1;
    Vector3 startVFXLocation = new Vector3(15.6000004f,3.74000001f,-15.71f);

    //  Health bars
    public EnemyHealthBar enemyHealthBar;
     
    // Enemy health text display
    public Text enemyHealthTotal;
    public Text enemyHealthCurrent;

    public float decDmgPercent = 1f;
    private float thornsPercent = 0f;
    private float DOTamount = 0f;
    public bool asleep = false;

    void Start()
    {
        enemyHealthBar.SetMaxHealth(currentHealth);
        enemyHealthTotal.text = currentHealth.ToString();
        enemyHealthCurrent.text = currentHealth.ToString();
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
            GameObject vfxInstance = Instantiate(attackVFX_1, startVFXLocation, Quaternion.identity);

            ParticleSystem[] particleSystems = vfxInstance.GetComponentsInChildren<ParticleSystem>();

            // Play all particle systems
            foreach (ParticleSystem ps in particleSystems)
            {
                ps.Play();
            }
            // Destroy the VFX after it finishes playing, also play for 5 seconds
            Destroy(vfxInstance, 5f); 

            // ENEMY DAMAGE AMOUNT
            StartCoroutine(ApplyDamage());

            Invoke("EnemyTurnEnd", 3f);
        }
    }

    private IEnumerator ApplyDamage()
    {
        yield return new WaitForSeconds(2f);
        Random random = new System.Random();
        int dmgTemp = random.Next(5,16);
        float dmg = (float)dmgTemp * decDmgPercent;
        
        attackManager.DecreaseTeamHealth(dmg);

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