using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Newtonsoft.Json.Linq;

public class SaveManager : MonoBehaviour
{
    const string saveFileName = "AllObjectSave.sav";

    public List<ISerializable> objToSaveList = new List<ISerializable>();

    private void Start()
    {
        //string msg = "Hello World";
        //byte[] encryptedMsg = Encrypt(msg);

        //string decryptedMsg = Decrypt(encryptedMsg);
        //print(decryptedMsg);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S)) // save
        {
            // 기존 방법
            //JObject jSaveGame = new JObject();
            //for (int i = 0; i < enemies.Length; ++i)
            //{
            //    Enemy curEnemy = enemies[i];
            //    JObject jEnemy = curEnemy.Serialize();
            //    jSaveGame.Add(curEnemy.gameObject.name, jEnemy); // Key value 형식으로 저장됨
            //}

            // Interface 사용
            JObject jSaveGame = new JObject();
            for (int i = 0; i < objToSaveList.Count; ++i)
            {
                jSaveGame.Add(objToSaveList[i].GetJsonKey(), objToSaveList[i].Serialize());
            }

            // 파일로 저장
            //StreamWriter sw = new StreamWriter(GetFilePath(saveFileName));
            //print("save to : " + GetFilePath(saveFileName));
            //sw.WriteLine(jSaveGame.ToString());
            //sw.Close();

            // 암호화
            byte[] encryptedSaveGame = Encrypt(jSaveGame.ToString());
            File.WriteAllBytes(GetFilePath(saveFileName), encryptedSaveGame);

            print(jSaveGame.ToString());
        }
        if (Input.GetKeyDown(KeyCode.L)) // load
        {
            print("load to: " + GetFilePath(saveFileName));

            string fileStr = GetFilePath(saveFileName);
            if (File.Exists(fileStr))
            {
                // 복호화
                byte[] decryptedSaveGame = File.ReadAllBytes(fileStr);
                string jsonString = Decrypt(decryptedSaveGame);

                print(jsonString);

                // 적 리스트 불러옴
                //JObject jSaveGame = JObject.Parse(jsonString);
                //for (int i = 0; i < enemies.Length; ++i)
                //{
                //    Enemy curEnemy = enemies[i];
                //    string enemyString = jSaveGame[curEnemy.gameObject.name].ToString();
                //    curEnemy.DeSerialize(enemyString);
                //}

                // Interface 사용
                JObject jSaveGame = JObject.Parse(jsonString);
                for (int i = 0; i < objToSaveList.Count; ++i)
                {
                    string objJsonStr = jSaveGame[objToSaveList[i].GetJsonKey()].ToString();
                    objToSaveList[i].DeSerialize(objJsonStr);
                }

            }
            else
            {
                print("Savefile is null");
                // 새로운 파일을 생성해서 게임플레이에 지장이 없게
                // OR 클라우드 저장
            }
        }
    }

    string GetFilePath(string filename)
    {
        return Application.persistentDataPath + "/" + filename;
    }

    byte[] _key = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10 };
    byte[] _initVector = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10 };

    byte[] Encrypt(string msg)
    {
        AesManaged aes = new AesManaged();
        ICryptoTransform encryptor = aes.CreateEncryptor(_key, _initVector);

        MemoryStream memoryStream = new MemoryStream(); // 메모리 잡고
        CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write); // Cryptostream 으로 아마 인코딩 시키고
        StreamWriter streamWriter = new StreamWriter(cryptoStream); // sw 에 저장?

        streamWriter.WriteLine(msg);

        streamWriter.Close();
        cryptoStream.Close();
        memoryStream.Close();

        return memoryStream.ToArray();
    }

    // sw => CryptoStream?

    string Decrypt(byte[] msg)
    {
        AesManaged aes = new AesManaged();
        ICryptoTransform decryptor = aes.CreateDecryptor(_key, _initVector);

        MemoryStream memoryStream = new MemoryStream(); // 메모리 잡고
        CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read); // Cryptostream 으로 아마 디코딩 시키고
        StreamReader streamReader = new StreamReader(cryptoStream); // sw 에 저장?

        string decryptedMsg = streamReader.ReadToEnd();

        streamReader.Close();
        cryptoStream.Close();
        memoryStream.Close();

        return decryptedMsg;
    }
}
