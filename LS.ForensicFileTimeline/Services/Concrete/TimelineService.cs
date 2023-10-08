using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LS.ForensicFileTimeline.Models;

namespace LS.ForensicFileTimeline.Services
{
    public class TimelineService : ITimelineService
    {        
        public IEnumerable<FileModel> GetAll()
        {
            var list = new List<FileModel>();                       
            
            using (var stream = File.Open("App_Data/TimelineData.xlsx", FileMode.Open, FileAccess.Read))
            {
                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        var file = worksheet.Cells[row, 2].Value.ToString().Split('/').Last();
                        var created = worksheet.Cells[row, 7].Value.ToString().Split("(").First();
                        var accessed = worksheet.Cells[row, 8].Value.ToString().Split("(").First();
                        var modified = worksheet.Cells[row, 9].Value.ToString().Split("(").First();

                        list.Add(new FileModel
                        {
                            Item = worksheet.Cells[row, 1].Value.ToString().Trim(),
                            Path = worksheet.Cells[row, 2].Value.ToString().Trim(),
                            Folder = worksheet.Cells[row, 2].Value.ToString().Replace(file, ""),
                            Category = worksheet.Cells[row, 3].Value.ToString().Trim(),
                            PSize = worksheet.Cells[row, 4].Value.ToString().Trim(),
                            LSize = worksheet.Cells[row, 5].Value.ToString().Trim(),
                            MD5 = worksheet.Cells[row, 6].Value.ToString().Trim(),
                            Created = worksheet.Cells[row, 7].Value != null && !worksheet.Cells[row, 7].Value.Equals("n/a")
                            ? Convert.ToDateTime(created)
                            : DateTime.Now.Date,
                            Accessed = worksheet.Cells[row, 8].Value != null && !worksheet.Cells[row, 8].Value.Equals("n/a")
                            ? Convert.ToDateTime(accessed)
                            : DateTime.Now.Date,
                            Modified = worksheet.Cells[row, 9].Value != null && !worksheet.Cells[row, 9].Value.Equals("n/a")
                            ? Convert.ToDateTime(modified)
                            : DateTime.Now.Date,
                            FileName = file.Split('.').First(),
                            Extension = file.Split('.').Last()
                        });
                    }
                }
            }            

            return list;
        }
    }

    public interface ITimelineService
    {
        IEnumerable<FileModel> GetAll();
    }
}
