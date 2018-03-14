using System;
using System.Linq;
using Argotic.Extensions.Core;
using Argotic.Syndication;
using Firehose.Utils;

namespace Firehose.Core
{
    public class FeedItem
    {
        private readonly DateTime nullDate = new DateTime(1,1,1);
        
        
        public FeedItem(RssItem rss)
        {
            Title = rss.Title;
            PublishDate = GetDate_(rss);
            Author = GetAuthor_(rss);
            Description = rss.Description;
            (IsEnconded, Content) = GetContent_(rss);
            Link = rss.Link;
            Id = rss.Guid?.Value ?? rss.Link.ToString();
        }


        public FeedItem(AtomEntry entry)
        {
            Title = entry.Title.Content;
            PublishDate = entry.PublishedOn;
            Author = string.Join(", ", entry.Authors.Select(a => a.Name));
            Description = entry?.Summary?.Content ?? entry.Title.Content;
            Content = entry.Content.Content;
            Link = entry.BaseUri;
            Id = entry.Id.ToString();
        }
        
        
        public FeedItem(GenericSyndicationItem item)
        {
            Title = item.Title;
            PublishDate = item.PublishedOn;
            Description = item.Summary;
            Content = item.Summary;
        } 
        

        public string Author { get; }
        public string Title { get; }
        public DateTime PublishDate { get; }
        public string Description { get; }
        public string Content { get; }
        public Uri Link { get; }
        public string Id { get; }
        public bool IsEnconded { get; }


        private string GetAuthor_(RssItem item)
        {
            if (!item.Author.IsNullOrWhitespace() || !item.HasExtensions)
                return item.Author;
            var extension = item.FindExtension<DublinCoreElementSetSyndicationExtension>();
            if (extension == null)
                return item.Author;
            return extension.Context.Creator;
        }
        
        
        private DateTime GetDate_(RssItem item)
        {
            if (item.PublicationDate.Date != nullDate || !item.HasExtensions)
                return item.PublicationDate;
            var extension = item.FindExtension<DublinCoreElementSetSyndicationExtension>();
            if (extension == null)
                return item.PublicationDate;
            return extension.Context.Date;
        }


        private (bool,string) GetContent_(RssItem item)
        {
            if (!item.HasExtensions)
                return (false, item.Description);
            var extension = item.FindExtension<SiteSummaryContentSyndicationExtension>();
            if (extension == null || extension.Context.Encoded.IsNullOrWhitespace())
                return (false,item.Description);
            return (true, extension.Context.Encoded);
        }
        
    }
}