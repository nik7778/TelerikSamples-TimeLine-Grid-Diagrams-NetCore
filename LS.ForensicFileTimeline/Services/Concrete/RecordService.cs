using LS.ForensicFileTimeline.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;

namespace LS.ForensicFileTimeline.Services
{
    public class RecordService : IRecordService
    {        
        public IEnumerable<Record> GetAll()
        {
            var list = new List<Record>();

            using (var stream = File.Open("Data.xlsx", FileMode.Open, FileAccess.Read))
            {
                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        list.Add(new Record
                        {
                            Id = Convert.ToInt32(worksheet.Cells[row, 1].Value.ToString().Trim()),
                            Date = Convert.ToDateTime(worksheet.Cells[row, 2].Value.ToString().Trim()),
                            Day = worksheet.Cells[row, 3].Value.ToString().Trim(),
                            Time = worksheet.Cells[row, 4].Value.ToString().Trim(),
                            Client = worksheet.Cells[row, 5].Value.ToString().Trim(),
                            Duration = Convert.ToDecimal(worksheet.Cells[row, 6].Value.ToString().Trim()),
                            DNB = worksheet.Cells[row, 7].Value != null ? Convert.ToBoolean(worksheet.Cells[row, 7].Value.ToString().Trim()) : false,
                            Status = worksheet.Cells[row, 8].Value != null ? worksheet.Cells[row, 8].Value.ToString().Trim() : string.Empty,
                            Memo = worksheet.Cells[row, 9].Value.ToString().Trim(),
                            Staff = worksheet.Cells[row, 10].Value.ToString().Trim()
                        });
                    }
                }
            }

            return list;
        }  
        
    }
}
