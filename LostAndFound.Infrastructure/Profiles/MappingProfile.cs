using AutoMapper;
using LostAndFound.Core.Entities;
using LostAndFound.Infrastructure.DTOs.Comment;
using LostAndFound.Infrastructure.DTOs.Item;
using LostAndFound.Infrastructure.DTOs.ItemMedia;
using LostAndFound.Infrastructure.DTOs.Media;
using LostAndFound.Infrastructure.DTOs.Post;
using LostAndFound.Infrastructure.DTOs.PostMedia;
using LostAndFound.Infrastructure.DTOs.User;
using LostAndFound.Infrastructure.DTOs.UserMedia;
using LostAndFound.Infrastructure.DTOs.ViolationReport;

namespace LostAndFound.Infrastructure.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Media Mapping
            CreateMap<Media, MediaReadDTO>();
            CreateMap<Media, MediaDetailReadDTO>();
            CreateMap<MediaUpdateWriteDTO, Media>();
            CreateMap<MediaWriteDTO, Media>();

            //Post Media
            CreateMap<PostMedia, PostMediaReadDTO>();

            //Item Media
            CreateMap<ItemMedia, ItemMediaReadDTO>();

            //User Media Mapping
            CreateMap<UserMediaWriteDTO, UserMedia>();
            CreateMap<UserMedia, UserMediaReadDTO>();
            
            //User Mapping
            CreateMap<User, UserReadDTO>();
            CreateMap<User, UserDetailsReadDTO>();
            CreateMap<UserWriteDTO, User>();
            CreateMap<User, UserDetailAuthenticateReadDTO>();
            CreateMap<UserUpdatePasswordDTO, User>();
            CreateMap<UserUpdateDTO, User>();
            CreateMap<User, UserBriefDetailDTO>();

            //Item Mapping
            CreateMap<Item, ItemReadDTO>();
            CreateMap<ItemWriteDTO, Item>();

            //Category Mapping
            //CreateMap<Category, CategoryReadDTO>();
            //CreateMap<CategoryWriteDTO, Category>();

            //Violation Report Mapping
            CreateMap<ViolationReportWriteDTO, ViolationReport>();
            CreateMap<ViolationReport, ViolationReportReadDTO>();
            CreateMap<UserViolationReport, UserViolationReportDetailDTO>();
            
            //Post Mapping
            CreateMap<Post, PostReadDTO>();
            CreateMap<Post, PostDetailReadDTO>();
            CreateMap<Post, PostDetailWithCommentsReadDTO>();
            CreateMap<PostWriteDTO, Post>();
            CreateMap<PostUpdateDTO, Post>();
            CreateMap<PostStatusUpdateDTO, Post>();
            
            //Comment Mapping
            CreateMap<Comment, CommentReadDTO>();
            CreateMap<Comment, CommentDetailReadDTO>();
            CreateMap<Comment, CommentDetailWithReplyDetailReadDTO>();
            CreateMap<CommentWriteDTO, Comment>();
            CreateMap<CommentUpdateDTO, Comment>();

        }
    }
}
