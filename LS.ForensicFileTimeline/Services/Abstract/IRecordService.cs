using LS.ForensicFileTimeline.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LS.ForensicFileTimeline.Services
{
    public interface IRecordService
    {
        IEnumerable<Record> GetAll();
    }
}
