﻿namespace Mvc.Mailer
{
    using System.Collections.Generic;
    using System.Net.Mail;

    /// <summary>
    /// Declares the methods for creating LinkedResources
    /// </summary>
    public interface ILinkedResourceProvider
    {
        /// <summary>
        /// Gets a list of resources given their Id and FilePath
        /// </summary>
        IList<LinkedResource> GetAll(Dictionary<string, string> resources);

        /// <summary>
        /// Gets a linked resources given its Id and FilePath
        /// </summary>
        LinkedResource Get(string contentId, string filePath);
    }
}
