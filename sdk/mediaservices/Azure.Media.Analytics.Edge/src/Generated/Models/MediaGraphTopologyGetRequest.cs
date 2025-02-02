// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;

namespace Azure.Media.Analytics.Edge.Models
{
    /// <summary> The MediaGraphTopologyGetRequest. </summary>
    public partial class MediaGraphTopologyGetRequest : ItemNonSetRequestBase
    {
        /// <summary> Initializes a new instance of MediaGraphTopologyGetRequest. </summary>
        /// <param name="name"> method name. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="name"/> is null. </exception>
        public MediaGraphTopologyGetRequest(string name) : base(name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            MethodName = "GraphTopologyGet";
        }

        /// <summary> Initializes a new instance of MediaGraphTopologyGetRequest. </summary>
        /// <param name="methodName"> method name. </param>
        /// <param name="apiVersion"> api version. </param>
        /// <param name="name"> method name. </param>
        internal MediaGraphTopologyGetRequest(string methodName, string apiVersion, string name) : base(methodName, apiVersion, name)
        {
            MethodName = methodName ?? "GraphTopologyGet";
        }
    }
}
