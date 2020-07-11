﻿//------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

namespace Microsoft.Azure.Cosmos.ChangeFeed.Utils
{
    using System;

    internal static class ResultSetIteratorUtils
    {
        public static ChangeFeedPartitionKeyResultSetIteratorCore BuildResultSetIterator(
            string partitionKeyRangeId,
            string continuationToken,
            int? maxItemCount,
            ContainerInternal container,
            DateTime? startTime,
            bool startFromBeginning)
        {
            ChangeFeedRequestOptions.StartFrom startFrom;
            if (continuationToken != null)
            {
                startFrom = ChangeFeedRequestOptions.StartFrom.CreateFromContinuation(continuationToken);
            }
            else if (startTime.HasValue)
            {
                startFrom = ChangeFeedRequestOptions.StartFrom.CreateFromTime(startTime.Value);
            }
            else if (startFromBeginning)
            {
                startFrom = ChangeFeedRequestOptions.StartFrom.CreateFromBeginning();
            }
            else
            {
                startFrom = ChangeFeedRequestOptions.StartFrom.CreateFromNow();
            }

            ChangeFeedRequestOptions requestOptions = new ChangeFeedRequestOptions()
            {
                MaxItemCount = maxItemCount,
                FeedRange = new FeedRangePartitionKeyRange(partitionKeyRangeId),
                From = startFrom,
            };

            return new ChangeFeedPartitionKeyResultSetIteratorCore(
                clientContext: container.ClientContext,
                container: container,
                options: requestOptions);
        }
    }
}
