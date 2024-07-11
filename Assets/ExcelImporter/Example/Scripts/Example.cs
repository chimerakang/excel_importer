using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Example : MonoBehaviour
{
	[SerializeField] SceneLocalization localizationItems;
	[SerializeField] Text text;

	void Start()
	{
		ShowItems();
	}

	void ShowItems()
	{
		string str = "";

		localizationItems.Scene1
			.ForEach(entity => str += DescribeMstItemEntity(entity) + "\n");

		localizationItems.Scene2
			.ForEach(entity => str += DescribeMstItemEntity(entity) + "\n");

		text.text = str;
	}

	string DescribeMstItemEntity(LocalizationEntity entity)
	{
		return string.Format(
			"{0} : {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}",
			entity.id,
			entity.name,
			entity.price,
			entity.isNotForSale,
			entity.rate,
			entity.scene,
			entity.en,
			entity.cn,
			entity.jp,
			entity.kr
		);
	}
}

