using System;

namespace DapperStoredProc.Controllers
{
    internal class HttpPostedFileBase
    {
        public string FileName { get; internal set; }
        public int ContentLength { get; internal set; }

        internal void SaveAs(string path)
        {
            throw new NotImplementedException();
        }
    }
}