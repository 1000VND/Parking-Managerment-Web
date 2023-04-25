import { Component, OnInit } from '@angular/core';
import { Login } from 'src/app/models/login.model';
import { UserDto } from 'src/app/models/user.model';
import { DataFormatService } from 'src/app/services/data-format.service';
import { UserService } from 'src/app/services/user.service';
import { ToastrService } from 'ngx-toastr';
import { catchError, finalize, throwError } from 'rxjs';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent implements OnInit {
  editCache: { [key: string]: { edit: boolean; data: Login } } = {};
  listData: any[] = [];
  isVisible = false;
  isOkLoading = false;
  selectedData: any;
  addUser: UserDto = new UserDto();

  constructor(
    private _dataformat: DataFormatService,
    private _service: UserService,
    private _toast: ToastrService,
    private spinner: NgxSpinnerService
  ) { }

  ngOnInit() {
    this.getAllData();
  }

  getAllData() {
    this.listData = [];
    this._service.getAllUser().subscribe((res) => {
      this.listData = res.data;
    });
  }

  deleteUser(id: number) {
    this.loading(true);
    this._service.deleteUser(id)
      .pipe(
        finalize(() => {
          this.loading(false);
        })).subscribe((res) => {
          if (res.statusCode == 200) {
            this._toast.success('Delete success');
          }
          this.listData = this.listData.filter((user) => user.id !== id);
        });
  }


  loading(loading: boolean) {
    if (loading) this.spinner.show();
    else this.spinner.hide();
  }
}
