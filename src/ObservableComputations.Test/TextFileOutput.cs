// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System;
using System.IO;

namespace ObservableComputations.Test
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
			bool success;
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
