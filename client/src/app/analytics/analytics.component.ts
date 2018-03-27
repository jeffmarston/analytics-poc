import { Component, OnInit } from '@angular/core';
import { GridOptions, GridApi } from "ag-grid";
import { RestapiService } from '../services/restapi.service'
import { SignalR, SignalRConnection, BroadcastEventListener } from 'ng2-signalr';
import _ from 'lodash'

@Component({
  selector: 'app-analytics',
  templateUrl: './analytics.component.html',
  styleUrls: ['./analytics.component.scss']
})
export class AnalyticsComponent implements OnInit {
  private gridOptions: GridOptions;
  private gridApi: GridApi;
  private rowData: any;
  private columnDefs: any[];
  private pageTitle: string;
  private messages: string[] = [];
  private connectionState: any;
  private connection: any;

  constructor(
    private restapi: RestapiService,
    private _signalR: SignalR
  ) {
    this.gridOptions = <GridOptions>{};
  }

  ngOnInit() {
    this.pageTitle = "By Security";

    // Rest
    this.columnDefs = [];
    this.rowData = [];
    this.restapi.GetViewAsync().subscribe(view => {

      view.columns.forEach(colName => {
        this.columnDefs.push({
          headerName: colName,
          field: colName,
          width: 100,
          filter: "agTextColumnFilter"
        });
      });

      view.rows.forEach(row => {
        this.addRowToGrid(row);        
      });

      this.gridOptions.api.setColumnDefs(this.columnDefs);


      // SignalR
      this._signalR.connect().then((c) => {
        this.connection = c;

        let onAddRow$ = new BroadcastEventListener('addRow');
        this.connection.listen(onAddRow$);
        onAddRow$.subscribe((row) => this.addRowToGrid(row));

        let onDeleteRow$ = new BroadcastEventListener('deleteRow');
        this.connection.listen(onDeleteRow$);
        onDeleteRow$.subscribe((row) => this.deleteRowFromGrid(row));

        let onUpdateRow$ = new BroadcastEventListener('updateRow');
        this.connection.listen(onUpdateRow$);
        onUpdateRow$.subscribe((row) => this.updateRowInGrid(row));
      });
    });

  }

  private addRowToGrid(row) {
    let rowItem = { rowKey: row.key };
    for (let i = 0; i < this.columnDefs.length; i++) {
      let propname = this.columnDefs[i].field;
      rowItem[propname] = row.values[i];
    }

    var res = this.gridApi.updateRowData({
      add: [rowItem],
      addIndex: 0
    });
  }


  private updateRowInGrid(newRow) {
    var itemsToUpdate = [];
    this.gridOptions.api.forEachNodeAfterFilterAndSort((rowNode) => {
      if (rowNode.data.rowKey == newRow.key) {
        
        // Replace current values with new ones
        for (let i = 0; i < this.columnDefs.length; i++) {
          let propname = this.columnDefs[i].field;
          rowNode.data[propname] = newRow.values[i];
        }
        itemsToUpdate.push(rowNode.data);
      }
    });
    var res = this.gridOptions.api.updateRowData({ update: itemsToUpdate });
  }

  private deleteRowFromGrid(row) {
    var itemsToDelete = [];
    this.gridOptions.api.forEachNodeAfterFilterAndSort((rowNode) => {
      if (rowNode.data.rowKey == row.key) {
        itemsToDelete.push(rowNode.data);
      }
    });
    var res = this.gridOptions.api.updateRowData({ remove: itemsToDelete });
  }

  onGridReady(params) {
    this.gridApi = params.api;
    params.api.sizeColumnsToFit();
  }

}
