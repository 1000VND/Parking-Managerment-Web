import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserService } from 'src/app/services/user.service';
import { CreateEditUserDto } from 'src/app/models/User/createedit.model';
import { finalize } from 'rxjs';

@Component({
  selector: 'user-create-edit',
  templateUrl: './user-create-edit.component.html',
  styleUrls: ['../user.component.css']
})
export class UserCreateEditComponent implements OnInit {
  @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
  @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

  isVisible = false;
  isOkLoading = false;
  options = [
    { label: 'Admin', value: 1 },
    { label: 'User', value: 0 }
  ];
  userDto!: CreateEditUserDto | null;
  form!: FormGroup;

  constructor(
    private _service: UserService,
    private fb: FormBuilder,
  ) { }

  ngOnInit() {
    this.form = this.fb.group({
      id: [null],
      userName: [null, Validators.required],
      passWord: [null],
      fullName: [null, Validators.required],
      role: [null, Validators.required],
    })
  }

  handleCancel(): void {
    this.isVisible = false;
    this.modalClose.emit(null)
  }

  save(): void {
    this.form.controls['passWord'].setValue(this.userDto?.passWord);
    if (this.form.valid) {
      this.isOkLoading = true;
      this._service.createOrEdit(this.form.value).pipe(finalize(() => {
        this.isVisible = false;
        this.isOkLoading = false;
        this.modalSave.emit(null);
      })).subscribe(() => {
      })
    }
    else {
      Object.values(this.form.controls).forEach(control => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({ onlySelf: true });
        }
      });
    }
  }

  show(userDto?: CreateEditUserDto ) {
    this.form.reset();
    this.userDto = null;
    if (userDto) {
      this.form.controls['passWord'].setValidators(null);
      this.form.patchValue(userDto);
      this.userDto = userDto;
    } else {
      this.form.controls['passWord'].setValidators(Validators.required);
    }
    this.isVisible = true;
  }

}
