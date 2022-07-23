using AutoMapper;
using MarketApp.DataTransferObjects;
using MarketApp.Models;

namespace MarketApp.Helpers
{
    public class MarketMapper : Profile
    {
        public MarketMapper()
        {
            CreateMap<ProductDto, Product>().ReverseMap();
        }
    }
}
