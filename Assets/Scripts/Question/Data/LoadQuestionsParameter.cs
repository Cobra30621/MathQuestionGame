using Question.Enum;
using UnityEngine;

namespace Question.Data
{
    /// <summary>
    /// 讀取圖片的參數設定
    /// </summary>
    public class LoadQuestionsParameter
    {
        /// <summary>
        /// 出版社
        /// </summary>
        public Publisher Publisher;
        /// <summary>
        /// 年級
        /// </summary>
        public Grade Grade;
        /// <summary>
        /// 資料夾路徑
        /// </summary>
        public string FolderPath;
        /// <summary>
        /// 題目的 csv 檔案
        /// </summary>
        public TextAsset QuestionCsv;

        public override string ToString()
        {
            return $"{nameof(Publisher)}: {Publisher}, {nameof(Grade)}: {Grade}";
        }
    }
}