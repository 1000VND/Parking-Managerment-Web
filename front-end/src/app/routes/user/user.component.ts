import { Component, OnInit, ViewChild } from '@angular/core';
import { Login } from 'src/app/models/login.model';
import { UserDto } from 'src/app/models/user.model';
import { DataFormatService } from 'src/app/services/data-format.service';
import { UserService } from 'src/app/services/user.service';
import { LoginService } from 'src/app/services/login.service';
import { ToastrService } from 'ngx-toastr';
import { finalize } from 'rxjs';
import { NgxSpinnerService } from 'ngx-spinner';
import { CreateEditUserComponent } from './create-edit-user/create-edit-user.component';

@Component({
  selector: 'user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent implements OnInit {
  @ViewChild("createOrEditModal") createOrEditModal!: CreateEditUserComponent
  editCache: { [key: string]: { edit: boolean; data: Login } } = {};
  listData: UserDto[] = [];
  isVisible = false;
  isOkLoading = false;
  selectedData: UserDto = new UserDto();
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
    this._service.getAllUser().subscribe((res) => {
      this.listData = res.data;
    })
  }

  showModal(data: UserDto) {
    this.selectedData = Object.assign({}, data);
    this.createOrEditModal.show(this.selectedData)
  }

  deleteUser(id: number) {
    this.loading(true);
    this._service.deleteUser(id).pipe(finalize(()=>{
      this.loading(false);
    })).subscribe(() => {
      this._toast.success('Delete success')
      this.getAllData();
    }, error => {
      if (error) {
        this._toast.success('Delete fail')
      }
    });
  }

  loading(loading: boolean) {
    if (loading) this.spinner.show();
    else this.spinner.hide();
  }
}
