(function() {
    var selectedIds;
    let selectdId;
    function onGridViewInit(s, e) {
        AddAdjustmentDelegate(adjustGridView);
        updateToolbarButtonsState();
    }
    function onGridViewSelectionChanged(s, e) {
        updateToolbarButtonsState();
    }
    function adjustGridView() {
        gridView.AdjustControl();
    }
    function updateToolbarButtonsState() {
        var enabled = gridView.GetSelectedRowCount() > 0;
        pageToolbar.GetItemByName("Delete").SetEnabled(enabled);
        pageToolbar.GetItemByName("Export").SetEnabled(enabled);

        pageToolbar.GetItemByName("Edit").SetEnabled(gridView.GetFocusedRowIndex() !== -1);
    }
    function onPageToolbarItemClick(s, e) {
        switch(e.item.name) {
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
        if(confirm('Confirm Delete?')) {
            gridView.GetSelectedFieldValues("OC_CNUMORD", getSelectedFieldValuesCallback);
        }
    }
    function onFiltersNavBarItemClick(s, e) {
        var filters = {
            All: "",
            Active: "[Status] = 1",
            Bugs: "[Kind] = 1",
            Suggestions: "[Kind] = 2",
            HighPriority: "[Priority] = 1"
        };
        gridView.ApplyFilter(filters[e.item.name]);
        HideLeftPanelIfRequired();
    }

    function toggleFilterPanel() {
        filterPanel.Toggle();
    }

    function onFilterPanelExpanded(s, e) {
        adjustPageControls();
        searchButtonEdit.SetFocus();
    }

    function onGridViewBeginCallback(s, e) {
        e.customArgs['SelectedRows'] = selectedIds;
        
    }


    function getSelectedFieldValuesCallback(values) {
              
        gridView.PerformCallback({ customAction: 'delete', codigo: selectdId});
    }
    function OnGridFocusedRowChangedOrdenCompra(s, e) {
        s.GetRowValues(s.GetFocusedRowIndex(), 'OC_CNUMORD', OnGetRowValuesOrdenCompra);
    }
    function OnGetRowValuesOrdenCompra(values) {
        //DetailPhoto.SetImageUrl("@GridViewRowsDemosHelper.GetEmployeeImageRouteUrl()?@GridViewRowsDemosHelper.ImageQueryKey=" + values[0]);
        selectdId = values;   
    }



    // para orden de compra detalles
    function onGridViewInitDetalles(s, e) {
        AddAdjustmentDelegate(adjustGridViewDetalles);
        updateToolbarButtonsStateDetalles();
    }
    function onGridViewSelectionChangedDetalles(s, e) {
        updateToolbarButtonsStateDetalles();
    }
    function adjustGridViewDetalles() {
        gridViewd.AdjustControl();
    }
    function updateToolbarButtonsStateDetalles(){
        var enabled = gridView.GetSelectedRowCount() > 0;
        pageToolbarOrdenes.GetItemByName("DeleteDetalles").SetEnabled(enabled);
        pageToolbarOrdenes.GetItemByName("EditDetalles").SetEnabled(gridViewd.GetFocusedRowIndex() !== -1);
    }



    function onPageToolbarItemClickDetalles(s, e) {
        switch (e.item.name) {
            case "NewDetalles":
                console.log("holas");
                gridViewd.AddNewRow();
                break;
            case "EditDetalles":
                gridViewd.StartEditRow(gridViewd.GetFocusedRowIndex());
                break;
            case "DeleteDetalles":
                deleteSelectedRecordsDetalles();
                break;
        }
    }
    function deleteSelectedRecordsDetalles() {
        if (confirm('Confirm Delete?')) {
            gridViewd.GetSelectedFieldValues("Codigo", getSelectedFieldValuesCallbackDetalles);
        }
    }
    function getSelectedFieldValuesCallbackDetalles(values) {
        selectedIds = values.join(',');
        gridView.PerformCallback({ customAction: 'delete' });
    }
    function onGridViewBeginCallbackDetalles(s, e) {
        e.customArgs['SelectedRows'] = selectedIds;

    }

    function OnToolbarItemClick(s, e) {
        if (!IsCustomExportToolbarCommand(e.item.name))
            return;
        var $exportFormat = $('#customExportCommand');
        $exportFormat.val(e.item.name);
        $('form').submit();
        window.setTimeout(function () {
            $exportFormat.val("");
        }, 0);
    }

    function IsCustomExportToolbarCommand(command) {
        return command == "CustomExportToXLS" || command == "CustomExportToXLSX";
    }

    window.OnToolbarItemClick = OnToolbarItemClick

    window.onPageToolbarItemClickDetalles = onPageToolbarItemClickDetalles;

    window.onGridViewBeginCallback = onGridViewBeginCallback;
    window.onGridViewBeginCallbackDetalles = onGridViewBeginCallbackDetalles;
    window.onGridViewInit = onGridViewInit;
    window.onGridViewSelectionChanged = onGridViewSelectionChanged;
    window.onPageToolbarItemClick = onPageToolbarItemClick;
    window.onFilterPanelExpanded = onFilterPanelExpanded;
    window.onFiltersNavBarItemClick = onFiltersNavBarItemClick;

    window.onGridViewInitDetalles = onGridViewInitDetalles;
    window.onGridViewSelectionChangedDetalles = onGridViewSelectionChangedDetalles;
    window.OnGridFocusedRowChangedOrdenCompra = OnGridFocusedRowChangedOrdenCompra;
    
})();