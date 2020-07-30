using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using VimeoDotNet.Constants;
using VimeoDotNet.Models;
using VimeoDotNet.Parameters;

namespace VimeoDotNet
{
    public partial class VimeoClient
    {
        /// <inheritdoc />
        public async Task<Album> GetAlbumAsync(UserId userId, long albumId)
        {
            var request = _apiRequestFactory.AuthorizedRequest(
                AccessToken,
                HttpMethod.Get,
                userId.IsMe ? Endpoints.MeAlbum : Endpoints.UserAlbum,
                new Dictionary<string, string>
                {
                    {"userId", userId.ToString()},
                    {"albumId", albumId.ToString()}
                }
            );

            return await ExecuteApiRequest<Album>(request);
        }


        public async Task<Paginated<Album>> GetAlbumsAsync(UserId userId, string[] fields, GetAlbumsParameters parameters = null)
        {
            var request = _apiRequestFactory.AuthorizedRequest(
                AccessToken,
                HttpMethod.Get,
                userId.IsMe ? Endpoints.MeAlbums : Endpoints.UserAlbums,
                new Dictionary<string, string>
                {
                    {"userId", userId.ToString()},
                     {"fields", string.Join(",", fields)}


        },
                parameters
            );

            return await ExecuteApiRequest<Paginated<Album>>(request);
        }



        /// <inheritdoc />
        public async Task<Paginated<Album>> GetAlbumsAsync(UserId userId, GetAlbumsParameters parameters = null)
        {
            var request = _apiRequestFactory.AuthorizedRequest(
                AccessToken,
                HttpMethod.Get,
                userId.IsMe ? Endpoints.MeAlbums : Endpoints.UserAlbums,
                new Dictionary<string, string>
                {
                    {"userId", userId.ToString()}


        },
                parameters
            );

            return await ExecuteApiRequest<Paginated<Album>>(request);
        }

        /// <inheritdoc />
        public async Task<Album> CreateAlbumAsync(UserId userId, EditAlbumParameters parameters = null)
        {
            var request = _apiRequestFactory.AuthorizedRequest(
                AccessToken,
                HttpMethod.Post,
                Endpoints.GetCurrentUserEndpoint(Endpoints.UserAlbums),
                new Dictionary<string, string>
                {
                    

                       {"userId", userId.ToString()}
                },
                parameters
            );

            return await ExecuteApiRequest<Album>(request);
        }

        /// <inheritdoc />
        public async Task<Album> UpdateAlbumAsync(UserId userId, long albumId, EditAlbumParameters parameters = null)
        {
            var request = _apiRequestFactory.AuthorizedRequest(
                AccessToken,
                new HttpMethod("PATCH"),
                Endpoints.GetCurrentUserEndpoint(Endpoints.UserAlbum),
                new Dictionary<string, string>
                {
                    {"userId", userId.ToString()},
                    {"albumId", albumId.ToString()}
                },
                parameters
            );

            return await ExecuteApiRequest<Album>(request);
        }

        /// <inheritdoc />
        public async Task<bool> DeleteAlbumAsync(UserId userId, long albumId)
        {
            var request = _apiRequestFactory.AuthorizedRequest(
                AccessToken,
                HttpMethod.Delete,
                Endpoints.GetCurrentUserEndpoint(Endpoints.UserAlbum),
                new Dictionary<string, string>
                {
                    {"userId", userId.ToString()},
                    {"albumId", albumId.ToString()}
                }
            );

            return await ExecuteApiRequest(request);
        }

        /// <inheritdoc />
        public async Task<bool> AddToAlbumAsync(long albumId, AddToAlbumParameters parameters = null)
        {
            try
            {
                var request = _apiRequestFactory.AuthorizedRequest(
                    AccessToken,
                    HttpMethod.Put,
                    Endpoints.UserAlbumVideo,
                    new Dictionary<string, string>
                    {

                    {"albumId", albumId.ToString()},

                    }, parameters
                );
                return await ExecuteApiRequest(request);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }

           
        }

        /// <inheritdoc />
        public async Task<bool> RemoveFromAlbumAsync( long albumId, long clipId)
        {
            var request = _apiRequestFactory.AuthorizedRequest(
                AccessToken,
                HttpMethod.Delete,
                Endpoints.GetCurrentUserEndpoint(Endpoints.UserAlbumVideo),
                new Dictionary<string, string>
                {
                    {"albumId", albumId.ToString()},
                    {"videoId", clipId.ToString()}
                }
            );

            return await ExecuteApiRequest(request);
        }
    }
}