using System;
using UnityEditor;
using UnityEngine;

namespace GoogleSheetsForUnity
{
    [ExecuteAlways]
    public class WeaponSpreadsheetManager : MonoBehaviour
    {
        [SerializeField] private string weaponToRetrieveID;

        // Simple struct for the example.
        [System.Serializable]
        public struct WeaponInfos
        {
            public string ID;
            public string Type; 
            public string Description;
            public float FireRate;
            public int Slots_Amount;
        }

        public WeaponStats weaponStats;

        // Create an example object.
        private WeaponInfos _WeaponData;

        // For the table to be created and queried.
        private string _tableName = "Weapons";

        private void OnEnable()
        {
            // Suscribe for catching cloud responses.
            Drive.responseCallback += HandleDriveResponse;
        }

        private void OnDisable()
        {
            // Remove listeners.
            Drive.responseCallback -= HandleDriveResponse;
        }

        private void OnGUI()
        {
            _WeaponData = new WeaponInfos { ID = weaponStats.ID, Type = weaponStats.Type.ToString(), Description = weaponStats.Description, FireRate = weaponStats.FireRate, Slots_Amount = weaponStats.Slots_Amount };

            GUILayout.BeginArea(new Rect(10, 10, 800, 1500));
            GUILayout.BeginHorizontal();
            GUILayout.Space(10f);
            GUILayout.BeginVertical();

            GUILayout.Space(10f);
            GUILayout.BeginVertical("Currently selected 'Weapon' Object :", GUI.skin.box, GUILayout.MaxWidth(230));
            GUILayout.Space(20f);

            GUILayout.BeginHorizontal();
            GUILayout.Space(10f);
            GUILayout.Label("Description:", GUILayout.MaxWidth(200f));
            GUILayout.Label(_WeaponData.Description, GUILayout.MaxWidth(400f));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Space(10f);
            GUILayout.Label("Gameplay Type:", GUILayout.MaxWidth(200f));
            GUILayout.Label(_WeaponData.Type.ToString(), GUILayout.MaxWidth(400f));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Space(10f);
            GUILayout.Label("Fire Rate:", GUILayout.MaxWidth(200f));
            GUILayout.Label(_WeaponData.FireRate.ToString(), GUILayout.MaxWidth(100f));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Space(10f);
            GUILayout.Label("Slots Amount:", GUILayout.MaxWidth(200f));
            GUILayout.Label(_WeaponData.Slots_Amount.ToString(), GUILayout.MaxWidth(100f));
            GUILayout.EndHorizontal();

            GUILayout.Space(5f);
            GUILayout.EndVertical();

            GUILayout.Space(10f);

            if (GUILayout.Button("Create Table", GUILayout.MinHeight(20f), GUILayout.MaxWidth(220f)))
                CreateWeaponTable();

            if (GUILayout.Button("Update or Create Weapon", GUILayout.MinHeight(20f), GUILayout.MaxWidth(220f)))
                UpdateWeapon(true);

            if (GUILayout.Button("Retrieve Weapon", GUILayout.MinHeight(20f), GUILayout.MaxWidth(220f)))
                GenerateSOFromWeapon();

            /*if (GUILayout.Button("Retrieve All Weapons", GUILayout.MinHeight(20f), GUILayout.MaxWidth(220f)))
                GetAllWeapons();

            if (GUILayout.Button("Retrieve All Tables", GUILayout.MinHeight(20f), GUILayout.MaxWidth(220f)))
                GetAllTables(); */

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        private void CreateWeaponTable()
        {
            Debug.Log("<color=yellow>Creating a table in the cloud for Weapons data.</color>");

            // Creating a string array for field names (table headers) .
            string[] fieldNames = new string[5];
            fieldNames[0] = "ID";
            fieldNames[1] = "Type";
            fieldNames[2] = "Description";
            fieldNames[3] = "FireRate";
            fieldNames[4] = "Slots_Amount";

            // Request for the table to be created on the cloud.
            Drive.CreateTable(fieldNames, _tableName, true);
        }

        private void UpdateWeapon(bool create)
        {
            _WeaponData = new WeaponInfos { ID = weaponStats.ID, Type = weaponStats.Type.ToString(), Description = weaponStats.Description, FireRate = weaponStats.FireRate, Slots_Amount = weaponStats.Slots_Amount };

            // Get the json string of the object.
            string jsonWeapon = JsonUtility.ToJson(_WeaponData);

            // Look in the 'WeaponInfo' table, for an object of name as specified, and overwrite with the current obj data.
            Drive.UpdateObjects(_tableName, "ID", _WeaponData.ID, jsonWeapon, create, true);
        }

        private void GenerateSOFromWeapon()
        {
            Drive.GetObjectsByField(_tableName, "ID", weaponToRetrieveID, true);
        }

        private void GetAllWeapons()
        {
            Debug.Log("<color=yellow>Retrieving all Weapons from the Cloud.</color>");

            // Get all objects from table 'WeaponInfo'.
            Drive.GetTable(_tableName, true);
        }

        private void GetAllTables()
        {
            Debug.Log("<color=yellow>Retrieving all data tables from the Cloud.</color>");

            // Get all objects from table 'WeaponInfo'.
            Drive.GetAllTables(true);
        }

        // Processes the data received from the cloud.
        public void HandleDriveResponse(Drive.DataContainer dataContainer)
        {
            Debug.Log(dataContainer.msg);

            // First check the type of answer.
            if (dataContainer.QueryType == Drive.QueryType.getObjectsByField)
            {
                string rawJSon = dataContainer.payload;
                Debug.Log(rawJSon);

                // Check if the type is correct.
                if (string.Compare(dataContainer.objType, _tableName) == 0)
                {
                    // Parse from json to the desired object type.
                    WeaponInfos[] Weapons = JsonHelper.ArrayFromJson<WeaponInfos>(rawJSon);

                    for (int i = 0; i < Weapons.Length; i++)
                    {
                        _WeaponData = Weapons[i];
                        Debug.Log("<color=yellow>Object retrieved from the cloud and parsed: \n</color>" +
                            "Description: " + _WeaponData.Description + "\n" +
                            "Gameplay Type: " + _WeaponData.Type + "\n" +
                            "FireRate: " + _WeaponData.FireRate + "\n" +
                            "Slots Amount: " + _WeaponData.Slots_Amount + "\n");

                        WeaponStats temp = ScriptableObject.CreateInstance<WeaponStats>();
                        temp.ID = Weapons[i].ID;
                        Enum.TryParse(Weapons[i].Type, out temp.Type);
                        temp.Description = _WeaponData.Description;
                        temp.FireRate = _WeaponData.FireRate;
                        temp.Slots_Amount = _WeaponData.Slots_Amount;

                        string path = $"Assets/Scripts/Tools/ExcelToSO/Data/Weapons/{temp.ID.Replace('_', ' ')}.asset";
                        AssetDatabase.CreateAsset(temp, path);
                    }
                }
            }

            // First check the type of answer.
            if (dataContainer.QueryType == Drive.QueryType.getTable)
            {
                string rawJSon = dataContainer.payload;
                Debug.Log(rawJSon);

                // Check if the type is correct.
                if (string.Compare(dataContainer.objType, _tableName) == 0)
                {
                    // Parse from json to the desired object type.
                    WeaponInfos[] Weapons = JsonHelper.ArrayFromJson<WeaponInfos>(rawJSon);

                    string logMsg = "<color=yellow>" + Weapons.Length.ToString() + " objects retrieved from the cloud and parsed:</color>";
                    for (int i = 0; i < Weapons.Length; i++)
                    {
                        logMsg += "\n" +
                            "<color=blue>Description: " + Weapons[i].Description + "</color>\n" +
                            "Gameplay Type: " + Weapons[i].Type + "\n" +
                            "FireRate: " + Weapons[i].FireRate + "\n" +
                            "Slots Amount: " + Weapons[i].Slots_Amount + "\n";
                    }
                    Debug.Log(logMsg);
                }
            }

            // First check the type of answer.
            if (dataContainer.QueryType == Drive.QueryType.getAllTables)
            {
                string rawJSon = dataContainer.payload;

                // The response for this query is a json list of objects that hold tow fields:
                // * objType: the table name (we use for identifying the type).
                // * payload: the contents of the table in json format.
                Drive.DataContainer[] tables = JsonHelper.ArrayFromJson<Drive.DataContainer>(rawJSon);

                // Once we get the list of tables, we could use the objTypes to know the type and convert json to specific objects.
                // On this example, we will just dump all content to the console, sorted by table name.
                string logMsg = "<color=yellow>All data tables retrieved from the cloud.\n</color>";
                for (int i = 0; i < tables.Length; i++)
                {
                    logMsg += "\n<color=blue>Table Name: " + tables[i].objType + "</color>\n" + tables[i].payload + "\n";
                }
                Debug.Log(logMsg);
            }
        }

    }
}


