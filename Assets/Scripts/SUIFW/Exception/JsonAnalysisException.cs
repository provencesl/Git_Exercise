/**
 *	Title: "SOAP" UI框架
 *
 *  Description:json解析异常
 *  
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SUIFW
{
    public class JsonAnalysisException : Exception {

        public JsonAnalysisException() : base()
        {

        }
        public JsonAnalysisException(string exceptionMessage) : base(exceptionMessage)
        {

        }
    }
}
