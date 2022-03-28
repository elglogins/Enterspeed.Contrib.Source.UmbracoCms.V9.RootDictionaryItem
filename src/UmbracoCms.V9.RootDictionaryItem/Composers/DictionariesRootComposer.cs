using Enterspeed.Source.UmbracoCms.V9.Composers;
using Enterspeed.Source.UmbracoCms.V9.DataPropertyValueConverters;
using Enterspeed.Source.UmbracoCms.V9.Handlers;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Enterspeed.Source.UmbracoCms.V9.Handlers.Dictionaries;
using Enterspeed.Source.UmbracoCms.V9.Handlers.PreviewDictionaries;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using UmbracoCms.V9.RootDictionaryItem.JobHandlers;

namespace UmbracoCms.V9.RootDictionaryItem.Composers
{
    [ComposeAfter(typeof(EnterspeedComposer))]
    public class DictionariesRootComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            // Remove default jobs handler
            var enterspeedJobsHandlerDescriptor = builder.Services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(IEnterspeedJobsHandler));
            builder.Services.Remove(enterspeedJobsHandlerDescriptor);

            // Register own dictionaries root aware jobs handler
            builder.Services.AddTransient<IEnterspeedJobsHandler, DictionariesRootAwareEnterspeedJobsHandler>();

            // Remove default published dictionary items job handler
            builder.EnterspeedJobHandlers()
                .Remove<EnterspeedDictionaryItemPublishJobHandler>()
                .Remove<EnterspeedPreviewDictionaryItemPublishJobHandler>();

            // Register own dictionary job handlers
            builder.EnterspeedJobHandlers()
                // Preview
                .Append<JobHandlers.Dictionaries.Preview.NonRootPublishedDictionaryItemJobHandler>()
                .Append<JobHandlers.Dictionaries.Preview.RootPublishedDictionaryItemJobHandler>()
                // Publish
                .Append<JobHandlers.Dictionaries.Publish.NonRootPublishedDictionaryItemJobHandler>()
                .Append<JobHandlers.Dictionaries.Publish.RootPublishedDictionaryItemJobHandler>();
        }
    }
}
