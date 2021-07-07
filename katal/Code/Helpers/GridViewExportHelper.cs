using DevExpress.Web.Mvc;
using DevExpress.XtraPrinting;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace katal.Code.Helpers
{
    public delegate ActionResult GridViewExportMethod(GridViewSettings settings, object dataObject);
    public class GridViewExportDemoHelper
    {
        static Dictionary<string, GridViewExportMethod> exportFormatsInfo;
        public static Dictionary<string, GridViewExportMethod> ExportFormatsInfo
        {
            get
            {
                if (exportFormatsInfo == null)
                    exportFormatsInfo = CreateExportFormatsInfo();
                return exportFormatsInfo;
            }
        }
        static Dictionary<string, GridViewExportMethod> CreateExportFormatsInfo()
        {
            return new Dictionary<string, GridViewExportMethod> {
                {
                    "CustomExportToXLS",
                    (settings, data) => GridViewExtension.ExportToXls(settings, data, new XlsExportOptionsEx { ExportType = DevExpress.Export.ExportType.WYSIWYG })
                },
                {
                    "CustomExportToXLSX",
                    (settings, data) => GridViewExtension.ExportToXlsx(settings, data, new XlsxExportOptionsEx { ExportType = DevExpress.Export.ExportType.WYSIWYG })
                }
            };
        }
        public static GridViewSettings CreateGeneralDetailGridSettings(int uniqueKey)
        {
            GridViewSettings settings = new GridViewSettings();
            settings.Name = "detailGrid" + uniqueKey;
            settings.Width = Unit.Percentage(100);

            settings.KeyFieldName = "ProductID";
            settings.Columns.Add("ProductID");
            settings.Columns.Add("ProductName");
            settings.Columns.Add("UnitPrice");
            settings.Columns.Add("QuantityPerUnit");

            settings.SettingsDetail.MasterGridName = "masterGrid";
            settings.Styles.Header.Wrap = DevExpress.Utils.DefaultBoolean.True;

            return settings;
        }
    }
}