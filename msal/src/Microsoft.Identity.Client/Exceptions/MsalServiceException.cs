﻿//----------------------------------------------------------------------
//
// Copyright (c) Microsoft Corporation.
// All rights reserved.
//
// This code is licensed under the MIT License.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files(the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions :
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//------------------------------------------------------------------------------

using System;
using System.Globalization;

namespace Microsoft.Identity.Client
{
    /// <summary>
    /// Exception type thrown when service returns an error response or other networking errors occur.
    ///  For more details, see https://aka.ms/msal-net-exceptions
    /// </summary>
    public class MsalServiceException : MsalException
    {
        /// <summary>
        /// Service is unavailable and returned HTTP error code within the range of 500-599
        /// <para>Mitigation</para> you can retry after a delay. Note that the retry-after header is not yet
        /// surface in MSAL.NET (on the backlog)
        /// </summary>
        public const string ServiceNotAvailable = "service_not_available";

        /// <summary>
        /// The Http Request to the STS timed out.
        /// <para>Mitigation</para> you can retry after a delay.
        /// </summary>
        public const string RequestTimeout = "request_timeout";

        /// <summary>
        /// Upn required
        /// <para>What happens?</para> An override of a token acquisition operation was called in <see cref="T:PublicClientApplication"/> which
        /// takes a <c>loginHint</c> as a parameters, but this login hint was not using the UserPrincipalName (UPN) format, e.g. <c>john.doe@contoso.com</c> 
        /// expected by the service
        /// <para>Remediation</para> Make sure in your code that you enforce <c>loginHint</c> to be a UPN
        /// </summary>
        public const string UpnRequired = "upn_required";

        /// <summary>
        /// No passive auth endpoint was found in the OIDC configuration of the authority
        /// <para>What happens?</para> When the libraries goes to the authority and gets its open id connect configuration
        /// it expects to find a Passive Auth Endpoint entry, and could not find it.
        /// <para>remediation</para> Check that the authority configured for the application, or passed on some overrides of token acquisition tokens
        /// supporting authority override is correct
        /// </summary>
        public const string MissingPassiveAuthEndpoint = "missing_passive_auth_endpoint";

        /// <summary>
        /// Invalid authority
        /// <para>What happens</para> When the library attempts to discover the authority and get the endpoints it needs to
        /// acquire a token, it got an un-authorize HTTP code or an unexpected response
        /// <para>remediation</para> Check that the authority configured for the application, or passed on some overrides of token acquisition tokens
        /// supporting authority override is correct
        /// </summary>
        public const string InvalidAuthority = "invalid_authority";

        /// <summary>
        /// Initializes a new instance of the exception class with a specified
        /// error code, error message and a reference to the inner exception that is the cause of
        /// this exception.
        /// </summary>
        /// <param name="errorCode">
        /// The protocol error code returned by the service or generated by client. This is the code you
        /// can rely on for exception handling.
        /// </param>
        /// <param name="errorMessage">The error message that explains the reason for the exception.</param>
        public MsalServiceException(string errorCode, string errorMessage)
            : base(
                errorCode, errorMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of the exception class with a specified
        /// error code, error message and a reference to the inner exception that is the cause of
        /// this exception.
        /// </summary>
        /// <param name="errorCode">
        /// The protocol error code returned by the service or generated by client. This is the code you
        /// can rely on for exception handling.
        /// </param>
        /// <param name="errorMessage">The error message that explains the reason for the exception.</param>
        /// <param name="statusCode">Status code of the resposne received from the service.</param>
        public MsalServiceException(string errorCode, string errorMessage, int statusCode)
            : this(errorCode, errorMessage)
        {
            StatusCode = statusCode;
        }

        /// <summary>
        /// Initializes a new instance of the exception class with a specified
        /// error code, error message and a reference to the inner exception that is the cause of
        /// this exception.
        /// </summary>
        /// <param name="errorCode">
        /// The protocol error code returned by the service or generated by client. This is the code you
        /// can rely on for exception handling.
        /// </param>
        /// <param name="errorMessage">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null reference if no inner
        /// exception is specified.
        /// </param>
        public MsalServiceException(string errorCode, string errorMessage,
            Exception innerException)
            : base(
                errorCode, errorMessage, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the exception class with a specified
        /// error code, error message and a reference to the inner exception that is the cause of
        /// this exception.
        /// </summary>
        /// <param name="errorCode">
        /// The protocol error code returned by the service or generated by client. This is the code you
        /// can rely on for exception handling.
        /// </param>
        /// <param name="errorMessage">The error message that explains the reason for the exception.</param>
        /// <param name="statusCode">Status code of the resposne received from the service.</param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null reference if no inner
        /// exception is specified.
        /// </param>
        public MsalServiceException(string errorCode, string errorMessage, int statusCode,
            Exception innerException)
            : base(
                errorCode, errorMessage, innerException)
        {
            StatusCode = statusCode;
        }


        /// <summary>
        /// Initializes a new instance of the exception class with a specified
        /// error code, error message and a reference to the inner exception that is the cause of
        /// this exception.
        /// </summary>
        /// <param name="errorCode">
        /// The protocol error code returned by the service or generated by client. This is the code you
        /// can rely on for exception handling.
        /// </param>
        /// <param name="errorMessage">The error message that explains the reason for the exception.</param>
        /// <param name="statusCode">The status code of the request.</param>
        /// <param name="claims">The claims challenge returned back from the service.</param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null reference if no inner
        /// exception is specified.
        /// </param>
        public MsalServiceException(string errorCode, string errorMessage, int statusCode, string claims,
            Exception innerException)
            : this(
                errorCode, errorMessage, statusCode, innerException)
        {
            Claims = claims;
        }

        /// <summary>
        /// Gets the status code returned from http layer. This status code is either the <c>HttpStatusCode</c>  in the inner
        /// <see cref="T:System.Net.Http.HttpRequestException"/> response or the the NavigateError Event Status Code in a browser based flow (See
        /// http://msdn.microsoft.com/en-us/library/bb268233(v=vs.85).aspx).
        /// You can use this code for purposes such as implementing retry logic or error investigation.
        /// </summary>
        public int StatusCode { get; internal set; } = 0;

        /// <summary>
        /// Additional claims requested by the service. When this property is not null or empty, this means that the service requires the user to 
        /// provide additional claims, such as doing two factor authentication. The are two cases:
        /// <list type="bullent">
        /// <item>
        /// If your application is a <see cref="T:PublicClientApplication"/>, you should just call and <see cref="M:PublicClientApplication.AcquireTokenAsync"/>
        /// override of <see cref="T:PublicClientApplication"/> having an <c>extraQueryParameter</c> argument, and add the following string <c>$"claims={ex.Claims}"</c>
        /// to the extraQueryParameters, where ex is an instance of this exception.
        /// </item>
        /// <item>If you application is a <see cref="T:ConfidentialClientApplication"/>, (therefore doing the On behalf of flow), you should throw an Http unauthorize 
        /// exception wuth a message containing the claims</item>
        /// </list>
        /// For more details see https://aka.ms/msal-net-claim-challenge
        /// </summary>
        public string Claims { get; internal set; }
        
        /// <summary>
        /// Raw response body received from the server.
        /// </summary>
        public string ResponseBody { get; internal set; }

        /// <summary>
        /// Creates and returns a string representation of the current exception.
        /// </summary>
        /// <returns>A string representation of the current exception.</returns>
        public override string ToString()
        {
            return base.ToString() + string.Format(CultureInfo.InvariantCulture, "\n\tStatusCode: {0}\n\tClaims: {1}", StatusCode, Claims);
        }
    }
}