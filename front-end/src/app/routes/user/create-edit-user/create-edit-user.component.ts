import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { UserDto } from 'src/app/models/user.model';
import { UserService } from 'src/app/services/user.service';
import { UserComponent } from '../user.component';
import { LoginService } from 'src/app/services/login.service';
import { of, finalize, catchError } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'create-edit-user',
  templateUrl: './create-edit-user.component.html',
  styleUrls: ['../user.component.css']
})
export class CreateEditUserComponent implements OnInit {
  @Input() selectedData!: UserDto
  @ViewChild(UserComponent) modal!: UserComponent
  @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
  @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();
  form!: FormGroup;
  title: string = '';


  isVisible = false;
  isOkLoading = false;

  constructor(
    private _service: UserService,
    private toastr: ToastrService,
    private _loginService: LoginService,
    private fb: FormBuilder,
  ) { }

  ngOnInit() {
  }

  handleCancel(): void {
    this.isVisible = false;
    this.modalClose.emit(null)
  }

  handleOk(): void {
    this.isOkLoading = true;
    this._service.createOrEdit(this.selectedData).pipe(finalize(() => {
      this.isVisible = false;
      this.isOkLoading = false;
      this.modalSave.emit(null);
    })).subscribe(() => {
      this.toastr.success('Edit succes');
    }, error => {
      if (error) {
        this.toastr.success('Edit fail');
      }
    })
  }

  show(data: UserDto) {
    if (data.id != undefined) {
      this.selectedData = data;

    } else {
      this.selectedData = new UserDto;
      this.selectedData.id = null;
    }
    this.form = this.fb.group({
      id: [null],
      userName: [null, Validators.required],
    })
    this.form.patchValue(this.selectedData);
    this.title = this.form.value.id == null ? 'Add user' : 'Edit user'
    this.isVisible = true;
  }
}
