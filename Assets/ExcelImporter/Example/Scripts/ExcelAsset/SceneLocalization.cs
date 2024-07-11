using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset]
public class SceneLocalization : ScriptableObject
{
	public List<LocalizationEntity> Scene1; // Replace 'EntityType' to an actual type that is serializable.
	public List<LocalizationEntity> Scene2; // Replace 'EntityType' to an actual type that is serializable.
}
