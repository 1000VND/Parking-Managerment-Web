import { Component, ViewChild } from '@angular/core';
import { LoadingComponent } from '../common/loading/loading.component';
import { TicketService } from 'src/app/services/ticket.service';
import { ToastrService } from 'ngx-toastr';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-ticket-monthly',
  templateUrl: './ticket-monthly.component.html',
  styleUrls: ['./ticket-monthly.component.css']
})
export class TicketMonthlyComponent {
  @ViewChild(LoadingComponent, { static: false }) loading!: LoadingComponent;

  listData: any[] = [];

  constructor(
    private _service: TicketService,
    private _toastr: ToastrService,
  ) { }

  ngOnInit() {
    this.getAllData();
  }

  getAllData() {
    
  }

  deletePromotion(id: number) {
    
  }

}
