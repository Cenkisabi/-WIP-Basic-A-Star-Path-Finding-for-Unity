using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Products
{
    public class Barrack : MonoBehaviour, IProduct {
        public string GetName()
        {
            return "Barrack";
        }

        public Vector2 GetDimension()
        {
            return new Vector2(4,4);
        }

        public Image GetProductScrollImage()
        {
            throw new System.NotImplementedException();
        }

        public Image GetProductInfoImage()
        {
            throw new System.NotImplementedException();
        }

        public List<IProduct> GetProducibleProducts()
        {
            throw new System.NotImplementedException();
        }
    }
}
