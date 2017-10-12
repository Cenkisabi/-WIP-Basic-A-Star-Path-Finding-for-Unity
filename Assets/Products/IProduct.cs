using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Products
{
	public interface IProduct
	{
		string GetName();

		Vector2 GetDimension();
	
		
		Image GetProductScrollImage();
		Image GetProductInfoImage();

		List<IProduct> GetProducibleProducts();
	}
}
