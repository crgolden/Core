﻿namespace Core.Filters
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    /// <inheritdoc />
    [PublicAPI]
    public class ModelStatePageFilter : IPageFilter
    {
        /// <inheritdoc />
        [SuppressMessage("Critical Code Smell", "S1186:Methods should not be empty", Justification = "Not implemented")]
        public void OnPageHandlerSelected(PageHandlerSelectedContext context)
        {
        }

        /// <inheritdoc />
        public void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            if (context == default)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.ModelState.IsValid)
            {
                return;
            }

            context.Result = new BadRequestObjectResult(context.ModelState);
        }

        /// <inheritdoc />
        [SuppressMessage("Critical Code Smell", "S1186:Methods should not be empty", Justification = "Not implemented")]
        public void OnPageHandlerExecuted(PageHandlerExecutedContext context)
        {
        }
    }
}
