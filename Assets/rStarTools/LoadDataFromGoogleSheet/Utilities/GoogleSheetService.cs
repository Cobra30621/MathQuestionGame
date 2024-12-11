#region

using System;
using ThirdParty.Utilities;

#endregion

namespace Utilities
{
    public static class GoogleSheetService
    {
    #region Public Variables

        public static void LoadDataArray<T>(string url , Action<T[]> complete)
        {
            CustomerWebRequest.Complete += delegate(string jsonText)
            {
                try
                {
                    var result = JsonHelper.FromJson<T>(jsonText , true);
                    complete.Invoke(result);
                }
                catch (Exception)
                {
                    CustomerWebRequest.ClearAction();
                    throw;
                }
            };
            CustomerWebRequest.Request(url);
        }

    #endregion
    }
}