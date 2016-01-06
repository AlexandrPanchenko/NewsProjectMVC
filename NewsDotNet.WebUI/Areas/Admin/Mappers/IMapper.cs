using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsDotNet.WebUI.Areas.Admin.Mappers
{
    public interface IMapper
    {
        TDestinationType Map<TSourceType, TDestinationType>(TSourceType source);
    }
}
