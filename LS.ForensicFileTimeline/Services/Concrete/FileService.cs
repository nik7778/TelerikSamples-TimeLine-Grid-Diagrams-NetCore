using LS.ForensicFileTimeline.Models;
using LumenWorks.Framework.IO.Csv;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace LS.ForensicFileTimeline.Services
{
    public class FileService : IFileService
    {
        public IEnumerable<FileModel> GetAll()
        {
            List<FileModel> records = new List<FileModel>();
            var csvTable = new DataTable();
            using (var csvReader = new CsvReader(new StreamReader(System.IO.File.OpenRead(@"App_Data\TimelineData.csv")), true))
            {
                csvTable.Load(csvReader);
            }

            for (int i = 0; i < csvTable.Rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(csvTable.Rows[i][0].ToString())) {
                    var file = csvTable.Rows[i][1].ToString().Split('/').Last();
                    var created = csvTable.Rows[i][6].ToString().Split("(").First();
                    var accessed = csvTable.Rows[i][7].ToString().Split("(").First();
                    var modified = csvTable.Rows[i][8].ToString().Split("(").First();

                    records.Add(new FileModel
                    {
                        Item = csvTable.Rows[i][0].ToString().Trim(),
                        Path = csvTable.Rows[i][1].ToString().Trim(),
                        Folder = csvTable.Rows[i][1].ToString().Replace(file, ""),
                        Category = csvTable.Rows[i][2].ToString().Trim(),
                        PSize = csvTable.Rows[i][3].ToString().Trim(),
                        LSize = csvTable.Rows[i][4].ToString().Trim(),
                        MD5 = csvTable.Rows[i][5].ToString().Trim(),
                        Created = csvTable.Rows[i][6] != null && !csvTable.Rows[i][6].Equals("n/a")
                                ? Convert.ToDateTime(created)
                                : DateTime.Now.Date,
                        Accessed = csvTable.Rows[i][7] != null && !csvTable.Rows[i][7].Equals("n/a")
                                ? Convert.ToDateTime(accessed)
                                : DateTime.Now.Date,
                        Modified = csvTable.Rows[i][8] != null && !csvTable.Rows[i][8].Equals("n/a")
                                ? Convert.ToDateTime(modified)
                                : DateTime.Now.Date,
                        FileName = file.Split('.').First(),
                        Extension = file.Split('.').Last()
                    });
                }
            }
            return records;
        }

    }

    public interface IFileService
    {
        IEnumerable<FileModel> GetAll();
    }
}