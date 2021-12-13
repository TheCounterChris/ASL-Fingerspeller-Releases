using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaverLoader
{
    public static List<Profile> savedProfiles = new List<Profile>();
    public static List<Profile> loadedProfiles = new List<Profile>();

    public static void SaveProfiles()
    {
        SaverLoader.savedProfiles = ProfileManager.instance.profiles;
        
        BinaryFormatter bf = new BinaryFormatter();

        FileStream file = File.Create(Application.persistentDataPath + "./Data/savedProfiles.prof");

        bf.Serialize(file, SaverLoader.savedProfiles);
        
        file.Close();

        Debug.Log("Saved in SaverLoader");
    }

    public static List<Profile> LoadProfiles()
    {
        if(File.Exists(Application.persistentDataPath + "./Data/savedProfiles.prof"))
        {
            BinaryFormatter bf = new BinaryFormatter();

            FileStream file = File.Open(Application.persistentDataPath + "./Data/savedProfiles.prof", FileMode.Open);

            SaverLoader.savedProfiles = (List<Profile>)bf.Deserialize(file);
            
            file.Close();

            Debug.Log("Loaded in SaverLoader");
            return SaverLoader.savedProfiles;            
        }
        else
        {
            Debug.Log("No file found from SaverLoader");
            return null;
        }             
    }

    public static void SaveProfiles2(List<Profile> profilesToSave)
    {
        for(int i = 0; i < profilesToSave.Count; i++)
        {
            Profile newProfile = new Profile();
            newProfile = newProfile.DeepCopy(profilesToSave[i]);
            savedProfiles.Add(newProfile);            
        }

        BinaryFormatter bf = new BinaryFormatter();

        FileStream file = File.Create(Application.persistentDataPath + "./Data/savedProfiles.prof");

        bf.Serialize(file, savedProfiles);
        
        file.Close();

        Debug.Log("Saved in SaverLoader");
    }

    public static void LoadProfiles2()
    {
        if(File.Exists(Application.persistentDataPath + "./Data/savedProfiles.prof"))
        {
            BinaryFormatter bf = new BinaryFormatter();

            FileStream file = File.Open(Application.persistentDataPath + "./Data/savedProfiles.prof", FileMode.Open);

            List<Profile> load = (List<Profile>)bf.Deserialize(file);
            
            file.Close();

            for(int i = 0; i < load.Count; i++)
            {
                Profile newProfile = new Profile();
                newProfile = newProfile.DeepCopy(load[i]);
                loadedProfiles[i] = newProfile;
            }

            Debug.Log("Loaded in SaverLoader");            
        }
    }
}