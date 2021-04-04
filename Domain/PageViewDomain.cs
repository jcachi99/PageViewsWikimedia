using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Wikimedia.Infraestructure;

namespace Wikimedia.Domain
{
    class PageViewDomain
    {
		private static string directoryPath = @"D:\";
		public void GetFiles(List<DateEntity> dates)
        {
			String url,destino;

			// Mostrar link para obtener zipeado
			// Console.WriteLine("https://dumps.wikimedia.org/other/pageviews/"+annio+"/"+annio+"-"+mes+"/pageviews-"+annio+mes+dia+"-"+c.ToString(fmt)+"0000.gz");

			foreach(DateEntity date in dates)
            {
				//Descargar archivos
				destino = Path.Combine("D:\\", "pageviews-" + date.year+date.month+date.day+"-"+date.hour + "0000.gz");
				url = "https://dumps.wikimedia.org/other/pageviews/" + date.year + "/" + date.year + "-" + date.month + "/pageviews-" + date.year + date.month + date.day + "-" + date.hour + "0000.gz";

				//Imprimir links de descarga
				Console.WriteLine("fecha: " + date.year + date.month + date.day + "-" + date.hour);
				//Console.WriteLine("https://dumps.wikimedia.org/other/pageviews/" + annio + "/" + annio + "-" + mes + "/pageviews-" + annio + mes + dia + "-" + c.ToString(fmt) + "0000.gz");
				Console.WriteLine("url: " + url + " destino: " + destino);

				WebClient myWebClient = new WebClient();
				myWebClient.DownloadFile(url, destino);
			}
		}

		public List<DateEntity> ObtainDates()
        {
			List<DateEntity> dateList = new List<DateEntity>();

			String fmt = "00";
			int hour = Int32.Parse(DateTime.Now.ToString("HH"));
			int month = Int32.Parse(DateTime.Now.Date.ToString("MM"));
			int day = Int32.Parse(DateTime.Now.Date.ToString("dd"));
			int year = Int32.Parse(DateTime.Now.Date.ToString("yyyy"));
			int h = hour;
			int m = month;
			int d = day;
			int y = year;

			for (int i = 0; i < 5; i++)
			{

				dateList.Add(new DateEntity(y.ToString(), m.ToString(fmt), d.ToString(fmt), h.ToString(fmt))); ;
				if (h == 0)
				{
					h = 24;
					if (d == 1)
					{
						d = DateTime.DaysInMonth(year, (m - 1));
						if (m == 1)
						{
							y = y - 1;
						}
						else
						{
							m = m - 1;
						}
					}
					else
					{
						d = d - 1;
					}
				}
				//Console.WriteLine(c.ToString(fmt));
				h--;
			}
			return dateList;
        }

		public void Decompress(FileInfo fileToDecompress)
        {

			using (FileStream originalFileStream = fileToDecompress.OpenRead())
			{
				string currentFileName = fileToDecompress.FullName;
				string newFileName = currentFileName.Remove(currentFileName.Length - fileToDecompress.Extension.Length);

				using (FileStream decompressedFileStream = File.Create(newFileName))
				{
					using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
					{
						decompressionStream.CopyTo(decompressedFileStream);
						Console.WriteLine($"Decompressed: {fileToDecompress.Name}");
					}
				}
			}
		}

		public void DecompressAll()
        {
			DirectoryInfo directorySelected = new DirectoryInfo(directoryPath);

			foreach (FileInfo fileToDecompress in directorySelected.GetFiles("*.gz"))
			{
				Decompress(fileToDecompress);
			}
		}

		public List<PageView> ReadInfo(DateEntity date)
		{
			String domainCode = "";
			String domainName = "";
			List<PageView> pageList = new List<PageView>();

			try {
					string[] lines = System.IO.File.ReadAllLines(@"D:\pageviews-" + date.year + date.month + date.day + "-" + date.hour + "0000");
					foreach (string line in lines)
					{
						
						var values = line.Split(" ");
						var valuesD = values[0].Split(".");
						int x = valuesD.Length;
						if (x == 1)
							{
								domainCode = "a";

							}
						else
							{
								domainCode = valuesD[x - 1];
							}

							switch (domainCode)
							{
								case "a":
									domainName = "wikipedia";
									break;
								case "b":
									domainName = "wikibooks";
									break;
								case "d":
									domainName = "wiktionary";
									break;
								case "f":
									domainName = "wikimediafoundation";
									break;
								case "m":
									domainName = "wikimedia";
									break;
								case "mw":
									domainName = "whitelisted_project";
									break;
								case "n":
									domainName = "wikinews";
									break;
								case "q":
									domainName = "wikiquote";
									break;
								case "s":
									domainName = "wikisource";
									break;
								case "v":
									domainName = "wikiversity";
									break;
								case "voy":
									domainName = "wikivoyage";
									break;
								case "w":
									domainName = "mediawiki";
									break;
								case "wd":
									domainName = "wikidata";
									break;
								default:
									domainName = "wikipedia";
									break;
							}

						int temp = 0; 
						if (int.TryParse(values[2], out temp))
						{
							pageList.Add(new PageView(date.hour, valuesD[0], domainName, values[1], Int32.Parse(values[2])));
						}
						
					}
				}
				catch (Exception ex)
                {
					Console.WriteLine("error: " + ex);
                }

			return pageList;

		}

		public void ObtainAllMaxViewPerPage (List<DateEntity> dates)
        {
			List<PageView> viewMaxCount = new List<PageView>();
			
			foreach (DateEntity date in dates)
			{
				List<PageView> pageList = new List<PageView>();
				pageList = ReadInfo(date);
				viewMaxCount.Add(ObtaiMaxViewPerPage(pageList));
			}

			Console.WriteLine("Period - Page - ViewCount");
			foreach (PageView element in viewMaxCount)
			{
				string Time = element.period + ":00:00";
				DateTime date = DateTime.Parse(Time, System.Globalization.CultureInfo.CurrentCulture);

				string t = date.ToString("hh tt");
				Console.WriteLine(t + " - " + element.pageTitle + " - " + element.viewCount);
			}
		}

		public void ObtainAllMaxViewLD(List<DateEntity> dates)
		{
			List<PageView> viewMaxCount = new List<PageView>();

			foreach (DateEntity date in dates)
			{
				List<PageView> pageList = new List<PageView>();
				pageList = ReadInfo(date);
				viewMaxCount.Add(ObtainMaxViewCountLD(pageList));
			}

			Console.WriteLine("Period - Language - Domain - ViewCount");
			foreach (PageView element in viewMaxCount)
			{
				string Time = element.period + ":00:00";
				DateTime date = DateTime.Parse(Time, System.Globalization.CultureInfo.CurrentCulture);

				string t = date.ToString("hh tt");
				Console.WriteLine(t + " - " + element.language + " - "+ element.domain + " - " + element.viewCount);
			}
		}

		public PageView ObtaiMaxViewPerPage(List<PageView> pageList)
		{

			List<PageView> temp = new List<PageView>();
			var query = from item in pageList
						group item by new { period = item.period ,page = item.pageTitle } into g
						select new
						{
							Key = g.Key,
							viewCount = g.Sum(x => x.viewCount)
						};

			foreach (var item in query)
            {
				temp.Add(new PageView(item.Key.period,"","",item.Key.page,item.viewCount));

			}

			List<PageView> pageOrderByViewDesc = temp.OrderByDescending(x => x.viewCount).ToList();
			PageView maxView = pageOrderByViewDesc[0];
			return maxView;
			
		}


		public PageView ObtainMaxViewCountLD(List<PageView> pageList)
        {

			List<PageView> temp = new List<PageView>();
			var query = from item in pageList
						group item by new { period = item.period,language = item.language, domain = item.domain } into g
						select new
						{
							Key = g.Key,
							viewCount = g.Sum(x => x.viewCount)
						};

			foreach (var item in query)
			{
				temp.Add(new PageView(item.Key.period, item.Key.language,item.Key.domain, "", item.viewCount));

			}

			List<PageView> pageOrderByViewDesc = temp.OrderByDescending(x => x.viewCount).ToList();
			PageView maxView = pageOrderByViewDesc[0];

			return maxView;
		}

	}
}
