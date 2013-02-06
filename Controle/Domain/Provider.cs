using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Controle.Helpers;

namespace Controle.Domain
{
	public class Provider
	{
		public long Id { get; set; }
		public string Name { get; set; }

		internal bool Validate()
		{
			string error = "";

			if (String.IsNullOrEmpty(this.Name))
				error += "O Nome do fornecedor deve ser inserido";

			if (error != String.Empty) error += ".";

			if (String.IsNullOrEmpty(error)) return true;
			else
			{
				ErrorInfo.DisplayErrorMessage(error);
				return false;
			}
		}
	}
}
