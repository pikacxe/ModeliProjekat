using FTN.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClient.ViewModels
{
    public class ResourceDescriptionViewModel
    {
        public long GlobalId { get; set; }
        public string GlobalIdHex
        {
            get
            {
                return String.Format("0x{0:X16}", GlobalId);
            }
        }
        public string Type { get; set; }
        public List<PropertyViewModel> Properties { get; set; }
    }
}
