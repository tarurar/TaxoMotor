// <copyright file="TaxoMotorWebAppLevelCustomizations.EventReceiver.cs" company="Armd">
// Copyright Armd. All rights reserved.
// </copyright>
// <author>SPDEV\developer</author>
// <date>2014-08-12 18:34:41Z</date>
namespace TM.SP.Customizations
{
    using System;
    using System.Collections.Generic;
    using System.Security.Permissions;
    using System.Text;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Security;
    using Microsoft.SharePoint.Utilities;
    using Microsoft.SharePoint.Administration;

    /// <summary>
    /// TODO: Add comment to TaxoMotorWebAppLevelCustomizationsEventReceiver
    /// </summary>
    [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
    public class TaxoMotorWebAppLevelCustomizationsEventReceiver : SPFeatureReceiver
    {
        private static readonly string FeatureId = "{8f35872e-8f90-40b6-9253-d267c52a40c3}";

        private static string GetFeatureLocalizedResource(string resourceName, uint lang)
        {
            return SPUtility.GetLocalizedString(
                string.Format("$Resources:_FeatureId{0},{1}", FeatureId, resourceName), string.Empty, lang);
        }
        /// <summary>
        /// TODO: Add comment to describe the actions after feature activation
        /// </summary>
        /// <param name="properties">Properties of the feature</param>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPWebApplication webApp = properties.Feature.Parent as SPWebApplication;
                if (webApp != null)
                {
                    webApp.SuiteBarBrandingElementHtml = "<div class=\"ms-core-brandingText\">" + GetFeatureLocalizedResource("WebApplicationTitle", 1033) + "</div>";
                    webApp.Update();
                }
            });
        }

        /// <summary>
        /// TODO: Add comment to describe the actions during feature deactivation
        /// </summary>
        /// <param name="properties">Properties of the feature</param>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPWebApplication webApp = properties.Feature.Parent as SPWebApplication;
                if (webApp != null)
                {
                    webApp.SuiteBarBrandingElementHtml = "<div class=\"ms-core-brandingText\">SharePoint</div>";
                    webApp.Update();
                }
            });
        }

        /// <summary>
        /// TODO: Add comment to describe the actions after feature installation
        /// </summary>
        /// <param name="properties">Properties of the feature</param>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        {
            ////TODO: place receiver code here or remove method
        }

        /// <summary>
        /// TODO: Add comment to describe the actions during feature uninstalling
        /// </summary>
        /// <param name="properties">Properties of the feature</param>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        {
            ////TODO: place receiver code here or remove method
        }

        /// <summary>
        /// TODO: Add comment to describe the actions during feature upgrading
        /// </summary>
        /// <param name="properties">Properties of the feature</param>
        /// <param name="upgradeActionName">The name of the custom upgrade action to execute. The value can be null if the override of this method implements only one upgrade action.</param>
        /// <param name="parameters">Parameter names and values for the custom action</param>
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        {
            ////TODO: place receiver code here or remove method
        }
    }
}

