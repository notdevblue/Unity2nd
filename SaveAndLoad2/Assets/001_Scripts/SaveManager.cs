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
            // ���� ���
            //JObject jSaveGame = new JObject();
            //for (int i = 0; i < enemies.Length; ++i)
            //{
            //    Enemy curEnemy = enemies[i];
            //    JObject jEnemy = curEnemy.Serialize();
            //    jSaveGame.Add(curEnemy.gameObject.name, jEnemy); // Key value �������� �����
            //}

            // Interface ���
            JObject jSaveGame = new JObject();
            for (int i = 0; i < objToSaveList.Count; ++i)
            {
                jSaveGame.Add(objToSaveList[i].GetJsonKey(), objToSaveList[i].Serialize());
            }

            // ���Ϸ� ����
            //StreamWriter sw = new StreamWriter(GetFilePath(saveFileName));
            //print("save to : " + GetFilePath(saveFileName));
            //sw.WriteLine(jSaveGame.ToString());
            //sw.Close();

            // ��ȣȭ
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
                // ��ȣȭ
                byte[] decryptedSaveGame = File.ReadAllBytes(fileStr);
                string jsonString = Decrypt(decryptedSaveGame);

                print(jsonString);

                // �� ����Ʈ �ҷ���
                //JObject jSaveGame = JObject.Parse(jsonString);
                //for (int i = 0; i < enemies.Length; ++i)
                //{
                //    Enemy curEnemy = enemies[i];
                //    string enemyString = jSaveGame[curEnemy.gameObject.name].ToString();
                //    curEnemy.DeSerialize(enemyString);
                //}

                // Interface ���
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
                // ���ο� ������ �����ؼ� �����÷��̿� ������ ����
                // OR Ŭ���� ����
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

        MemoryStream memoryStream = new MemoryStream(); // �޸� ���
        CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write); // Cryptostream ���� �Ƹ� ���ڵ� ��Ű��
        StreamWriter streamWriter = new StreamWriter(cryptoStream); // sw �� ����?

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

        MemoryStream memoryStream = new MemoryStream(); // �޸� ���
        CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read); // Cryptostream ���� �Ƹ� ���ڵ� ��Ű��
        StreamReader streamReader = new StreamReader(cryptoStream); // sw �� ����?

        string decryptedMsg = streamReader.ReadToEnd();

        streamReader.Close();
        cryptoStream.Close();
        memoryStream.Close();

        return decryptedMsg;
    }
}
