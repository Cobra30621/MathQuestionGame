using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Question
{
    public class CsvLoader
    {
        // public string [][] VTuberAnswers;
        // 注意:檔案類型要txt
        public void Init(){
            // VTuberAnswers = LoadData("holotalk");
            // options = LoadData("options");
        }
        
        public string [][] LoadData(string filename){
            //读取csv二进制文件  
            TextAsset binAsset = Resources.Load (filename, typeof(TextAsset)) as TextAsset;         
            
            //读取每一行的内容  
            string [] lineArray = binAsset.text.Split ("\n"[0]);  
            
            //创建二维数组  
            string [][] array;
            array = new string [lineArray.Length][];  
            
            //把csv中的数据储存在二位数组中  
            for(int i =0;i < lineArray.Length; i++)  
            {  
                array[i] = lineArray[i].Split (',');  
            
            }

            return array;
        }
        
        
    }
}