using System;
using UnityEngine;

namespace VersionControl
{
    [Serializable]
    public class GameVersion
    {
        /// <summary>
        /// 這個版本的名稱
        /// </summary>
        public string VersionName;
        /// <summary>
        /// 主要的大版本
        /// </summary>
        public int MainVersionNum;
        /// <summary>
        /// 新增一些功能的小版本
        /// </summary>
        public int SubVersionNum;
        /// <summary>
        /// 修正一些 Bug 的版本
        /// </summary>
        public int FixVersionNum;
        
        /// <summary>
        /// 更新內容
        /// </summary>
        [TextArea]
        public string UpdateMemo;

        /// <summary>
        /// 用來給遊戲顯示用的，必需要Memo
        /// </summary>
        /// <returns></returns>
        public string GetVersionString()
        {
            if (UpdateMemo != "")
            {
                Debug.LogWarning("請確認定義UpdateMemo否則無法獲得版號");
            }
            if (UpdateMemo != null)
            {
                return VersionName + " " + MainVersionNum + "." + SubVersionNum + "." + FixVersionNum + " - " + UpdateMemo.ToString();
            }
            else
            {
                return null;
            }
        }

        // 這個是用來給存檔系統做判斷和Log用的，不用寫memo也沒關係
        public string GetVersionNum()
        {
            return (MainVersionNum + "." + SubVersionNum + "." + FixVersionNum);
        }
    }
}