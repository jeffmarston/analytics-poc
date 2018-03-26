import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AnalyticsComponent } from './analytics.component';
import { AnalyticsRoutingModule } from './analytics-routing.module';
import { AgGridModule } from "ag-grid-angular/main";

@NgModule({
  imports: [
    CommonModule,
    AnalyticsRoutingModule,
    AgGridModule.withComponents([])
  ],
  declarations: [AnalyticsComponent],
  providers: [],
})
export class AnalyticsModule { } 
