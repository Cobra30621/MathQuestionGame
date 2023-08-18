using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DataPersistence
{
    public class ScriptableObjectFileHandler : SerializedMonoBehaviour
    {
        [InlineEditor()]
        public ScriptableObjectReferenceCache dataReference;

        public string DataToGuid<T>(T data)
        {
            if (dataReference.CanReference(data, out string guid))
            {
            }
            else
            {
                Debug.Log($"{nameof(T)} 無法轉換");
            }
            
            return guid;
        }
        
        [Button]
        public List<string> DataToGuid<T>(List<T> dataList)
        {
            var guids = new List<string>();
            foreach (var data in dataList)
            {
                if (dataReference.CanReference(data, out string guid))
                {
                    guids.Add(guid);
                }
                else
                {
                    Debug.Log($"{nameof(T)} 無法轉換");
                }
            }

            return guids;
        }

        public T GuidToData<T>(string guid)
        {
            T data = default(T);
            if (dataReference.TryResolveReference(guid, out object output))
            {
                data = (T)output;
            }
            return data;
        }
        
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
            }

            return dataList;
        }
    }
}