import { Component, Renderer2, ViewChild } from '@angular/core';
import { LoadingComponent } from '../common/loading/loading.component';
import { TicketService } from 'src/app/services/ticket.service';
import { ToastrService } from 'ngx-toastr';
import { finalize } from 'rxjs';
import { GetAllDataTicketMonthlyDto } from 'src/app/models/TicketMonthly/get-all.model';

@Component({
  selector: 'app-ticket-monthly',
  templateUrl: './ticket-monthly.component.html',
  styleUrls: ['./ticket-monthly.component.css']
})
export class TicketMonthlyComponent {
  @ViewChild(LoadingComponent, { static: false }) loading!: LoadingComponent;

  listData: any[] = [];
  plate: string = '';
  selectedRowIndex = -1;
  selectedItem: any;

  constructor(
    private _service: TicketService,
    private _toastr: ToastrService,
    private renderer: Renderer2
  ) { }

  ngOnInit() {
    this.getAllData();
  }

  getAllData() {
    this._service.getAllTicket().subscribe(res => {
      this.listData = res.data;
    })
  }

  deleteTicket(id: number) {
    this.loading.loading(true);
    this._service.deleteTicketMonthly(id)
      .pipe(
        finalize(() => {
          this.loading.loading(false);
        })).subscribe((res) => {
          if (res.statusCode == 200) {
            this._toastr.success('Delete success');
          }
          this.listData = this.listData.filter((user) => user.id !== id);
        });
  }
}
