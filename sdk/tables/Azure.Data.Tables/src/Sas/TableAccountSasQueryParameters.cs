﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Azure.Data.Tables.Sas
{
    /// <summary>
    /// A <see cref="TableAccountSasQueryParameters"/> object represents the components
    /// that make up an Azure Storage Shared Access Signature's query
    /// parameters.  You can construct a new instance using
    /// <see cref="TableAccountSasBuilder"/>.
    ///
    /// For more information, <see href="https://docs.microsoft.com/en-us/rest/api/storageservices/create-account-sas">Create an account SAS</see>.
    /// </summary>
    public class TableAccountSasQueryParameters
    {
        // sv
        private readonly string _version;

        // srt
        private TableAccountSasResourceTypes? _resourceTypes;

        // spr
        private readonly TableSasProtocol _protocol;

        // st
        private DateTimeOffset _startTime;

        // se
        private DateTimeOffset _expiryTime;

        // sip
        private readonly TableSasIPRange _ipRange;

        // si
        private readonly string _identifier;

        // sr
        private readonly string _resource;

        // sp
        private readonly string _permissions;

        // sig
        private readonly string _signature;

        /// <summary>
        /// The default service version to use for Shared Access Signatures.
        /// </summary>
        internal const string DefaultSasVersion = TableConstants.Sas.DefaultSasVersion;

        /// <summary>
        /// Gets the storage service version to use to authenticate requests
        /// made with this shared access signature, and the service version to
        /// use when handling requests made with this shared access signature.
        /// </summary>
        public string Version => _version ?? TableConstants.Sas.DefaultSasVersion;

        /// <summary>
        /// Gets which resources are accessible via the shared access signature.
        /// </summary>
        public TableAccountSasResourceTypes? ResourceTypes => _resourceTypes;

        /// <summary>
        /// Optional. Specifies the protocol permitted for a request made with
        /// the shared access signature.
        /// </summary>
        public TableSasProtocol Protocol => _protocol;

        /// <summary>
        /// Gets the optional time at which the shared access signature becomes
        /// valid.  If omitted, start time for this call is assumed to be the
        /// time when the storage service receives the request.
        /// <see cref="DateTimeOffset.MinValue"/> means not set.
        /// </summary>
        public DateTimeOffset StartsOn => _startTime;

        /// <summary>
        /// Gets the time at which the shared access signature becomes invalid.
        /// <see cref="DateTimeOffset.MinValue"/> means not set.
        /// </summary>
        public DateTimeOffset ExpiresOn => _expiryTime;
        /// <summary>
        /// Gets the optional IP address or a range of IP addresses from which
        /// to accept requests.  When specifying a range, note that the range
        /// is inclusive.
        /// </summary>
        public TableSasIPRange IPRange => _ipRange;

        /// <summary>
        /// Gets the optional unique value up to 64 characters in length that
        /// correlates to an access policy specified for the blob container, queue,
        /// or share.
        /// </summary>
        public string Identifier => _identifier ?? string.Empty;

        /// <summary>
        /// Gets the resources are accessible via the shared access signature.
        /// </summary>
        public string Resource => _resource ?? string.Empty;

        /// <summary>
        /// Gets the permissions associated with the shared access signature.
        /// The user is restricted to operations allowed by the permissions.
        /// This field must be omitted if it has been specified in an
        /// associated stored access policy.
        /// </summary>
        public string Permissions => _permissions ?? string.Empty;

        /// <summary>
        /// The signature is an HMAC computed over the string-to-sign and key
        /// using the SHA256 algorithm, and then encoded using Base64 encoding.
        /// </summary>
        public string Signature => _signature ?? string.Empty;

        internal TableAccountSasQueryParameters()
            : base()
        {
        }

        /// <summary>
        /// Creates a new TableAccountSasQueryParameters instance.
        /// </summary>
        internal TableAccountSasQueryParameters(
            string version,
            TableAccountSasResourceTypes? resourceTypes,
            TableSasProtocol protocol,
            DateTimeOffset startsOn,
            DateTimeOffset expiresOn,
            TableSasIPRange ipRange,
            string identifier,
            string resource,
            string permissions,
            string signature)
        {
            _version = version;
            _resourceTypes = resourceTypes;
            _protocol = protocol;
            _startTime = startsOn;
            _expiryTime = expiresOn;
            _ipRange = ipRange;
            _identifier = identifier;
            _resource = resource;
            _permissions = permissions;
            _signature = signature;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="TableAccountSasQueryParameters"/>
        /// type based on the supplied query parameters <paramref name="values"/>.
        /// All SAS-related query parameters will be removed from
        /// <paramref name="values"/>.
        /// </summary>
        /// <param name="values">URI query parameters</param>
        internal TableAccountSasQueryParameters(
            IDictionary<string, string> values)
        {
            // make copy, otherwise we'll get an exception when we remove
            IEnumerable<KeyValuePair<string, string>> kvps = values.ToArray();
            foreach (KeyValuePair<string, string> kv in kvps)
            {
                // these are already decoded
                var isSasKey = true;
                switch (kv.Key.ToUpperInvariant())
                {
                    case TableConstants.Sas.Parameters.VersionUpper:
                        _version = kv.Value;
                        break;
                    case TableConstants.Sas.Parameters.ResourceTypesUpper:
                        _resourceTypes = TableSasExtensions.ParseResourceTypes(kv.Value);
                        break;
                    case TableConstants.Sas.Parameters.ProtocolUpper:
                        _protocol = TableSasExtensions.ParseProtocol(kv.Value);
                        break;
                    case TableConstants.Sas.Parameters.StartTimeUpper:
                        _startTime = DateTimeOffset.ParseExact(kv.Value, TableConstants.Sas.SasTimeFormat, CultureInfo.InvariantCulture);
                        break;
                    case TableConstants.Sas.Parameters.ExpiryTimeUpper:
                        _expiryTime = DateTimeOffset.ParseExact(kv.Value, TableConstants.Sas.SasTimeFormat, CultureInfo.InvariantCulture);
                        break;
                    case TableConstants.Sas.Parameters.IPRangeUpper:
                        _ipRange = TableSasIPRange.Parse(kv.Value);
                        break;
                    case TableConstants.Sas.Parameters.IdentifierUpper:
                        _identifier = kv.Value;
                        break;
                    case TableConstants.Sas.Parameters.ResourceUpper:
                        _resource = kv.Value;
                        break;
                    case TableConstants.Sas.Parameters.PermissionsUpper:
                        _permissions = kv.Value;
                        break;
                    case TableConstants.Sas.Parameters.SignatureUpper:
                        _signature = kv.Value;
                        break;

                    // We didn't recognize the query parameter
                    default:
                        isSasKey = false;
                        break;
                }

                // Remove the query parameter if it's part of the SAS
                if (isSasKey)
                {
                    values.Remove(kv.Key);
                }
            }
        }

        /// <summary>
        /// Convert the SAS query parameters into a URL encoded query string.
        /// </summary>
        /// <returns>
        /// A URL encoded query string representing the SAS.
        /// </returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            this.AppendProperties(sb);
            return sb.ToString();
        }
    }
}
