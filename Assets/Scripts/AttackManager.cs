using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CardNamespace;

public class AttackManager : MonoBehaviour
{
    // Managers
    public List<GameObject> cardsList = new List<GameObject>();
    public EnemyManager enemyManager;
    public HandManager handManager;

    // Health bars
    public HealthBar healthBar;
    
    // Health bar text
    public float currentHealth;
    public Text teamHealthTotal;
    public Text teamHealthCurrent;
    private int BellaSpecialTurns = 0;
    [SerializeField] private Transform VFXProjectileSpawnTop;
    [SerializeField] private Transform VFXProjectileSpawnMiddle;
    [SerializeField] private Transform VFXProjectileSpawnBottom;
    [SerializeField] private Transform VFXHealSpawnTop;
    [SerializeField] private Transform VFXHealSpawnMiddle;
    [SerializeField] private Transform VFXHealSpawnBottom;
    [SerializeField] private Transform VFXRainfallSpawn;
    [SerializeField] private Transform VFXUnderEnemySpawn;
    [SerializeField] private Transform VFXImpactSpawn;

    void Start()
    {
        healthBar.SetMaxHealth(currentHealth);
        teamHealthTotal.text = currentHealth.ToString();
        teamHealthCurrent.text = currentHealth.ToString();
    }

    public void DecreaseTeamHealth(float health)
    {
        // If player dies
        if(currentHealth - health <= 0){
            teamHealthCurrent.text = "0";
            healthBar.DecreaseTeamHealth(health);
        }
        else
        {
            currentHealth -= health;
            teamHealthCurrent.text = currentHealth.ToString();
            healthBar.DecreaseTeamHealth(health);
        }
    }

    public void IncreaseTeamHealth(float health)
    {
        healthBar.IncreaseTeamHealth(health);
        if(currentHealth + health >= healthBar.slider.maxValue)
        {
            currentHealth = healthBar.slider.maxValue;
            teamHealthCurrent.text = currentHealth.ToString();
        }
        else
        {
            currentHealth += health;
            teamHealthCurrent.text = currentHealth.ToString();
        }

    }
    
    public void AttackStart()
    {
        if(BellaSpecialTurns > 0)
        {
            BellaSpecialTurns--;
            // CHANGE 5 IF BELLA'S HEALING AMOUNT CHANGES FOR SPECIAL ABILITY
            IncreaseTeamHealth(5);
        }
        Vector3 startVFXLocation = new Vector3(6.04000006f, 2.529999995f ,-17.816000015f);

        // see if team buffs are being used
        float teamMultipler = 1f;
        foreach(GameObject card in cardsList){
            CardDisplay cardDisplay = card.GetComponent<CardDisplay>();
            if(cardDisplay.cardData.cardType.ToString() == "TeamDmgMultiplier" || cardDisplay.cardData.cardType.ToString() == "BellaSpecial")
                teamMultipler = cardDisplay.cardData.Team_dmgMultiplier;
        }


        // incoming dmg reducer sends signal to enemy manager, handled over there during enemy turn (field?)
        foreach(GameObject card in cardsList){

            CardDisplay cardDisplay = card.GetComponent<CardDisplay>();

            // use cardDisplay.character to get character position so VFX can play from the correct place
            // Get type of VFX
            if(cardDisplay.cardData.vfxProjectile != null)
                Projectile(cardDisplay);

            else if(cardDisplay.cardData.vfxHeal != null)
                Healing(cardDisplay);

            else if(cardDisplay.cardData.vfxBuff != null)
                Buffing(cardDisplay);
            
            else if(cardDisplay.cardData.vfxRainFall != null)
                RainFall(cardDisplay);

            else if(cardDisplay.cardData.vfxUnderEnemy != null)
                UnderEnemyAttack(cardDisplay);


            Card data = cardDisplay.cardData;
            string currentCardType = data.cardType.ToString();
            //Delay health bar changes
            StartCoroutine(ApplyAbility(currentCardType, data, teamMultipler));
            
        }
        EmptyCards();
        enemyManager.EnemyTurnStart();
    }

    private void Projectile(CardDisplay cardDisplay)
    {
        GameObject vfxInstance;

        switch(cardDisplay.cardData.CharacterPosition)
        {
            case "top":
                vfxInstance = Instantiate(cardDisplay.cardData.vfxProjectile, VFXProjectileSpawnTop.position, Quaternion.identity);
                vfxInstance.transform.localScale = VFXProjectileSpawnTop.localScale;
                vfxInstance.transform.localRotation = VFXProjectileSpawnTop.localRotation;
                ParticleSystem[] particleSystemsTop = vfxInstance.GetComponentsInChildren<ParticleSystem>();
                
                foreach (ParticleSystem ps in particleSystemsTop)
                    ps.Play();
        
                Destroy(vfxInstance, 4f);

                if(cardDisplay.cardData.vfxImpact != null)
                StartCoroutine(PlayImpact(cardDisplay.cardData.vfxImpact, 1.8f));
                break;

            case "middle":
                vfxInstance = Instantiate(cardDisplay.cardData.vfxProjectile, VFXProjectileSpawnMiddle.position, VFXProjectileSpawnMiddle.localRotation);
                vfxInstance.transform.localScale = VFXProjectileSpawnMiddle.localScale;
                vfxInstance.transform.localRotation = VFXProjectileSpawnMiddle.localRotation;
                ParticleSystem[] particleSystemsMid = vfxInstance.GetComponentsInChildren<ParticleSystem>();

                foreach (ParticleSystem ps in particleSystemsMid){
                    ps.Play();
                }
                
                Destroy(vfxInstance, 3f);

                if(cardDisplay.cardData.vfxImpact != null)
                StartCoroutine(PlayImpact(cardDisplay.cardData.vfxImpact, 1.7f));
                break;

            case "bottom":
                vfxInstance = Instantiate(cardDisplay.cardData.vfxProjectile, VFXProjectileSpawnBottom.position, Quaternion.identity);
                vfxInstance.transform.localScale = VFXProjectileSpawnBottom.localScale;
                vfxInstance.transform.localRotation = VFXProjectileSpawnBottom.localRotation;
                ParticleSystem[] particleSystemsBottom = vfxInstance.GetComponentsInChildren<ParticleSystem>();
                
                foreach (ParticleSystem ps in particleSystemsBottom)
                    ps.Play();
        
                Destroy(vfxInstance, 1.9f);

                if(cardDisplay.cardData.vfxImpact != null)
                StartCoroutine(PlayImpact(cardDisplay.cardData.vfxImpact, 1.8f));
                break;
        }
    }

    private void Healing(CardDisplay cardDisplay)
    {
        GameObject vfxInstance;
        Transform vfxSpawn = VFXHealSpawnTop;
        for(int i = 0; i < 3; i++)
        {
            if(i == 1)
                vfxSpawn = VFXHealSpawnMiddle;
            else if(i == 2)
                vfxSpawn = VFXHealSpawnBottom;

            vfxInstance = Instantiate(cardDisplay.cardData.vfxHeal, vfxSpawn.position, Quaternion.identity);
            vfxInstance.transform.localScale = vfxSpawn.localScale;
            ParticleSystem[] particleSystemsTop = vfxInstance.GetComponentsInChildren<ParticleSystem>();
            
            foreach (ParticleSystem ps in particleSystemsTop)
                ps.Play();
    
            Destroy(vfxInstance, 4f);
        }
    }

    private void Buffing(CardDisplay cardDisplay)
    {
         GameObject vfxInstance;

        switch(cardDisplay.cardData.CharacterPosition)
        {
            case "top":
                vfxInstance = Instantiate(cardDisplay.cardData.vfxBuff, VFXHealSpawnTop.position, Quaternion.identity);
                vfxInstance.transform.localScale = VFXHealSpawnTop.localScale;
                ParticleSystem[] particleSystemsTop = vfxInstance.GetComponentsInChildren<ParticleSystem>();
                
                foreach (ParticleSystem ps in particleSystemsTop)
                    ps.Play();
        
                Destroy(vfxInstance, 1.9f);
                break;

            case "middle":
                vfxInstance = Instantiate(cardDisplay.cardData.vfxBuff, VFXHealSpawnMiddle.position, Quaternion.identity);
                vfxInstance.transform.localScale = VFXHealSpawnMiddle.localScale;
                ParticleSystem[] particleSystemsMid = vfxInstance.GetComponentsInChildren<ParticleSystem>();

                foreach (ParticleSystem ps in particleSystemsMid)
                    ps.Play();
                
                Destroy(vfxInstance, 1.8f);
                break;

            case "bottom":
                vfxInstance = Instantiate(cardDisplay.cardData.vfxBuff, VFXHealSpawnBottom.position, Quaternion.identity);
                vfxInstance.transform.localScale = VFXHealSpawnBottom.localScale;
                ParticleSystem[] particleSystemsBottom = vfxInstance.GetComponentsInChildren<ParticleSystem>();
                
                foreach (ParticleSystem ps in particleSystemsBottom)
                    ps.Play();
        
                Destroy(vfxInstance, 1.9f);
                break;
        }
    }

    public void RainFall(CardDisplay cardDisplay)
    {

        GameObject  vfxInstance = Instantiate(cardDisplay.cardData.vfxRainFall, VFXRainfallSpawn.position, Quaternion.identity);
        vfxInstance.transform.localScale = VFXRainfallSpawn.localScale;
        vfxInstance.transform.localRotation = VFXRainfallSpawn.localRotation;
        ParticleSystem[] particleSystemsTop = vfxInstance.GetComponentsInChildren<ParticleSystem>();
        
        foreach (ParticleSystem ps in particleSystemsTop)
            ps.Play();

        Destroy(vfxInstance, 4f);
    
    }

    public void UnderEnemyAttack(CardDisplay cardDisplay)
    {
        GameObject  vfxInstance = Instantiate(cardDisplay.cardData.vfxUnderEnemy, VFXUnderEnemySpawn.position, Quaternion.identity);
        vfxInstance.transform.localScale = VFXUnderEnemySpawn.localScale;
        vfxInstance.transform.localRotation = VFXUnderEnemySpawn.localRotation;
        ParticleSystem[] particleSystemsTop = vfxInstance.GetComponentsInChildren<ParticleSystem>();
        
        foreach (ParticleSystem ps in particleSystemsTop)
            ps.Play();

        Destroy(vfxInstance, 2f);
    }

    private IEnumerator ApplyAbility(string cardType, Card data, float teamMultipler)
    {
        yield return new WaitForSeconds(2f);
        switch(cardType)
            {
                case "Damage":
                    enemyManager.DecreaseEnemyHealth(data.Damage * teamMultipler);
                    break;

                case "TeamHeal":
                    float health = data.Heal;
                    IncreaseTeamHealth(health);
                    break;

                case "DecEnemyDmg":
                    break;
                case "SingleAtkAdder":
                    enemyManager.DecreaseEnemyHealth(data.Single_atkAdder);
                    break;

                case "Shield":
                    enemyManager.ToggleSheildStatus(true);
                    break;

                case "DmgOverTime":
                    break;

                case "Thorns":
                    enemyManager.DecreaseEnemyHealth(data.Damage);
                    enemyManager.ToggleThornsStatus(true);
                    break;

                case "DiceManipulation":
                    handManager.DiceManipulationActive(data.DiceManipulationAmount);
                    break;

                case "KingFireBlastSpecial":
                    if(enemyManager.attackedLastTurn)
                        enemyManager.DecreaseEnemyHealth(data.Damage + 2);
                    else   
                        enemyManager.DecreaseEnemyHealth(data.Damage);
                    break;
                
                case "BellaSpecial":
                        BellaSpecialTurns = 2;
                        IncreaseTeamHealth(data.Heal);
                    break;
            }
    }


    private IEnumerator PlayImpact(GameObject vfxImpactParameter, float time)
    {
        yield return new WaitForSeconds(time);
        GameObject vfxImpact = Instantiate(vfxImpactParameter, VFXImpactSpawn.position, Quaternion.identity);
        vfxImpact.transform.localScale = VFXImpactSpawn.localScale;
        ParticleSystem[] particleSystemImpact = vfxImpact.GetComponentsInChildren<ParticleSystem>();

        foreach (ParticleSystem psi in particleSystemImpact)
        {
            psi.Play();
        }
        Destroy(vfxImpact, 2f); 
    
    }


    public void EmptyCards(){
        cardsList = new List<GameObject>();
    }
}

