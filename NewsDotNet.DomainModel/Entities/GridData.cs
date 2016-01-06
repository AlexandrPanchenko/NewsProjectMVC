using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsDotNet.DomainModel.Entities
{
    public class GridData
    {
        public int ID { get; set; }
        public int Col { get; set; }
        public int Row { get; set; }
        public int SizeX { get; set; }
        public int SizeY { get; set; }
    }
}
