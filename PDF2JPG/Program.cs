using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Ghostscript.NET.Rasterizer;

namespace PDF2JPG
{
	class Program
	{
		public static void Main(string[] args)
		{
			var opts = new Options();

			if (ArgsValid(args, opts))
			{
				if (CheckInputOutput(opts.Input, opts.Output))
				{
					var files = GetFiles(opts.Input);

					foreach (var file in files)
					{
						using (var processor = new GhostscriptRasterizer())
						{
							processor.Open(string.Format("{0}/{1}", opts.Input, file));

							using (var image = processor.GetPage(300, 300, 1))
							{
								var regex = new Regex(".pdf", RegexOptions.IgnoreCase);

								var fileJpeg = regex.Replace(file, ".jpeg");

								var jpegPath = string.Format("{0}/{1}", opts.Output, fileJpeg);

								Console.WriteLine(string.Format("Converting... {0} to... {1}", file, jpegPath));

								if (!File.Exists(jpegPath))
								{
									image.Save(jpegPath, ImageFormat.Jpeg);
									Thread.Sleep(10);
								}
							}
						}
					}

					Thread.Sleep(250);
					Console.WriteLine(string.Format("{0} PDFs converted...", files.Length));
				}
			}

			Console.ReadLine();
		}

		public static List<string[]> Chunk(string[] array, int size)
		{
			var chunks = new List<string[]>();

			for (var i = 0; i < array.Length / size; i++)
			{
				chunks.Add(array.Skip(i * size).Take(size).ToArray());
			}

			return chunks;
		}

		public static string[] GetFiles(string input)
		{
			var files = Directory.GetFiles(input, "*.pdf")
				.Select(Path.GetFileName)
				.ToArray();
			return files;
		}

		private static bool CheckInputOutput(string input, string output)
		{
			if (!Directory.Exists(input))
			{
				return false;
			}

			if (!Directory.Exists(output))
			{
				Directory.CreateDirectory(output);
			}

			return true;
		}

		private static bool ArgsValid(string[] args, Options options)
		{
			return Parser.Default.ParseArgumentsStrict(args, options);
		}
	}
}
