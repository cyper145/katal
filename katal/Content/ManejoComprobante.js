(function () {
    var selectedIds;
    let selectdId;
    function onGridViewInitComprobante(s, e) {
        AddAdjustmentDelegate(adjustGridViewComprobante);
        updateToolbarButtonsStateComprobante();
    }
    function onGridViewSelectionChangedComprobante(s, e) {
        updateToolbarButtonsStateComprobante();
    }
    function adjustGridViewComprobante() {
        gridView.AdjustControl();
    }
    function updateToolbarButtonsStateComprobante() {
        var enabled = gridView.GetSelectedRowCount() > 0;
        pageToolbar.GetItemByName("Delete").SetEnabled(enabled);
        pageToolbar.GetItemByName("Export").SetEnabled(enabled);
        pageToolbar.GetItemByName("Edit").SetEnabled(gridView.GetFocusedRowIndex() !== -1);
    }
    function onPageToolbarItemClickComprobante(s, e) {
        switch (e.item.name) {
            case "ToggleFilterPanel":
                toggleFilterPanel();
                break;
            case "New":

                gridView.AddNewRow();

                break;
            case "Edit":

                gridView.StartEditRow(gridView.GetFocusedRowIndex());

                break;
            case "Delete":
                deleteSelectedRecords();
                break;
            case "Export":

                ExportSelectedRecords()
                //gridView.ExportTo(ASPxClientGridViewExportFormat.Xlsx);
                break;
        }
    }
    function ExportSelectedRecords() {

        gridView.GetSelectedFieldValues("OC_CNUMORD", getSelectedFieldValuesExportCallback);

    }
    function getSelectedFieldValuesExportCallback(values) {
        gridView.PerformCallback({ customAction: 'export', codigo: selectdId });
    }
    function deleteSelectedRecords() {
        if (confirm('Confirm Delete?')) {
            gridView.GetSelectedFieldValues("OC_CNUMORD", getSelectedFieldValuesCallback);
        }
    }
    
    function toggleFilterPanel() {
        filterPanel.Toggle();
    }
    function onFilterPanelExpandedComprobante(s, e) {
        adjustPageControls();
        searchButtonEdit.SetFocus();
    }
    function onGridViewBeginCallbackComprobante(s, e) {
        e.customArgs['SelectedRows'] = selectedIds;

    }
    function getSelectedFieldValuesCallback(values) {

        gridView.PerformCallback({ customAction: 'delete', codigo: selectdId });
    }
    function OnGridFocusedRowChangedComprobante(s, e) {
        s.GetRowValues(s.GetFocusedRowIndex(), 'OC_CNUMORD', OnGetRowValuesComprobante);
    }
    function OnGetRowValuesComprobante(values) {
       
        selectdId = values;
    }
    window.onGridViewInitComprobante = onGridViewInitComprobante;
    window.onGridViewSelectionChangedComprobante = onGridViewSelectionChangedComprobante;
    window.onPageToolbarItemClickComprobante = onPageToolbarItemClickComprobante;
    window.onFilterPanelExpandedComprobante = onFilterPanelExpandedComprobante;
    window.onGridViewBeginCallbackComprobante = onGridViewBeginCallbackComprobante;
    window.OnGridFocusedRowChangedComprobante = OnGridFocusedRowChangedComprobante;
   

})();