/**
 *	Title: "SOAP" UI框架
 *  语言国际化
 *  Description:使得发布的游戏，根据不同的国家发布不同的语言版本
 *  
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SUIFW
{
    public class LauguageMgr {

        public static LauguageMgr _Instance;

        //语言翻译的缓存集合
        public Dictionary<string, string> _DicLauguageCache;

        public LauguageMgr()
        {
            _DicLauguageCache = new Dictionary<string, string>();
            //初始化语言缓存集合
            InitLauguageCache();
        }
        /// <summary>
        /// 得到本类实例
        /// </summary>
        /// <returns>The instance.</returns>
        public static LauguageMgr GetInstance()
        {
            if(_Instance == null)
            {
                _Instance = new LauguageMgr();
            }
            return _Instance;
        }
        /// <summary>
        /// 得到显示文本
        /// </summary>
        /// <returns>The text.</returns>
        /// <param name="languageID">语言ID</param>
        public string ShowText(string languageID)
        {

            string strQueryResult = string.Empty;
            if (string.IsNullOrEmpty(languageID)) return null;
            if (_DicLauguageCache != null && _DicLauguageCache.Count >= 1)
            {
                _DicLauguageCache.TryGetValue(languageID, out strQueryResult);
                if (!string.IsNullOrEmpty(strQueryResult))
                {
                    return strQueryResult;
                }

                Debug.Log(GetType() + "ShowText() query is null languageID is " + languageID);
            }
            return null;
        }

        //初始化语言缓存集合
        private void InitLauguageCache()
        {
            IConfigManager config = new ConfigManagerByJson(SysDefine.SYS_LANGUAGE_JSON_CONFIG);
            if(config != null)
            {
                _DicLauguageCache = config.AppSetting;
            }
        }
    }
}
