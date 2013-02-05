using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xceed.Wpf.Toolkit;

namespace Controle.Helpers
{
	class ErrorInfo
	{
		public ErrorInfo() { }
		public static void DisplayErrorMessage(string message)
		{
			MessageBox.Show(message, "Problemas na validação", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
		}
	}
}
