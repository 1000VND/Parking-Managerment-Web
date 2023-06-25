import { Component, OnInit, Renderer2, ViewChild } from '@angular/core';
import { UserDto } from 'src/app/models/User/user.model';
import { UserService } from 'src/app/services/user.service';
import { ToastrService } from 'ngx-toastr';
import { LoadingComponent } from '../common/loading/loading.component';
import { finalize } from 'rxjs';

@Component({
  selector: 'user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent implements OnInit {
  @ViewChild(LoadingComponent, { static: false }) loading!: LoadingComponent;
  listData: any[] = [];
  isVisible = false;
  isOkLoading = false;
  selectedData: any;
  addUser: UserDto = new UserDto();
  selectedRowIndex = -1;

  constructor(
    private _service: UserService,
    private _toast: ToastrService,
    private renderer: Renderer2
  ) { }

  ngOnInit() {
  }

  ngAfterViewInit() {
    this.getAllData();
  }

  getAllData() {
    this.listData = [];
    this.loading.loading(true);
    this._service.getAllUser().pipe(finalize(() => {
      this.loading.loading(false);
    })).subscribe((res) => {
      this.listData = res.data;
    });
  }

  deleteUser(id: number) {
    this.loading.loading(true);
    this._service.deleteUser(id)
      .pipe(
        finalize(() => {
          this.loading.loading(false);
        })).subscribe((res) => {
          if (res.statusCode == 200) {
            this._toast.success('Delete success');
          }
          this.listData = this.listData.filter((user) => user.id !== id);
        });
  }

  onChangeSelectRow(index: number) {
    if (index === this.selectedRowIndex) {
      this.selectedRowIndex = -1;
    } else {
      const previousRowElement = document.querySelector('.selected');
      if (previousRowElement) {
        this.renderer.removeClass(previousRowElement, 'selected');
      }
      this.selectedRowIndex = index;
    }

    const rowElements = document.querySelectorAll('tr[nz-tr]');
    rowElements.forEach((rowElement, i) => {
      if (i === this.selectedRowIndex) {
        this.renderer.addClass(rowElement, 'selected');
      } else {
        this.renderer.removeClass(rowElement, 'selected');
      }
    });
  }
}
