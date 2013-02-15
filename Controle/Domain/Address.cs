using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Controle.Helpers;

namespace Controle.Domain
{
	public class Address
	{
		public long Id { get; set; }
		public long IdClient { get; set; }
		public string Place { get; set; }
		public string Neighborhood { get; set; }
		public long Number { get; set; }
		public string Complement { get; set; }
		public City City { get; set; }
		public State State { get; set; }
		public Country Country { get; set; }
		public DateTime LastModification { get; set; }
		
		public bool Validate()
		{
			string fraseError = "Os seguintes campos não estão válidos: \r\n\r\n";
			string error = "";

			if (String.IsNullOrEmpty(this.Place))
				error += "- Um logradouro deve ser inserido \r\n";

			if (String.IsNullOrEmpty(this.Neighborhood))
				error += "- Um bairro deve ser inserido \r\n";

			if (this.Number == 0)
				error += "- Um número deve ser inserida \r\n";

			if (this.City == null)
				error += "- Uma cidade deve ser selecionada \r\n";

			if (this.State == null)
				error += "- Um estado deve ser selecionado \r\n";

			if (this.Country == null)
				error += "- Um país deve ser selecionado \r\n";

			if (String.IsNullOrEmpty(error)) return true;
			else
			{
				ErrorInfo.DisplayErrorMessage(fraseError + error);
				return false;
			}
		}
	}
}
