using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriveManagement
{
    public class DriveDto
    {
        public string DriveLetter { get; set; }
        public int Order { get; set; }

        public bool IsSelected { get; set; }
    }
}
