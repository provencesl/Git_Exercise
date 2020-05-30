/**
 *	Title: "SOAP" UI框架
 *
 *  Description:
 *  
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SUIFW;

namespace DemoProject
{
    public class HeroInfoUIForm : BaseUIForm {

        private void Awake()
        {
            //定义本窗体的性质(默认数值)
            base.CurrentUIType.UIForms_Type = UIFormType.Fixed;
            base.CurrentUIType.UIForms_ShowMode = UIFormShowMode.Normal;
            base.CurrentUIType.UIForms_LuccencyType = UIFormLucencyType.Lucency;
            //事件注册
        }
    }
}
