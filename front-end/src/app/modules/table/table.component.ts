import { Component, ContentChildren, Input, OnInit, QueryList } from '@angular/core';
import { Dictionary } from '../../models/types';
import { ColumnDirective } from './directives/column.directive';

@Component({
  selector: 'parking-table',
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.css']
})
export class TableComponent implements OnInit {

  @Input() rows: Dictionary[]= [];  
  @Input() page = 1;
  @Input() pageSize = 2;
  @Input() totalRows = 0;

  @ContentChildren(ColumnDirective) columns!: QueryList<ColumnDirective>;
  
  constructor(){

  }

  ngOnInit(): void {
  }

}
