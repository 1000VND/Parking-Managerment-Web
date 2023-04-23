import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TableComponent } from './table.component';
import { NzTableModule } from 'ng-zorro-antd/table';
import { ColumnDirective } from './directives/column.directive';
import { CellDirective } from './directives/cell.directive';
import { HeaderDirective } from './directives/header.directive';

@NgModule({
  imports: [
    CommonModule,
    NzTableModule
  ],
  exports: [
    TableComponent,
    ColumnDirective

  ],
  declarations: [TableComponent, ColumnDirective, CellDirective, HeaderDirective]
})
export class TableModule { }
