using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Navigation;

namespace TM.SP.Customizations.Features.TaxoMotor_TaxoMotorCustomizations
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("f000cffa-dcd8-4933-b751-d46d4dbab6a9")]
    public class TaxoMotor_TaxoMotorCustomizationsEventReceiver : SPFeatureReceiver
    {
        private SPDiagnosticsService diagService;
        private static readonly string FeatureId = "{71741ffe-7337-47c5-949a-d68dedc69845}";

        private static string GetFeatureLocalizedResource(string resourceName, uint lang)
        {
            return SPUtility.GetLocalizedString(
                string.Format("$Resources:_FeatureId{0},{1}", FeatureId, resourceName), string.Empty, lang);
        }

        public TaxoMotor_TaxoMotorCustomizationsEventReceiver()
        {
            this.diagService = SPDiagnosticsService.Local;
        }

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    SPWeb web = properties.Feature.Parent as SPWeb;
                    if (web != null)
                    {
                        web.SiteLogoUrl = "/_layouts/15/TM.SP.Customizations/Images/taxi_2013_logo.png";
                        web.Title = GetFeatureLocalizedResource("SiteTitle", web.Language);
                        // add navigation items
                        // TODO: move to another feature which deploys pages for every section
                        web.Navigation.TopNavigationBar.AddAsLast(
                            new SPNavigationNode(GetFeatureLocalizedResource("TopNavBarRequestLink", web.Language), "/"));
                        web.Navigation.TopNavigationBar.AddAsLast(
                            new SPNavigationNode(GetFeatureLocalizedResource("TopNavBarClearanceLink", web.Language), "/"));
                        web.Navigation.TopNavigationBar.AddAsLast(
                            new SPNavigationNode(GetFeatureLocalizedResource("TopNavBarBooksLink", web.Language), "/"));
                        web.Navigation.TopNavigationBar.AddAsLast(
                            new SPNavigationNode(GetFeatureLocalizedResource("TopNavBarMADILink", web.Language), "/"));
                        web.Navigation.TopNavigationBar.AddAsLast(
                            new SPNavigationNode(GetFeatureLocalizedResource("TopNavBarReportsLink", web.Language), "/"));
                        web.Update();
                    }
                });
            }
            catch (Exception ex)
            {
                SPWeb web = properties.Feature.Parent as SPWeb;
                string traceCategory = SPUtility.GetLocalizedString(
                    string.Format("$Resources:_FeatureId{0},{1}", FeatureId, "TraceEventLogCategory"), string.Empty, web != null ? web.Language : 1033);
                string errMessage = SPUtility.GetLocalizedString(
                    string.Format("$Resources:_FeatureId{0},{1}", FeatureId, "resFeatureActivationErrFmt"), string.Empty, web != null ? web.Language : 1033);
                
                this.diagService.WriteTrace(0,
                        new SPDiagnosticsCategory(
                            traceCategory,
                            TraceSeverity.Unexpected,
                            EventSeverity.Error),
                        TraceSeverity.Unexpected,
                        errMessage,
                        new object[] {ex});
                throw;
            }
        }


        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    SPWeb web = properties.Feature.Parent as SPWeb;
                    if (web != null)
                    {
                        web.SiteLogoUrl = "/_layouts/15/Images/siteicon.png";
                        web.Title = SPUtility.GetLocalizedString(
                            string.Format("$Resources:_FeatureId{0},{1}", FeatureId, "DefaultSiteTitle"), string.Empty, web.Language);
                        web.Update();
                    }
                });
            }
            catch (Exception ex)
            {
                SPWeb web = properties.Feature.Parent as SPWeb;
                string traceCategory = SPUtility.GetLocalizedString(
                    string.Format("$Resources:_FeatureId{0},{1}", FeatureId, "TraceEventLogCategory"), string.Empty, web != null ? web.Language : 1033);
                string errMessage = SPUtility.GetLocalizedString(
                    string.Format("$Resources:_FeatureId{0},{1}", FeatureId, "resFeatureDeactivationErrFmt"), string.Empty, web != null ? web.Language : 1033);

                this.diagService.WriteTrace(0,
                        new SPDiagnosticsCategory(
                            traceCategory,
                            TraceSeverity.Unexpected,
                            EventSeverity.Error),
                        TraceSeverity.Unexpected,
                        errMessage,
                        new object[] { ex });
                throw;
            }
        }


        // Uncomment the method below to handle the event raised after a feature has been installed.

        //public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised before a feature is uninstalled.

        //public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        //{
        //}

        // Uncomment the method below to handle the event raised when a feature is upgrading.

        //public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        //{
        //}
    }
}
