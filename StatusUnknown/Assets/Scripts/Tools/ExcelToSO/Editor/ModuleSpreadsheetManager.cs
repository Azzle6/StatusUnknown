using System;
using UnityEditor;
using UnityEngine;

namespace GoogleSheetsForUnity
{
    /* 
        This example will create a number of buttons on the scene, with self describing actions.
        It introduces to basic operations to handle spreadsheets with the API to make CRUD operations on:
        tables (worksheets) with fields (column headers), as well as objects (rows) on those tables.
    */
    [ExecuteAlways]
    public class ModuleSpreadsheetManager : MonoBehaviour
    {
        [SerializeField] private string moduleToRetrieveID; 

        // Simple struct for the example.
        [System.Serializable]
        public struct ModuleInfo
        {
            public string ID; 
            public string Description;
            public int Size;
            public int Effectors_Amount;
        }

        public ModuleStats ModuleStats;

        // Create an example object.
        private ModuleInfo _ModuleData;
        
        // For the table to be created and queried.
        private string _tableName = "Modules";

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
            _ModuleData = new ModuleInfo { ID = ModuleStats.ID, Description = ModuleStats.Description, Size = ModuleStats.Size, Effectors_Amount = ModuleStats.Effectors_Amount };

            GUILayout.BeginArea(new Rect(10, 10, 800, 1500));
            GUILayout.BeginHorizontal();
            GUILayout.Space(10f);
            GUILayout.BeginVertical();

            GUILayout.Space(10f);
            GUILayout.BeginVertical("Currently selected 'Module' Object :", GUI.skin.box, GUILayout.MaxWidth(230));
            GUILayout.Space(20f);

            GUILayout.BeginHorizontal();
            GUILayout.Space(10f);
            GUILayout.Label("Description:", GUILayout.MaxWidth(200f));
            GUILayout.Label(_ModuleData.Description, GUILayout.MaxWidth(400f));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Space(10f);
            GUILayout.Label("Size:", GUILayout.MaxWidth(200f));
            GUILayout.Label(_ModuleData.Size.ToString(), GUILayout.MaxWidth(100f));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Space(10f);
            GUILayout.Label("Effectors Amount:", GUILayout.MaxWidth(200f));
            GUILayout.Label(_ModuleData.Effectors_Amount.ToString(), GUILayout.MaxWidth(100f));
            GUILayout.EndHorizontal();

            GUILayout.Space(5f);
            GUILayout.EndVertical();

            GUILayout.Space(10f);

            if (GUILayout.Button("Create Table", GUILayout.MinHeight(20f), GUILayout.MaxWidth(220f)))
                CreateModuleTable();

            if (GUILayout.Button("Update or Create Module", GUILayout.MinHeight(20f), GUILayout.MaxWidth(220f)))
                UpdateModule(true);

            if (GUILayout.Button("Retrieve Module", GUILayout.MinHeight(20f), GUILayout.MaxWidth(220f)))
                GenerateSOFromModule();

            /*if (GUILayout.Button("Retrieve All Modules", GUILayout.MinHeight(20f), GUILayout.MaxWidth(220f)))
                GetAllModules();

            if (GUILayout.Button("Retrieve All Tables", GUILayout.MinHeight(20f), GUILayout.MaxWidth(220f)))
                GetAllTables(); */

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        private void CreateModuleTable()
        {
            Debug.Log("<color=yellow>Creating a table in the cloud for Modules data.</color>");

            // Creating a string array for field names (table headers) .
            string[] fieldNames = new string[4];
            fieldNames[0] = "ID"; 
            fieldNames[1] = "Description";
            fieldNames[2] = "Size";
            fieldNames[3] = "Effectors_Amount";

            // Request for the table to be created on the cloud.
            Drive.CreateTable(fieldNames, _tableName, true);
        }

        private void UpdateModule(bool create)
        {
            _ModuleData = new ModuleInfo { ID = ModuleStats.ID, Description = ModuleStats.Description, Size = ModuleStats.Size, Effectors_Amount = ModuleStats.Effectors_Amount };

            // Get the json string of the object.
            string jsonModule = JsonUtility.ToJson(_ModuleData);

            // Look in the 'ModuleInfo' table, for an object of name as specified, and overwrite with the current obj data.
            Drive.UpdateObjects(_tableName, "ID", _ModuleData.ID, jsonModule, create, true);
        }

        private void GenerateSOFromModule()
        {
            Drive.GetObjectsByField(_tableName, "ID", moduleToRetrieveID, true);
        }

        private void GetAllModules()
        {
            Debug.Log("<color=yellow>Retrieving all Modules from the Cloud.</color>");

            // Get all objects from table 'ModuleInfo'.
            Drive.GetTable(_tableName, true);
        }

        private void GetAllTables()
        {
            Debug.Log("<color=yellow>Retrieving all data tables from the Cloud.</color>");

            // Get all objects from table 'ModuleInfo'.
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
                    ModuleInfo[] Modules = JsonHelper.ArrayFromJson<ModuleInfo>(rawJSon);

                    for (int i = 0; i < Modules.Length; i++)
                    {
                        _ModuleData = Modules[i];
                        Debug.Log("<color=yellow>Object retrieved from the cloud and parsed: \n</color>" +
                            "Description: " + _ModuleData.Description + "\n" +
                            "Size: " + _ModuleData.Size + "\n" +
                            "Effectors Amount: " + _ModuleData.Effectors_Amount + "\n");

                        ModuleStats temp = ScriptableObject.CreateInstance<ModuleStats>();
                        temp.ID = Modules[i].ID; 
                        temp.Description = _ModuleData.Description;
                        temp.Size = _ModuleData.Size;
                        temp.Effectors_Amount = _ModuleData.Effectors_Amount;

                        string path = $"Assets/Scripts/Tools/ExcelToSO/Data/Modules/{temp.ID.Replace('_', ' ')}.asset";                       
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
                    ModuleInfo[] Modules = JsonHelper.ArrayFromJson<ModuleInfo>(rawJSon);

                    string logMsg = "<color=yellow>" + Modules.Length.ToString() + " objects retrieved from the cloud and parsed:</color>";
                    for (int i = 0; i < Modules.Length; i++)
                    {
                        logMsg += "\n" +
                            "<color=blue>Description: " + Modules[i].Description + "</color>\n" +
                            "Size: " + Modules[i].Size + "\n" +
                            "Effectors Amount: " + Modules[i].Effectors_Amount + "\n";
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



