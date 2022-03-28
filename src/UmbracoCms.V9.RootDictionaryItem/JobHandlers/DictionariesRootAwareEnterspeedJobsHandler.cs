using Enterspeed.Source.UmbracoCms.V9.Data.Models;
using Enterspeed.Source.UmbracoCms.V9.Data.Repositories;
using Enterspeed.Source.UmbracoCms.V9.Handlers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Enterspeed.Source.UmbracoCms.V9.Factories;
using Enterspeed.Source.UmbracoCms.V9.Services;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;
using UmbracoCms.V9.RootDictionaryItem;

namespace UmbracoCms.V9.RootDictionaryItem.JobHandlers
{
    public class DictionariesRootAwareEnterspeedJobsHandler : EnterspeedJobsHandler, IEnterspeedJobsHandler
    {
        private readonly ILocalizationService _localizationService;
        private readonly IEnterspeedConfigurationService _configuration;

        public DictionariesRootAwareEnterspeedJobsHandler(
            IEnterspeedJobRepository enterspeedJobRepository,
            ILogger<DictionariesRootAwareEnterspeedJobsHandler> logger,
            EnterspeedJobHandlerCollection jobHandlers,
            ILocalizationService localizationService,
            IEnterspeedJobFactory enterspeedJobFactory,
            IEnterspeedConfigurationService configuration)
            : base(enterspeedJobRepository, logger, jobHandlers, enterspeedJobFactory)
        {
            _localizationService = localizationService;
            _configuration = configuration;
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

            var languageIsoCodes = _localizationService.GetAllLanguages()
                .Select(s => s.IsoCode)
                .ToList();

            // Per configured destination, process separate jobs
            var stateConfigurations = new Dictionary<EnterspeedContentState, bool>()
            {
                { EnterspeedContentState.Preview, _configuration.IsPreviewConfigured() },
                { EnterspeedContentState.Publish, _configuration.IsPublishConfigured() },
            };

            foreach (var destination in stateConfigurations.Where(w => w.Value))
            {
                // Create job per culture of requested dictionary items
                var dictionaryItemsRootJobs = languageIsoCodes
                    .Select(isoCode => GetDictionaryItemsRootJob(isoCode, destination.Key))
                    .ToList();

                // Has to be handled in the end, when dictionary items are ingested
                base.HandleJobs(dictionaryItemsRootJobs);
            }
        }

        protected EnterspeedJob GetDictionaryItemsRootJob(string culture, EnterspeedContentState contentState)
        {
            return new EnterspeedJob
            {
                EntityId = DictionaryRootConstants.EntityId,
                EntityType = EnterspeedJobEntityType.Dictionary,
                Culture = culture,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                JobType = EnterspeedJobType.Publish,
                State = EnterspeedJobState.Processing,
                ContentState = contentState
            };
        }
    }
}