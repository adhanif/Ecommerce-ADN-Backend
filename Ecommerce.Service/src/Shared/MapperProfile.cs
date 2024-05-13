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

            #region User Mapper:
            CreateMap<UserCreateDto, User>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.UserEmail))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.UserPassword))
                .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.UserAvatar))
                .ForMember(dest => dest.UserRole, opt => opt.MapFrom(src => src.UserRole))
                 .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate));

            CreateMap<User, UserReadDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.UserAvatar, opt => opt.MapFrom(src => src.Avatar))
                .ForMember(dest => dest.UserRole, opt => opt.MapFrom(src => src.UserRole));

            CreateMap<UserUpdateDto, User>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.UserEmail))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.UserPassword))
                .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.UserAvatar))
                .ForMember(dest => dest.UserRole, opt => opt.MapFrom(src => src.UserRole));
            #endregion


            #region Product Mapper:
            CreateMap<ProductCreateDto, Product>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.ProductTitle))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.ProductDescription))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.ProductPrice))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
                 .ForMember(dest => dest.ProductImages, opt => opt.MapFrom(src => src.ProductImages));

            CreateMap<Product, ProductReadDto>()
                .ForMember(dest => dest.ProductTitle, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.ProductDescription, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.ProductPrice, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.ProductImages, opt => opt.MapFrom(src => src.ProductImages))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => src.UpdatedDate));


            CreateMap<ProductUpdateDto, Product>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.ProductTitle))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.ProductDescription))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.ProductPrice))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.Inventory, opt => opt.MapFrom(src => src.ProductInventory));

            CreateMap<Product, ProductUpdateDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ProductTitle, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.ProductDescription, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.ProductPrice, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.ProductInventory, opt => opt.MapFrom(src => src.Inventory));

            CreateMap<Product, ProductReviewReadDto>()
                .ForMember(dest => dest.ProductTitle, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.ProductDescription, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.ProductPrice, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId));

            #endregion


            #region Image Mapper:
            CreateMap<ProductImage, ProductImageReadDto>();
            CreateMap<ProductImageCreateDto, ProductImage>();
            CreateMap<ProductImageUpdateDto, ProductImage>();
            #endregion

            #region Category Mapper:
            CreateMap<Category, CategoryReadDto>()
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.CategoryImage, opt => opt.MapFrom(src => src.Image));

            CreateMap<CategoryCreateDto, Category>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CategoryName))
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.CategoryImage));

            CreateMap<CategoryUpdateDto, Category>()
             .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CategoryName))
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.CategoryImage));
            #endregion

            #region Order Mapper:
            CreateMap<Order, OrderReadDto>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User));

            CreateMap<OrderCreateDto, Order>()
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.OrderProducts, opt => opt.MapFrom(src => src.OrderProducts));

            CreateMap<OrderUpdateDto, Order>();
            CreateMap<Order, OrderReadUpdateDto>();
            #endregion

            #region Order Product Mapper:
            CreateMap<OrderProduct, OrderProductReadDto>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.ProductTitle, opt => opt.MapFrom(src => src.Product.Title))
                .ForMember(dest => dest.ProductPrice, opt => opt.MapFrom(src => src.Product.Price));

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