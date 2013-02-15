using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Controle.Helpers;

namespace Controle.Domain
{
	public class Client
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public string CpfCnpj { get; set; }
		public string Sex { get; set; }
		public DateTime BirthDate { get; set; }
		public State Birthplace { get; set; }
		public MarriageState MarriageState { get; set; }
		public DateTime LastModification { get; set; }

		public long AddressId { get; set; }

		public bool Validate()
		{
			string fraseError = "Os seguintes campos não estão válidos: \r\n\r\n";
			string error = "";
			
			if (String.IsNullOrEmpty(this.Name))
				error += "- O nome do cliente deve ser inserido \r\n";

			if (String.IsNullOrEmpty(this.CpfCnpj))
				error += "- O CPF/CNPJ do cliente deve ser inserido \r\n";

			if (this.BirthDate == DateTime.MinValue)
				error += "- A data de nascimento deve ser inserida \r\n";

			if (this.Birthplace == null)
				error += "- Uma naturalidade deve ser selecionada \r\n";

			if (this.MarriageState == null)
				error += "- Um estado civíl do cliente deve ser selecionado \r\n";

			if (String.IsNullOrEmpty(error)) return true;
			else
			{
				ErrorInfo.DisplayErrorMessage(fraseError + error);
				return false;
			}
		}
	}
}
