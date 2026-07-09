using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Diagnostics;


public class Lamp : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Light lamp;
    private int levelOfBattery=2;
    private int countActiveAttack=0;
    void Start()
    {
        lamp = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.lKey.wasPressedThisFrame && levelOfBattery>0)
        {
            lamp.enabled = !lamp.enabled;
            countActiveAttack++;
        }
        lightAtack();
        batteryIsDead();

    }
    
    public void batteryIsDead()
    {
        if(levelOfBattery==0)
        {
            lamp.enabled = false;
        }
    }

    public void lightAtack()
    {
        Debug.Log("CounAttack "+ countActiveAttack);
        if (countActiveAttack > 2 && levelOfBattery>=2)
        {
            Debug.Log("Make Attack "+ countActiveAttack);
            countActiveAttack=0;  
            StartCoroutine(increaseLightForAttack(7.0f));
        }

    }
    IEnumerator increaseLightForAttack(float delay)
    {
        lamp.intensity = 5000;
        yield return new WaitForSeconds(delay);
        lamp.intensity = 100;
        levelOfBattery -=2; 
    }
    public void chargeLamp(int value)
    {
        levelOfBattery+=value;
        Debug.Log("levelOfBattery "+levelOfBattery);
    }
}
