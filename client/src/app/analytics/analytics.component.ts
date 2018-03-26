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
    this.restapi.GetColumnsAsync().subscribe(columns => {
      columns.forEach(colName => {
        this.columnDefs.push({
          headerName: colName,
          field: colName,
          width: 100
        });
      });

      // this.rowData = [
      //   { Symbol: 5, Amount: 10 },
      //   { Symbol: 10, Amount: 15 },
      //   { Symbol: 15, Amount: 20 }
      // ];

      this.gridOptions.api.setColumnDefs(this.columnDefs);


      // SignalR
      this._signalR.connect().then((c) => {
        this.connection = c;

        let onAddRow$ = new BroadcastEventListener('addRow');
        this.connection.listen(onAddRow$);
        onAddRow$.subscribe((row) => this.addRowToGrid(row));

        let onDeleteRow$ = new BroadcastEventListener('deleteRow');
        this.connection.listen(onDeleteRow$);
        onDeleteRow$.subscribe((rowKey) => this.deleteRowFromGrid(rowKey));
      });
    });

  }

  private addRowToGrid(row) {
    let rowItem = {};
    for (let i = 0; i < this.columnDefs.length; i++) {
      let propname = this.columnDefs[i].field;
      rowItem[propname] = row[i];
    }

    var res = this.gridApi.updateRowData({
      add: [rowItem],
      addIndex: 0
    });
  }


  private deleteRowFromGrid(rowKey) {
    //var res = this.gridApi.removeItems();
  }

  onGridReady(params) {
    this.gridApi = params.api;
    params.api.sizeColumnsToFit();
  }

}
