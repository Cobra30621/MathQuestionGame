using System.Collections;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Sheets
{
    /// <summary>
    /// Loads data from Google Sheets for enemies, enemy skills, and skills.
    /// </summary>
    public class SheetDataLoader : MonoBehaviour
    {
        [Required] [InlineEditor] 
        [LabelText("資料讀取器")]
        public SheetDataGetter getter;

        [LabelText("資料驗證")]
        [Required] public SheetDataValidator sheetDataValidator;
        
        /// <summary>
        /// Starts loading data from Google Sheets.
        /// </summary>
        [Button("讀取表單")]
        public void Load()
        {
            StartCoroutine(LoadCoroutine());
        }

        /// <summary>
        /// Coroutine for loading data from Google Sheets.
        /// </summary>
        /// <returns></returns>
        private IEnumerator LoadCoroutine()
        {
            // Load skill data from Google Sheets
            getter.skillData.ParseDataFromGoogleSheet();
            yield return new WaitUntil(()=>!getter.skillData.IsLoading);
            
            // Load enemy skill data from Google Sheets
            getter.enemySkillData.ParseDataFromGoogleSheet();
            yield return new WaitUntil(()=>!getter.enemySkillData.IsLoading);
            
            // Load enemy data from Google Sheets
            getter.enemyData.ParseDataFromGoogleSheet();
            yield return new WaitUntil(()=>!getter.enemyData.IsLoading);
            
            Debug.Log("Loaded data from Google Sheets");
            
            SaveAsset();

            // Validate data
            sheetDataValidator.ValidateSheet();
        }

        private void SaveAsset()
        {
            EditorUtility.SetDirty(getter.skillData);
            EditorUtility.SetDirty(getter.enemySkillData);
            EditorUtility.SetDirty(getter.enemyData);
            AssetDatabase.SaveAssets();
        }
    }
}