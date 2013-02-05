using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Controle.Helpers;

namespace Controle.Domain
{
	public class Category
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }

		public bool Validate()
		{
			string separador = " e um ";
			string error = "";

			if (String.IsNullOrEmpty(this.Name))
				error += "A Descrição da categoria deve ser inserida";

			if (error != String.Empty) error += separador + "nome ";

			if (String.IsNullOrEmpty(this.Description) &&
				String.IsNullOrEmpty(error))
				error += "Nome ";

			if (String.IsNullOrEmpty(this.Description))
				error += "de categoria deve ser inserido";

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
