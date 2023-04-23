import { Directive, TemplateRef, ViewChild } from '@angular/core';

@Directive({
  selector: 'parking-cell'
})
export class CellDirective {
  constructor(
    public template: TemplateRef<any>
  ) { }

}
