using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Networking;

public class CSVReader : MonoBehaviour
{
    //System.Text.Encoding jp = System.Text.Encoding.GetEncoding(936);

    

    void Awake()
    {

#if UNITY_EDITOR
        ReadCsv("basicSong");
#else
        StartCoroutine(ReadCSVFromWeb("basicSong"));
#endif


    }

    void ReadCsv(string fileName)
    {

        //WriteTextFile();

        try
        {


            //Debug.Log(Application.persistentDataPath + path);
            StreamReader strReader = new StreamReader($"Music/{fileName}.royce");

            string results = "";
            bool endOfFile = false;

            while (!endOfFile)
            {

                string data_String = strReader.ReadLine();
                if (data_String == null)
                {
                    endOfFile = true;
                    break;
                }
                results += data_String + "\n";
            }
            Debug.Log(results);
        }
        catch
        {
            Debug.LogError("ERROR: File not found");
        }
        ;

        //string[] data = easyData.text.Split(new string[] {",","\n"}, System.StringSplitOptions.None);
        //data = System.Text.Encoding.UTF8.GetString(data);
        //data = jp.GetString(jp.GetBytes(data));
    }


    IEnumerator ReadCSVFromWeb(string fileName)
    {
        UnityWebRequest uwr = UnityWebRequest.Get($"./Music/{fileName}.royce");


        yield return uwr.SendWebRequest();

        
        if (uwr.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("ERROR: File not found");
        }
        else
        {
            string results = uwr.downloadHandler.text;
        }

        uwr.Dispose();
    }

}