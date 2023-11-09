using LostAndFound.Infrastructure.DTOs.Post;
using LostAndFound.Infrastructure.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace LostAndFound.API.Extensions
{
    public static class FacebookHelperExtensions
    {
        private static readonly string _baseUrl = "https://graph.facebook.com";
        public static async Task<string> CreatePost(FacebookCredentials facebookCredentials, IPostService postService, int postId)
        {

            var post = await postService.GetPostByIdAsync(postId);
            if(post.PostStatus != Core.Enums.PostStatus.ACTIVE)
            {
                return null;
            }

            //get message
            var message = "𝐓𝐢𝐭𝐥𝐞: " + post.Title + " \n" + "𝐂𝐨𝐧𝐭𝐞𝐧𝐭: " + post.PostContent + " \n";
            if(post.LocationLocationName != null)
            {
                message = message + " \n" + "𝐋𝐨𝐬𝐭 𝐋𝐨𝐜𝐚𝐭𝐢𝐨𝐧: " + post.LocationLocationName + " (" + post.Location.Property.Name + ")";
            }
            if (post.CategoryName != null)
            {
                message = message + " \n" + "𝐈𝐭𝐞𝐦 𝐂𝐚𝐭𝐞𝐠𝐨𝐫𝐲: " + post.CategoryName;
            }
            //add link to user web app (placeholder for now)
            message = message + " \n" + "𝐅𝐨𝐫 𝐦𝐨𝐫𝐞 𝐝𝐞𝐭𝐚𝐢𝐥: " + "www.google.com";


            if (post.PostMedias.Count > 0)
            {
                //get image url
                var url = post.PostMedias.First().Media.Url;

                using (var httpClient = new HttpClient())
                {
                    var request = new HttpRequestMessage
                    {
                        Method = new HttpMethod("POST"),
                        RequestUri = new Uri(_baseUrl + $"/{facebookCredentials.PageId}/photos?message={message}&access_token={facebookCredentials.AccessKey}&url={url}"),
                    };

                    HttpResponseMessage response = await httpClient.SendAsync(request);

                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();

                        return result;
                    }

                    return null;
                }
            }
            else
            {
                //get link (placeholder)
                var link = "www.google.com";

                using (var httpClient = new HttpClient())
                {
                    var request = new HttpRequestMessage
                    {
                        Method = new HttpMethod("POST"),
                        RequestUri = new Uri(_baseUrl + $"/{facebookCredentials.PageId}/feed?message={message}&access_token={facebookCredentials.AccessKey}&link={link}"),
                    };

                    HttpResponseMessage response = await httpClient.SendAsync(request);

                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();

                        return result;
                    }

                    return null;
                }
            }

        }
    }
}
