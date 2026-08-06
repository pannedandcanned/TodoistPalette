// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace TodoistPalette.Services
{
    /// <summary>
    /// Class for accessing API.
    /// </summary>
    internal sealed class ApiService
    {
        private string _accessToken;

        public ApiService(string initialToken = null)
        {
            _accessToken = initialToken;
        }



        public void SetToken(string token)
        {
            _accessToken = token;
        }

        public Task<string> GetAccessTokenAsync()
        {
            return Task.FromResult(_accessToken);
        }

        public HttpClient CreateAuthClient()
        {
            var client = new HttpClient();
            if (!string.IsNullOrEmpty(_accessToken))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
            }

            return client;
        }

        #region StatusCheck
        /// <summary>
        /// Status of token, need to implement connectivity checks for better troubleshooting
        /// </summary>
        public enum ApiStatus
        {
            Ready,
            NeedsConfiguration,
            Error
        }
        public ApiStatus GetStatus()
        {
            if (string.IsNullOrEmpty(_accessToken))
            {
                return ApiStatus.NeedsConfiguration;
            }

            return ApiStatus.Ready;
        }
        #endregion
    }
}