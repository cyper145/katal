using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevExpress.Web.Mvc;
using katal.Model;

namespace katal.Code.Helpers
{
    public class GridViewToolbarHelper
    {
        public const string KeyFieldName = "ProductID";

        static MVCxGridViewColumnCollection exportedColumns;
        public static MVCxGridViewColumnCollection ExportedColumns
        {
            get
            {
                if (exportedColumns == null)
                    exportedColumns = CreateExportedColumns();
                return exportedColumns;
            }
        }

        static GridViewSettings exportGridSettings;
        public static GridViewSettings ExportGridSettings
        {
            get
            {
                if (exportGridSettings == null)
                    exportGridSettings = CreateExportGridSettings();
                return exportGridSettings;
            }
        }

        static MVCxGridViewColumnCollection CreateExportedColumns()
        {
            var columns = new MVCxGridViewColumnCollection();
            columns.Add("ProductName");
            columns.Add(c => {
                c.FieldName = "CategoryID";
                c.Caption = "Category";

                c.EditorProperties().ComboBox(p => {
                    p.TextField = "CategoryName";
                    p.ValueField = "CategoryID";
                    p.ValueType = typeof(int);
                    p.BindList(GridViewHelper.getArticulos());
                });
            });
            columns.Add("QuantityPerUnit");
            columns.Add(c => {
                c.FieldName = "UnitPrice";
                c.EditorProperties().SpinEdit(p => {
                    p.DisplayFormatString = "c";
                    p.DisplayFormatInEditMode = true;
                });
            });
            columns.Add("UnitsInStock", MVCxGridViewColumnType.SpinEdit);
            columns.Add("Discontinued", MVCxGridViewColumnType.CheckBox);
            return columns;
        }

        static GridViewSettings CreateExportGridSettings()
        {
            var settings = new GridViewSettings();
            settings.Name = "grid";
            settings.KeyFieldName = KeyFieldName;
            settings.Columns.Assign(ExportedColumns);
            return settings;
        }

    }
}