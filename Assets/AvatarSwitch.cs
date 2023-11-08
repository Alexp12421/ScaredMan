using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarSwitch : MonoBehaviour
{
    //Avatars
    public GameObject MadMan;
    public GameObject ScaredMan;
    bool ScaredManIsDead = false;

    // Start is called before the first frame update
    void Start()
    {
        MadMan.gameObject.SetActive(false);
        ScaredMan.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (ScaredMan.gameObject.GetComponent<PlayerCombat>().getCurrentHp() <= 0 && !ScaredManIsDead)
        {
            ScaredManIsDead = true;
            
            MadMan.gameObject.SetActive(true);
            ScaredMan.gameObject.SetActive(false);
            MadMan.gameObject.GetComponent<MovementController>().setPlayerPosition(ScaredMan.gameObject.GetComponent<MovementController>().getPlayerTransform().position);
        }
    }
}
