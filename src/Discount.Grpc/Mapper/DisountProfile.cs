using AutoMapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Protos;

namespace Discount.Grpc.Mapper
{
    public class DisountProfile : Profile
    {
        public DisountProfile()
        {
            CreateMap<Coupon, CouponModel>().ReverseMap();
        }
    }
}
