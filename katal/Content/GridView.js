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
        if (pageToolbar.GetItemByName("transferir") != null) {
            pageToolbar.GetItemByName("transferir").SetEnabled(enabled);
        }
        if (pageToolbar.GetItemByName("exportTxt") != null) {
            pageToolbar.GetItemByName("exportTxt").SetEnabled(enabled);
        }
        pageToolbar.GetItemByName("Edit").SetEnabled(gridView.GetFocusedRowIndex() !== -1);
    }
    function onPageToolbarItemClick(s, e) { 
        switch(e.item.name) {
            case "ToggleFilterPanel":
                toggleFilterPanel();
                break;
            case "New":
                console.log("dar");
                gridView.AddNewRow();
               
                break;
            case "Edit":
                
                gridView.StartEditRow(gridView.GetFocusedRowIndex());

                break;
            case "Delete":
                ExportSelectedRecords();
                break;
            case "Export":

                ExportSelectedRecords();
                //gridView.ExportTo(ASPxClientGridViewExportFormat.Xlsx);
                break;
            case "transferir":
                console.log("dar");
                TransferirRecords();
                break;
            case "exportTxt":
                console.log("dar");
                exportTxtRecords();
                break;

            case "contabilizar":
                console.log("dar");
                contabilizarMovCaja();
                break;
            case "anular":
             
                anularrMovCaja();
                break;
            case "imprimirCaja":
                console.log("dar");
                imprimirCaja();
                break;
        }
    }
    /**
     * para movimiento cajabanco
     * 
     * 
     * 
     * */
    function contabilizarMovCaja() {

        gridView.GetSelectedFieldValues("CB_C_SECUE", getSelectedontabilizar);

    }
    function getSelectedontabilizar(values) {
        console.log(selectdId);
        gridView.PerformCallback({ customAction: 'contabilizar', codigo: selectdId });
    }
    function anularrMovCaja() {

        gridView.GetSelectedFieldValues("CB_C_SECUE", getSelectedAnular);

    }
    function getSelectedAnular(values) {
        gridView.PerformCallback({ customAction: 'anular', codigo: selectdId });
    }
    function imprimirCaja() {

        gridView.GetSelectedFieldValues("CB_C_SECUE", getSelectedimprimirCaja);

    }
    function getSelectedimprimirCaja(values) {
        gridView.PerformCallback({ customAction: 'imprimir', codigo: selectdId });
    }


    /**
    * end para movimiento cajabanco
    *
    *
    *
    * */

    function TransferirRecords() {

        gridView.GetSelectedFieldValues("CNROITEM", getSelectedTransferir);

    }
    function getSelectedTransferir(values) {
        gridView.PerformCallback({ customAction: 'transferir', codigo: selectdId });
    }

    



    function ExportSelectedRecords() {
        
      gridView.GetSelectedFieldValues("OC_CNUMORD", getSelectedFieldValuesExportCallback);
        
    }
    function getSelectedFieldValuesExportCallback(values) {
        gridView.PerformCallback({ customAction: 'export', codigo: selectdId });
    }


    function deleteSelectedRecords() {
        if(confirm('Confirm Delete? ratitas')) {
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
            case "exportTxt":
                console.log("dar");
                exportTxtRecords();
                break;
            case "registar":
                console.log("da2222");
                registarRecords();
                break;
             
        }
    }

    function registarRecords() {

        gridView.GetSelectedFieldValues("codigo", getSelectedRegistarRecords);

    }
    function getSelectedRegistarRecords(values) {
        gridView.PerformCallback({ customAction: 'registar', codigo: selectdId });
    }

    function exportTxtRecords() {

        gridView.GetSelectedFieldValues("codigo", getSelectedexportTxt);

    }
    function getSelectedexportTxt(values) {
        gridView.PerformCallback({ customAction: 'exportTxt', codigo: selectdId });
    }

    function OnGridFocusedRowChangedComprobante(s,e) {
        s.GetRowValues(s.GetFocusedRowIndex(), 'OC_CNUMORD', OnGetRowValuesComprobante);
    }
    function OnGetRowValuesComprobante(values) {
        //DetailPhoto.SetImageUrl("@GridViewRowsDemosHelper.GetEmployeeImageRouteUrl()?@GridViewRowsDemosHelper.ImageQueryKey=" + values[0]);
        selectdId = values;
    }


    //
    function onPageToolbarItemClickRequision(s, e) {
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
                deleteSelectedRecordsRequision();
                break;
            case "Export":
                ExportSelectedRecordsRequision()
                break;
        }
    }


    //para eliminar elemnto
    function deleteSelectedRecordsRequision() {
        if (confirm('Confirma Delete?')) {
            gridView.GetSelectedFieldValues("NROREQUI", getSelectedFieldValuesCallbackRequision);
        }
    }
    function getSelectedFieldValuesCallbackRequision(values) {

        gridView.PerformCallback({ customAction: 'delete', codigo: selectdId });
    }
     //para eliminar elemnto
    //para imprimir elemnto
    function ExportSelectedRecordsRequision() {
       
        gridView.GetSelectedFieldValues("NROREQUI", getSelectedFieldValuesCallbackRequisionExport);
        
    }
    function getSelectedFieldValuesCallbackRequisionExport(values) {

        gridView.PerformCallback({ customAction: 'export', codigo: selectdId });
    }
     //para imprimir elemnto


    // para seleccionar elemento
    function OnGridFocusedRowChangedRequision(s, e) {
      
        if (s.keyName != "CB_C_SECUE") {
            s.GetRowValues(s.GetFocusedRowIndex(), 'NROREQUI', OnGetRowValuesRequision);
        } else {
            s.GetRowValues(s.GetFocusedRowIndex(), 'CB_C_SECUE', OnGetRowValuesRequision);
        }
      
    }
    function OnGetRowValuesRequision(values) {
        selectdId = values;
        selectdId = values;
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
    window.onPageToolbarItemClickComprobante = onPageToolbarItemClickComprobante;
    window.OnGridFocusedRowChangedComprobante = OnGridFocusedRowChangedComprobante;
    //para orden requisiones
    window.onPageToolbarItemClickRequision = onPageToolbarItemClickRequision;
    window.OnGridFocusedRowChangedRequision = OnGridFocusedRowChangedRequision;
    
})();