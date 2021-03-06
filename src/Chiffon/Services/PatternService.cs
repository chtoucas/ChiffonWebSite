﻿namespace Chiffon.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;

    using Chiffon.Entities;
    using Chiffon.Persistence;
    using Narvalo;

    public sealed class PatternService/*Impl*/ : IPatternService
    {
        private readonly IDbQueries _queries;

        public PatternService(IDbQueries queries)
        {
            Require.NotNull(queries, "queries");

            _queries = queries;
        }

        public IEnumerable<Pattern> GetPatternViews(
            DesignerKey designerKey, string categoryKey, string reference)
        {
            return _queries
                .ListPatterns(designerKey, categoryKey)
                .Where(_ => _.Reference == reference)
                .OrderByDescending(_ => _.Preferred)
                .ThenBy(_ => _.Variant);
        }

        public PreviewPagedList ListPreviews(
            DesignerKey designerKey,
            int pageIndex,
            int pageSize)
        {
            if (pageIndex < 1) { return null; }

            // On ne garde que les motifs ayant un aperçu dont on prend le préféré.
            var previews = _queries.ListPatterns(designerKey).Where(_ => _.HasPreview && _.Preferred);

            var pageCount = CountPage_(previews, pageSize);
            if (pageIndex > pageCount) { return null; }

            var slice = previews
                .OrderByDescending(_ => _.LastModifiedTime)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize);

            return new PreviewPagedList {
                PageCount = pageCount,
                PageIndex = pageIndex,
                Previews = slice
            };
        }

        public PreviewPagedList ListPreviews(
            DesignerKey designerKey,
            string categoryKey,
            int pageIndex,
            int pageSize)
        {
            if (pageIndex < 1) { return null; }

            // On ne garde que les motifs ayant un aperçu et on ne garde que le préféré.
            var previews = _queries.ListPatterns(designerKey, categoryKey).Where(_ => _.HasPreview && _.Preferred);

            var pageCount = CountPage_(previews, pageSize);
            if (pageIndex > pageCount) { return null; }

            var slice = previews
                .OrderBy(_ => _.Reference)
                .ThenByDescending(_ => _.LastModifiedTime)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize);

            return new PreviewPagedList {
                PageCount = pageCount,
                PageIndex = pageIndex,
                Previews = slice
            };
        }

        private static int CountPage_<T>(IEnumerable<T> @this, int pageSize)
        {
            Contract.Requires(@this != null);

            return (int)Math.Ceiling((decimal)@this.Count() / pageSize);
        }
    }
}