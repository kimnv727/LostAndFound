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

            if (post.PostLocationList != null)
            {
                message = message + " \n" + "𝐋𝐨𝐬𝐭 𝐋𝐨𝐜𝐚𝐭𝐢𝐨𝐧: ";
                foreach(var l in post.PostLocationList)
                {
                    message = message + l.LocationName + ", ";
                }
                message = message.Substring(0, message.Length - 2);
                message = message + " (" + post.User.CampusName + ")";
            }
            if (post.PostCategoryList != null)
            {
                message = message + " \n" + "𝐈𝐭𝐞𝐦 𝐂𝐚𝐭𝐞𝐠𝐨𝐫𝐲: ";
                foreach (var c in post.PostCategoryList)
                {
                    message = message + c.Name + ", ";
                }
                message = message.Substring(0, message.Length - 2);
            }
            message = message + " \n" + "𝐅𝐨𝐫 𝐦𝐨𝐫𝐞 𝐝𝐞𝐭𝐚𝐢𝐥: " + "https://lnf-user.web.app/";

            string privacyValue = "EVERYONE";
            if (post.PostMedias.Count > 0)
            {
                //get image url
                var url = post.PostMedias.First().Media.Url;

                using (var httpClient = new HttpClient())
                {
                    var request = new HttpRequestMessage
                    {
                        Method = new HttpMethod("POST"),
                        RequestUri = new Uri(_baseUrl + $"/{facebookCredentials.PageId}/photos?message={message}&access_token={facebookCredentials.AccessKey}&url={url}&privacy={{\"value\":\"{privacyValue}\"}}"),
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
                //get link
                var link = "https://lnf-user.web.app/";

                using (var httpClient = new HttpClient())
                {
                    var request = new HttpRequestMessage
                    {
                        Method = new HttpMethod("POST"),
                        RequestUri = new Uri(_baseUrl + $"/{facebookCredentials.PageId}/feed?message={message}&access_token={facebookCredentials.AccessKey}&privacy={{\"value\":\"{privacyValue}\"}}"),
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
