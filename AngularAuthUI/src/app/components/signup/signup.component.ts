import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import ValidateForm from '../../helpers/validateform';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrl: './signup.component.css'
})
export class SignupComponent implements OnInit {
  type:string = "password";
  isDisplay:boolean = false;
  eyeIcon:string = "fa-eye-slash";

  signupForm!:FormGroup;

  constructor(
    private fb:FormBuilder,
    private auth:AuthService,
    private router:Router,
    private toastr: ToastrService
  ){}

  ngOnInit(): void {
    this.signupForm = this.fb.nonNullable.group(
      {
        username: ["", Validators.required],
        firstname: ["", Validators.required],
        lastname: ["", Validators.required],
        email: ["", [Validators.required, Validators.email]],
        password: ["", [Validators.required, Validators.minLength(6)]],
        confirmPassword: ["", Validators.required],
      },
      { validators: this.passwordMatchValidator }
    );
  }

  onHideShowPass(){
    this.isDisplay = !this.isDisplay;
    this.type = this.isDisplay ? "text" : "password";
    this.eyeIcon = this.isDisplay ? "fa-eye" : "fa-eye-slash";
  }

  onSignUp() {
    if (this.signupForm.valid) {
      this.auth.signUp(this.signupForm.value).subscribe({
        next: (res) => {
          this.signupForm.reset();
          this.toastr.success(res.message);
          this.router.navigate(["login"]);
        },
        error: (err) => {
          const errorResponse = err?.error || {};
          if (errorResponse.errors) {
            errorResponse.errors.forEach((message:string[]) => {
                message.forEach((m: string) => {
                  this.toastr.error(m);
                });
            });
          } else {
            this.toastr.error(err?.error.message);
          }
        },
      });
    } else {
      ValidateForm.validateAllFormFileds(this.signupForm);
    }
  }

  //بررسی مچ برودن رمز و تایید رمز
  //ورودی از نوع فیلدها یا کنترل ها ی فرم
  //خروجی بهصورت کلید مقدار، که کلید از جنسه آرایه ای از رشته ها و مقدار از جنس بولین
  passwordMatchValidator(controls: AbstractControl): { [key: string]: boolean } | null {
    const password = controls.get('password')?.value;
    const confirmPassword = controls.get('confirmPassword')?.value;

    if (password && confirmPassword && password !== confirmPassword) {
      return { passwordMismatch: true };
    }
    return null;
  }


}
/*
یک توضیح
ساختار پاسخ از سمت سرور به صورت زیر است

"errors": {
  "Email": [
    "ایمیل وارد شده معتبر نیست"
  ],
  "Password": [
    "طول رمز ورود باید حداقل 8 کاراکتر باشد"
  ]
},

اما کتابخانه
HttpClient
یک
error
هم اضافه می کند
*/
