using System;
using System.IO;

namespace IBCode.ObservableCalculations.Test
{
	public class TextFileOutput
	{
		public TextFileOutput(string fileName)
		{
			FileName = fileName;
			FileInfo fileInfo = new FileInfo(FileName);
			if (fileInfo.Exists)
			{
				fileInfo.Delete();
			}
		}

		private string FileName { get; set; }

		public void AppentLine(string text)
		{
			bool success = false;
			do
			{
				try
				{
					FileInfo fileInfo = new FileInfo(FileName);
					if (fileInfo.Exists && fileInfo.Length > 1 * 1024 * 1024)
					{
						fileInfo.Delete();
						File.WriteAllText(FileName, text);
					}
					else
					{
						File.AppendAllLines(FileName, new[] {text});
					}	
				}
				catch (Exception e)
				{
	
				}
				success = true;

			} while (!success);

		}
	}
}
