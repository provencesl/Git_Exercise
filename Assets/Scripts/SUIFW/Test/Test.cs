/**
 *	Title: "SOAP" UI框架
 *
 *  Description:
 *  
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SUIFW
{
    public class Test : MonoBehaviour {

        private Stack<string> _StaArray = new Stack<string>();
    	void Start () {
            Test2();

        }
    	
        private void Test2()
        {
            _StaArray.Push("1");
            _StaArray.Push("2");
            _StaArray.Push("3");
            IEnumerator<string> ie = _StaArray.GetEnumerator();
            while (ie.MoveNext())
            {
                Debug.Log(ie.Current);
            }
        }

    }
}
