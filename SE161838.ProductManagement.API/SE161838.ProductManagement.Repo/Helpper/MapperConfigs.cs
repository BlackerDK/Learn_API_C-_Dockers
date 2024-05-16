using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE161838.ProductManagement.Repo.Mappers
{
    public partial class MapperConfigs : Profile
    {
        public MapperConfigs()
        {
            CategoryMapperConfigs();         
        }
        partial void CategoryMapperConfigs();
        
    }
}
