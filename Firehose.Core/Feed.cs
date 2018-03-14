using System;
using System.Linq;
using Argotic.Extensions;
using Argotic.Extensions.Core;
using Argotic.Syndication;

namespace Firehose.Core
{
    
    public enum FeedType {Rss, Atom, Unknown}
    
    public class Feed
    {
        public Feed(int id, string name, string address, FeedType feedType)
        {
            Id = id;
            Name = name;
            Address = address;
            FeedType = feedType;
        }


        public Feed(RssFeed feed)
        {
            Name = feed.Channel.Title;
            Description = feed.Channel.Description;
            Id = -1;
            Address = feed.Channel.Link.ToString();
            FeedType = FeedType.Rss;
            Frequency = GetFrequency_(feed);
        }
        
        
        public Feed(AtomFeed feed)
        {
            Name = feed.Title.ToString();
            Description = feed.Subtitle.ToString();
            Id = -1;
            Address = feed.BaseUri.ToString();
            FeedType = FeedType.Rss;
            Frequency = GetFrequency_(feed);
        }


        public Feed(GenericSyndicationFeed feed)
        {
            Name = feed.Title;
            Description = feed.Description;
            Id = -1;
            Address = "";
            FeedType = FeedType.Unknown;
            Frequency = new TimeSpan(1,0,0);
        }
        
        public string Name { get; }
        public string Description { get; }
        public int Id { get; }
        public string Address { get; }
        public FeedType FeedType { get; }
        public TimeSpan Frequency { get; }
        public DateTime LastUpdate { get; set; }


        private TimeSpan GetFrequency_(IExtensibleSyndicationObject feed)
        {
            var ext = feed.FindExtension(SiteSummaryUpdateSyndicationExtension.MatchByType) as SiteSummaryUpdateSyndicationExtension;
            int day = 0, hour = 0;
            switch (ext.Context.Period)
            {
               case SiteSummaryUpdatePeriod.Daily: day = ext.Context.Frequency; break;
               case SiteSummaryUpdatePeriod.Hourly: hour = ext.Context.Frequency; break;
            }

            return new TimeSpan(day, hour, 0, 0);
        }
    }
}