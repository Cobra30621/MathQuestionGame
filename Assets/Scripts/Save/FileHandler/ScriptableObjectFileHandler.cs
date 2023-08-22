using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DataPersistence
{
    /// <summary>
    /// 目的：將 Scriptable Object 進行存檔
    /// Scriptable Object 無法直接序列化成 Json，因此改儲存該 Scriptable Object 的 guid
    /// </summary>
    public class ScriptableObjectFileHandler : SerializedMonoBehaviour
    {
        [InlineEditor()]
        public ScriptableObjectReferenceCache dataReference;

        /// <summary>
        /// 取得 ScriptableObject 的 guid
        /// </summary>
        /// <param name="data"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public string DataToGuid<T>(T data)
        {
            if (data == null)
            {
                Debug.LogError($"輸入的 ScriptableObject 為 null");
                return "";
            } 
            
            if (!dataReference.CanReference(data, out string guid))
            {
                Debug.LogError($"無法取得 ScriptableObject {data} 的 guid");
            }
            
            return guid;
        }
        
        /// <summary>
        /// 取得 ScriptableObject 的 guid
        /// </summary>
        /// <param name="dataList"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [Button]
        public List<string> DataToGuid<T>(List<T> dataList)
        {
            var guids = new List<string>();
            foreach (var data in dataList)
            {
                if (data == null)
                {
                    Debug.LogError($"輸入的 ScriptableObject 為 null");
                }
                else
                {
                    if (dataReference.CanReference(data, out string guid))
                    {
                        guids.Add(guid);
                    }
                    else
                    {
                        Debug.LogError($"無法取得 ScriptableObject {data} 的 guid");
                    }
                }
            }

            return guids;
        }

        /// <summary>
        /// 透過 guid，取得 ScriptableObject
        /// </summary>
        /// <param name="guid"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GuidToData<T>(string guid)
        {
            T data = default(T);
            if (dataReference.TryResolveReference(guid, out object output))
            {
                data = (T)output;
            }
            else
            {
                Debug.LogError($"無法透過 guid: {guid} ，取得 Scriptable Object");
            }
            return data;
        }
        
        /// <summary>
        /// 透過 guid，取得 ScriptableObject
        /// </summary>
        /// <param name="guids"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [Button]
        public List<T> GuidToData<T>(List<string> guids)
        {
            var dataList = new List<T>();
            foreach (var guid in guids)
            {
                if (dataReference.TryResolveReference(guid, out object output))
                {
                    T data = (T)output;
                    dataList.Add(data);
                }
                else
                {
                    Debug.LogError($"無法透過 guid: {guid} ，取得 Scriptable Object");
                }
            }

            return dataList;
        }
    }
}