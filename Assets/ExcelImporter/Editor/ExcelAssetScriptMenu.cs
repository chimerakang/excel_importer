using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using System;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

public class ExcelAssetScriptMenu
{
    // The name of the script template file.
	const string ScriptTemplateName = "ExcelAssetScriptTemplete.cs.txt";

    // The template for the field definition within the generated script.
	const string FieldTemplete = "\t//public List<EntityType> #FIELDNAME#; // Replace 'EntityType' to an actual type that is serializable.";

    // MenuItem attribute to add a menu item for creating Excel asset script.
	[MenuItem("Assets/Create/ExcelAssetScript", false)]
	static void CreateScript()
	{
        // Open a folder panel to select the save path for the generated script.
		string savePath = EditorUtility.SaveFolderPanel("Save ExcelAssetScript", Application.dataPath, "");
		if (savePath == "") return;

        // Get the selected asset (Excel file).
		var selectedAssets = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets);

        // Get the path and name of the selected Excel file.
		string excelPath = AssetDatabase.GetAssetPath(selectedAssets[0]);
		string excelName = Path.GetFileNameWithoutExtension(excelPath);
        
        // Get the names of the sheets in the Excel file.
		List<string> sheetNames = GetSheetNames(excelPath);

        // Build the script string from the template and sheet names.
		string scriptString = BuildScriptString(excelName, sheetNames);

        // Determine the save path for the generated script file.
		string path = Path.ChangeExtension(Path.Combine(savePath, excelName), "cs");

        // Write the generated script to the specified path.
		File.WriteAllText(path, scriptString);

        // Refresh the AssetDatabase to include the new script.
		AssetDatabase.Refresh();
	}

    // MenuItem attribute to validate when the "Create ExcelAssetScript" menu item is enabled.
	[MenuItem("Assets/Create/ExcelAssetScript", true)]
	static bool CreateScriptValidation()
	{
        // Get the selected assets.
		var selectedAssets = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets);
        
        // Ensure only one asset is selected and it is an Excel file.
		if (selectedAssets.Length != 1) return false;
		var path = AssetDatabase.GetAssetPath(selectedAssets[0]);
		return Path.GetExtension(path) == ".xls" || Path.GetExtension(path) == ".xlsx";
	}

    // Get the names of the sheets in the specified Excel file.
	static List<string> GetSheetNames(string excelPath)
	{
		var sheetNames = new List<string>();
        
        // Open the Excel file.
		using (FileStream stream = File.Open(excelPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
		{
			IWorkbook book = null;
            
            // Determine if the file is an .xls or .xlsx file and create the appropriate workbook.
			if (Path.GetExtension(excelPath) == ".xls") book = new HSSFWorkbook(stream);
			else book = new XSSFWorkbook(stream);

            // Loop through all sheets and add their names to the list.
			for (int i = 0; i < book.NumberOfSheets; i++)
			{
				var sheet = book.GetSheetAt(i);
				sheetNames.Add(sheet.SheetName);
			}
		}
		return sheetNames;
	}

    // Get the script template string from the template file.
	static string GetScriptTempleteString()
	{
		string currentDirectory = Directory.GetCurrentDirectory();
        // Search for the script template file.
		string[] filePath = Directory.GetFiles(currentDirectory, ScriptTemplateName, SearchOption.AllDirectories);
		if (filePath.Length == 0) throw new Exception("Script template not found.");

        // Read the contents of the template file.
		string templateString = File.ReadAllText(filePath[0]);
		return templateString;
	}

    // Build the script string by replacing placeholders in the template with actual values.
	static string BuildScriptString(string excelName, List<string> sheetNames)
	{
        // Get the template string.
		string scriptString = GetScriptTempleteString();

        // Replace the asset script name placeholder.
		scriptString = scriptString.Replace("#ASSETSCRIPTNAME#", excelName);

        // Loop through each sheet name to create corresponding field definitions.
		foreach (string sheetName in sheetNames)
		{
            // Copy the field template and replace the placeholder with the sheet name.
			string fieldString = String.Copy(FieldTemplete);
			fieldString = fieldString.Replace("#FIELDNAME#", sheetName);
            
            // Append the field string to the script string.
			fieldString += "\n#ENTITYFIELDS#";
			scriptString = scriptString.Replace("#ENTITYFIELDS#", fieldString);
		}
        
        // Remove the last entity fields placeholder.
		scriptString = scriptString.Replace("#ENTITYFIELDS#\n", "");

		return scriptString;
	}
}
