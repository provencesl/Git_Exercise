/**
 *	Title: "SOAP" UI框架
 *
 *  Description:
 *  
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test
{
    public class TestUnityJson2 : MonoBehaviour {
    
    	void Start () {
            TextAsset TaObj = Resources.Load<TextAsset>("People");
            PersonInfo personInfo = JsonUtility.FromJson<PersonInfo>(TaObj.text);
            Debug.Log(personInfo.People);
            //foreach(People people in personInfo.Peoples)
            //{
            //    Debug.Log(string.Format("name={0},Age={1}", people.Name, people.Age));
            //}
    	}
    	
    }
}
