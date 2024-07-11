using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class LocalizationEntity
{
	public int id;
	public string name;
	public int price;
	public bool isNotForSale;
	public float rate;
	public string scene;
	public string en;
	public string tw;
	public string cn;
	public string jp;
	public string kr;
	public LocalizationCategory category;

}

public enum LocalizationCategory
{
	Red,
	Green,
	Blue,
}