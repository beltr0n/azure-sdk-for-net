// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

namespace Azure.Media.Analytics.Edge.Models
{
    /// <summary> Allowed states for a graph instance. </summary>
    public enum MediaGraphInstanceState
    {
        /// <summary> The media graph instance is idle and not processing media. </summary>
        Inactive,
        /// <summary> The media graph instance is transitioning into the active state. </summary>
        Activating,
        /// <summary> The media graph instance is active and processing media. </summary>
        Active,
        /// <summary> The media graph instance is transitioning into the inactive state. </summary>
        Deactivating
    }
}
