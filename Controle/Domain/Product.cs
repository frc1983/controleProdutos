using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Controle.Helpers;

namespace Controle.Domain
{
	public class Product
	{
		public long Id { get; set; }
		public long Code { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int Quantity { get; set; }
		public decimal? UnityValue { get; set; }
		public decimal? WeightValue { get; set; }
		public Category Category { get; set; }
		public double Weight { get; set; }
		public Make Make { get; set; }
		public Provider Provider { get; set; }
		public long EAN { get; set; }
		public DateTime LastModification { get; set; }

		internal bool Validate()
		{
			string fraseError = "Os seguintes campos não estão válidos: \r\n\r\n";
			string error = "";

			if (this.Code <= 0)
				error += "- O código do produto deve ser inserido \r\n";

			if (String.IsNullOrEmpty(this.Name))
				error += "- O nome do produto deve ser inserido \r\n";

			if (String.IsNullOrEmpty(this.Description))
				error += "- A descrição do produto deve ser inserido \r\n";

			if (this.Quantity <= 0)
				error += "- A quantidade deste produto deve ser inserido \r\n";

			if (this.UnityValue <= 0 && this.WeightValue <= 0)
				error += "- O valor unitário ou valor por quilo do produto deve ser inserido \r\n";

			//if (this.WeightValue <= 0)
			//    error += "- O valor por peso do produto deve ser inserido \r\n";

			if (this.Category == null)
				error += "- Uma categoria do produto deve ser selecionada \r\n";

			if (this.Weight <= 0)
				error += "- O peso do produto deve ser inserido \r\n";

			if (this.Make == null)
				error += "- O Fabricante do produto deve ser selecionado \r\n";

			if (this.Provider == null)
				error += "- O fornecedor do produto deve ser selecionado \r\n";

			if (this.EAN <= 0)
				error += "- O código de barras do produto deve ser inserido \r\n";
			
			if (String.IsNullOrEmpty(error)) return true;
			else
			{
				ErrorInfo.DisplayErrorMessage(fraseError + error);
				return false;
			}
		}
	}
}
