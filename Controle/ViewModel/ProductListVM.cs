using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Controle.Domain;

namespace Controle.ViewModel
{
	public class ProductListVM
	{
		Product p;

		public long Codigo { get { return p.Code; } }
		public string Produto { get { return p.Name; } }
		public int Quantidade { get; set; }
		public decimal? ValorUnidade { get { return p.UnityValue; } }
		public decimal? ValorPeso { get { return p.WeightValue; } }
		public decimal? TotalProduto { get; set; }

		public string TextoValorUnidade { get { return String.Format("{0:c}", p.UnityValue); } }
		public string TextoValorPeso { get { return String.Format("{0:c}", p.WeightValue); } }
		public string TextoTotalProduto { get { return String.Format("{0:c}", this.TotalProduto); } }

		public ProductListVM(Product p, int quantidade, decimal? total)
		{
			this.p = p;
			this.Quantidade = quantidade;
			this.TotalProduto = total;
		}

		public static ProductListVM ToProductListVM(Product p, int quantidade, decimal? total)
		{
			return new ProductListVM(p, quantidade, total);
		}
	}
}
