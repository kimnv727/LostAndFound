﻿using AutoMapper;
using LostAndFound.Core.Entities;
using LostAndFound.Infrastructure.DTOs.Cabinet;
using LostAndFound.Infrastructure.DTOs.Category;
using LostAndFound.Infrastructure.DTOs.CategoryGroup;
using LostAndFound.Infrastructure.DTOs.Comment;
using LostAndFound.Infrastructure.DTOs.CommentFlag;
using LostAndFound.Infrastructure.DTOs.Giveaway;
using LostAndFound.Infrastructure.DTOs.GiveawayParticipant;
using LostAndFound.Infrastructure.DTOs.Item;
using LostAndFound.Infrastructure.DTOs.ItemBookmark;
using LostAndFound.Infrastructure.DTOs.ItemClaim;
using LostAndFound.Infrastructure.DTOs.ItemFlag;
using LostAndFound.Infrastructure.DTOs.ItemMedia;
using LostAndFound.Infrastructure.DTOs.Location;
using LostAndFound.Infrastructure.DTOs.Media;
using LostAndFound.Infrastructure.DTOs.Notification;
using LostAndFound.Infrastructure.DTOs.Post;
using LostAndFound.Infrastructure.DTOs.PostBookmark;
using LostAndFound.Infrastructure.DTOs.PostFlag;
using LostAndFound.Infrastructure.DTOs.PostMedia;
using LostAndFound.Infrastructure.DTOs.Property;
using LostAndFound.Infrastructure.DTOs.Receipt;
using LostAndFound.Infrastructure.DTOs.Storage;
using LostAndFound.Infrastructure.DTOs.User;
using LostAndFound.Infrastructure.DTOs.UserDevice;
using LostAndFound.Infrastructure.DTOs.UserMedia;
using LostAndFound.Infrastructure.DTOs.Report;
using LostAndFound.Infrastructure.DTOs.ReportMedia;

namespace LostAndFound.Infrastructure.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Media Mapping
            CreateMap<Media, MediaReadDTO>();
            CreateMap<Media, MediaDetailReadDTO>();
            CreateMap<Media, MediaLiteReadDTO>();
            CreateMap<MediaUpdateWriteDTO, Media>();
            CreateMap<MediaWriteDTO, Media>();

            //Post Media
            CreateMap<PostMedia, PostMediaReadDTO>();
            CreateMap<PostMedia, PostMediaLiteReadDTO>();

            //Item Media
            CreateMap<ItemMedia, ItemMediaReadDTO>();
            CreateMap<ItemMedia, ItemMediaLiteReadDTO>();

            //Item Media
            CreateMap<ReportMedia, ReportMediaReadDTO>();

            //User Media Mapping
            CreateMap<UserMediaWriteDTO, UserMedia>();
            CreateMap<UserMedia, UserMediaReadDTO>();
            CreateMap<UserMedia, UserMediaLiteReadDTO>();

            //User Mapping
            CreateMap<User, UserReadDTO>();
            CreateMap<User, UserDetailsReadDTO>();
            CreateMap<UserWriteDTO, User>();
            CreateMap<User, UserDetailAuthenticateReadDTO>();
            CreateMap<UserUpdatePasswordDTO, User>();
            CreateMap<UserUpdateDTO, User>();
            CreateMap<User, UserBriefDetailDTO>();
            CreateMap<UserVerifyStatusUpdateDTO, User>();

            //Item Mapping
            CreateMap<Item, ItemReadDTO>();
            CreateMap<Item, ItemDetailWithFlagReadDTO>();
            CreateMap<Item, ItemDetailReadDTO>();
            CreateMap<ItemWriteDTO, Item>();
            CreateMap<ItemUpdateDTO, Item>();
            CreateMap<ItemUpdateWithoutCabinetIdDTO, Item>();
            CreateMap<Item, ItemReadWithClaimStatusDTO>();
            CreateMap<Item, ItemReadWithReceiptDTO>();

            //Category Mapping
            CreateMap<Category, CategoryReadDTO>();
            CreateMap<Category, CategoryReadLiteDTO>();
            CreateMap<CategoryWriteDTO, Category>();
            
            //Category Group Mapping
            CreateMap<CategoryGroup, CategoryGroupReadDTO>();
            CreateMap<CategoryGroup, CategoryGroupReadLiteDTO>();
            CreateMap<CategoryGroupWriteDTO, CategoryGroup>();
            
            //Property Mapping
            CreateMap<Campus, CampusReadDTO>();
            CreateMap<Campus, CampusLiteReadDTO>();
            CreateMap<CampusWriteDTO, Campus>();

            //Violation Report Mapping
            CreateMap<ReportWriteDTO, Report>();
            CreateMap<Report, ReportReadDTO>();
            
            //Post Mapping
            CreateMap<Post, PostReadDTO>();
            CreateMap<Post, PostDetailReadDTO>();
            CreateMap<Post, PostDetailWithCommentsReadDTO>();
            CreateMap<Post, PostDetailWithFlagReadDTO>();
            CreateMap<PostWriteDTO, Post>();
            CreateMap<PostUpdateDTO, Post>();
            CreateMap<PostStatusUpdateDTO, Post>();
            
            //Comment Mapping
            CreateMap<Comment, CommentReadDTO>();
            CreateMap<Comment, CommentDetailReadDTO>();
            CreateMap<Comment, CommentDetailWithReplyDetailReadDTO>();
            CreateMap<Comment, CommentDetailReadWithFlagDTO>();
            CreateMap<CommentWriteDTO, Comment>();
            CreateMap<CommentUpdateDTO, Comment>();
            
            //PostBookmark Mapping
            CreateMap<PostBookmark, PostBookmarkReadDTO>();
            
            //PostFlag Mapping
            CreateMap<PostFlag, PostFlagReadDTO>();
            
            //ItemFlag Mapping
            CreateMap<ItemFlag, ItemFlagReadDTO>();
            
            //ItemBookmark Mapping
            CreateMap<ItemBookmark, ItemBookmarkReadDTO>();
            
            //CommentFlag Mapping
            CreateMap<CommentFlag, CommentFlagReadDTO>();
            
            //Location Mapping
            CreateMap<Location, LocationReadDTO>();
            CreateMap<Location, LocationLiteReadDTO>();
            CreateMap<LocationWriteDTO, Location>();
            
            //Notification Mapping
            CreateMap<Notification, NotificationReadDTO>();
            CreateMap<NotificationWriteDTO, Notification>();
            
            //UserDevice Mapping
            CreateMap<UserDevice, UserDeviceReadDTO>();
            CreateMap<UserDeviceWriteDTO, UserDevice>();
            
            //Giveaway
            CreateMap<Giveaway, GiveawayReadDTO>();
            CreateMap<Giveaway, GiveawayDetailWithParticipantsReadDTO>();
            CreateMap<GiveawayWriteDTO, Giveaway>();
            CreateMap<GiveawayUpdateDTO, Giveaway>();
            
            //GiveawayParticipant
            CreateMap<GiveawayParticipant, GiveawayParticipantReadDTO>();

            //Claim
            CreateMap<ItemClaim, ItemClaimReadDTO>();
            CreateMap<ItemClaim, ItemClaimWithUserReadDTO>();
            CreateMap<ItemClaimWriteDTO, ItemClaim>();

            //Storage
            CreateMap<Storage, StorageReadDTO>();
            CreateMap<Storage, StorageReadIncludeCabinetsDTO>();
            CreateMap<StorageWriteDTO, Storage>();
            CreateMap<StorageUpdateDTO, Storage>();

            //Cabinet
            CreateMap<Cabinet, CabinetReadDTO>();
            CreateMap<Cabinet, CabinetWithoutItemReadDTO>();
            CreateMap<CabinetWriteDTO, Cabinet>();
            CreateMap<CabinetUpdateDTO, Cabinet>();

            //Receipt
            CreateMap<TransferRecord, TransferRecordReadDTO>();
            CreateMap<TransferRecordWriteDTO, TransferRecord>();
            CreateMap<TransferRecordGiveawayCreateDTO, TransferRecord>();
            CreateMap<TransferRecord, TransferRecordReadWithoutItemDTO>();
        }
    }
}
