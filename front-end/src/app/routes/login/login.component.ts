import { Component, OnInit, ViewChild, ViewChildren } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { LoginService } from 'src/app/services/login.service';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { Login } from 'src/app/models/login.model';
import { LoadingComponent } from '../common/loading/loading.component';
import { finalize } from 'rxjs';
import { UserDto } from 'src/app/models/user.model';

@Component({
  selector: 'login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  @ViewChild(LoadingComponent, { static: false }) loading!: LoadingComponent;
  userName!: string;
  passWord!: string;
  validateForm!: UntypedFormGroup;
  dto: Login = new Login();
  passwordVisible = false;

  constructor(
    private _login: LoginService,
    private fb: UntypedFormBuilder,
    private toastr: ToastrService,
    private router: Router,
  ) { }

  ngOnInit(): void {
    this.validateForm = this.fb.group({
      userName: [null, [Validators.required]],
      password: [null, [Validators.required]],
    });
  }
  submitForm() {
    this.loading.loading(true);
    this._login.checkUser(this.dto)?.pipe(finalize(() => {
      this.loading.loading(false);
    })).subscribe((res: any) => {
      if (res.statusCode == 200) {
        localStorage.setItem('user', JSON.stringify(res.data))
        sessionStorage.setItem('user', JSON.stringify(res.data))
        if (res.data.role == 1) {
          this.router.navigateByUrl('nav-bar/register');
        } else {
          this.router.navigateByUrl('nav-bar/home');
        }
        this.toastr.success('Login susscess!');
      }
    })
  }
}
