using Godot;
using System;
using System.Collections.Generic;
using EventCallback;

public class IO : Control
{
    //Grab a refference to the text editor 
    ItemList itemList;
    //The Path the saves must be saved to
    const string SAVE_DIR = "user://saves/";
    //The file name to be used for the save files
    string savePath = SAVE_DIR + "save.dat";

    //Create a godot dictionary 
    Godot.Collections.Dictionary<string, int> saveData;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        saveData = new Godot.Collections.Dictionary<string, int>()
        {
            { "Filename", 10 },
            { "Parent", 10},
            { "PosX", 10 }, // Vector2 is not supported by JSON
            { "PosY", 10 },
            { "Attack", 10 },
            { "Defense",10 },
            { "CurrentHealth", 10 },
            { "MaxHealth", 10},
            { "Damage", 10 },
            { "Regen", 10 },
            { "Experience", 10},
            { "Tnl", 10 },
            { "Level", 10 },
            { "AttackGrowth", 10 },
            { "DefenseGrowth", 10 },
            { "HealthGrowth", 10 },
            { "IsAlive", 10 },
            { "LastAttack", 10 }
        };

        itemList = GetNode<ItemList>("MainVBox/ItemList");
        //Save();
        //ListFilesInDirectory();
    }

    private void Save() //Input some random save junk here
    {
        //Make a new reffernce of the directory func
        Directory dir = new Directory();
        //Check if the directory exists
        if (!dir.FileExists(SAVE_DIR))
        {
            //If the directory does not exist then we create all the folder and sub folders need for the directory to exist
            dir.MakeDirRecursive(SAVE_DIR);
        }
        //Create a new file refference
        File file = new File();
        //Attempt to open file for editing and catch eny errors that might occur
        Error err = file.OpenEncryptedWithPass(savePath, File.ModeFlags.Read, "Hello101");
        //If the error returns ok we can continue with accessing the file
        if (err == Error.Ok)
        {
            //Save the godot type dictionary info
            file.StoreVar(saveData);
            //Close the file after accesing to release it for later access by other functions
            file.Close();
        }
    }


    private void Load()
    {
        //Create a new file refference
        File file = new File();
        if (file.FileExists(savePath))
        {
            //Attempt to open file for editing and catch eny errors that might occur
            Error err = file.OpenEncryptedWithPass(savePath, File.ModeFlags.Read, "Hello101");
            if (err == Error.Ok)
            {
                //Get the raw date and convert it to the format of the dictionary
                saveData = file.GetVar() as Godot.Collections.Dictionary<string, int>;
                //Close the access point created for the file
                file.Close();
            }
        }
    }

    private void ListFilesInDirectory()
    {
        List<string> files = new List<string>();
        //Create a new refference to the directory class
        Directory dir = new Directory();
        //Opens the save directory
        dir.Open(SAVE_DIR);
        //Sets the directory to begin listing the files in
        dir.ListDirBegin();

        while (true)
        {
            //Get the file name in a temp string for parsing first
            string tempFile = dir.GetNext();
            //Check the last entry if it is an empty string and if it is break out of the loop
            if (tempFile == "") break;
            //If the file does not start with a . we can add it to the list of file names
            if (!tempFile.StartsWith("."))
            {
                tempFile = tempFile.Remove(tempFile.Find(".", 0, false), 4);
                //Add the fiel to the list to display
                files.Add(tempFile);
                //Write the file names on the text edit
                itemList.AddItem(tempFile, null, false);
            }
        }
        //Closes the read on the foolder content for the dir class
        dir.ListDirEnd();
    }
}
