using AutoMapper;
using Ecommerce.Core.src.Entity;
using Ecommerce.Service.src.DTO;

namespace Ecommerce.Service.src.Shared
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            // #region User Mapper:
            CreateMap<User, UserReadDto>();
            CreateMap<UserCreateDto, User>();
            CreateMap<UserUpdateDto, User>();
            // #endregion

            // Address mappings
            CreateMap<Address, AddressReadDto>()
                // .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.User, opt => opt.Ignore()); // Ignore User to prevent circular reference
            CreateMap<AddressCreateDto, Address>();
            // .ForMember(dest => dest.Id, opt => opt.Ignore()); // Ignore Id, as it will be generated
            CreateMap<AddressUpdateDto, Address>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.AddressId));

            #region User Mapper:
            CreateMap<User, UserReadDto>();
            CreateMap<UserCreateDto, User>();
            CreateMap<UserUpdateDto, User>();
            #endregion

            #region Product Mapper:
            CreateMap<Product, ProductReadDto>();
            CreateMap<ProductCreateDto, Product>();
            CreateMap<ProductUpdateDto, Product>();


            CreateMap<Product, ProductReviewReadDto>()
                // .ForMember(dest => dest.ProductTitle, opt => opt.MapFrom(src => src.Title))
                // .ForMember(dest => dest.ProductDescription, opt => opt.MapFrom(src => src.Description))
                // .ForMember(dest => dest.ProductPrice, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId));

            #endregion


            #region Image Mapper:
            CreateMap<ProductImage, ProductImageReadDto>();
            CreateMap<ProductImageCreateDto, ProductImage>();
            CreateMap<ProductImageUpdateDto, ProductImage>();
            #endregion

            #region Category Mapper:
            CreateMap<Category, CategoryReadDto>();
            CreateMap<CategoryCreateDto, Category>();
            CreateMap<CategoryUpdateDto, Category>();
            #endregion

            #region Order Mapper:
            CreateMap<Order, OrderReadDto>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User));

            CreateMap<OrderCreateDto, Order>()
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.OrderProducts, opt => opt.MapFrom(src => src.OrderProducts));

            CreateMap<OrderUpdateDto, Order>();
            CreateMap<Order, OrderReadUpdateDto>()

                .ForMember(dest => dest.OrderProducts, opt => opt.MapFrom(src => src.OrderProducts));
            #endregion

            #region Order Product Mapper:
            CreateMap<OrderProduct, OrderProductReadDto>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Product.Title))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.Price));

            CreateMap<OrderProductCreateDto, OrderProduct>();
            CreateMap<OrderProductUpdateDto, OrderProduct>();
            #endregion

            #region Review mapper
            CreateMap<Review, ReviewReadDto>()
                .ForMember(dest => dest.ReviewRating, opt => opt.MapFrom(src => src.Rating))
                .ForMember(dest => dest.ReviewContent, opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId));
            CreateMap<ReviewReadDto, Review>();
            CreateMap<ReviewCreateDto, Review>()
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.ReviewRating))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.ReviewContent))
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.ProductId, opt => opt.Ignore());
            CreateMap<Review, ReviewCreateDto>();
            #endregion
        }
    }
}

// Will be modified during the time we use them.