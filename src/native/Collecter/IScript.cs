using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collecter
{
	interface IScript
	{
		void Run(bool reset);
		string GetProgressString();
		void ResetProgress();
	}
}
