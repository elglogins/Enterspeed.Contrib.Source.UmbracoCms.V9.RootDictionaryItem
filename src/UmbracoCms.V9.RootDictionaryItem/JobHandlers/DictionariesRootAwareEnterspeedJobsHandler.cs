using Enterspeed.Source.UmbracoCms.V9.Data.Models;
using Enterspeed.Source.UmbracoCms.V9.Data.Repositories;
using Enterspeed.Source.UmbracoCms.V9.Handlers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Services;
using UmbracoCms.V9.RootDictionaryItem;

namespace UmbracoCms.V9.RootDictionaryItem.JobHandlers
{
    public class DictionariesRootAwareEnterspeedJobsHandler : EnterspeedJobsHandler, IEnterspeedJobsHandler
    {
        private readonly ILocalizationService _localizationService;

        public DictionariesRootAwareEnterspeedJobsHandler(
            IEnterspeedJobRepository enterspeedJobRepository,
            ILogger<EnterspeedJobsHandler> logger,
            EnterspeedJobHandlerCollection jobHandlers,
            ILocalizationService localizationService)
            : base(enterspeedJobRepository, logger, jobHandlers)
        {
            _localizationService = localizationService;
        }

        public new void HandleJobs(IList<EnterspeedJob> jobs)
        {
            // Perform default handling
            base.HandleJobs(jobs);

            // If any dictionary items were handled, push dictionaries root entity
            if (!jobs.Any(a => a.EntityType == EnterspeedJobEntityType.Dictionary))
            {
                return;
            }

            // Create job per culture of requested dictionary items
            var dictionaryItemsRootJobs = _localizationService.GetAllLanguages()
                .Select(s => s.IsoCode)
                .Select(GetDictionaryItemsRootJob)
                .ToList();

            // Has to be handled in the end, when dictionary items are ingested
            base.HandleJobs(dictionaryItemsRootJobs);
        }

        protected EnterspeedJob GetDictionaryItemsRootJob(string culture)
        {
            return new EnterspeedJob
            {
                EntityId = DictionaryRootConstants.EntityId,
                EntityType = EnterspeedJobEntityType.Dictionary,
                Culture = culture,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                JobType = EnterspeedJobType.Publish,
                State = EnterspeedJobState.Processing
            };
        }
    }
}
