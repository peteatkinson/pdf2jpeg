using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace PDF2JPG
{
	public class Options
	{
		[Option('i', "input", Required = true, HelpText = "The directory in which holds all the PDFs")]
		public string Input { get; set; }

		[Option('o', "output", Required = true, HelpText = "Where the JPEGs will be put")]
		public string Output { get; set; }
	}
}
