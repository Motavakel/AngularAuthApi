import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../../services/auth.service';
import ValidateForm from '../../helpers/validateform';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrl: './reset-password.component.css'
})
export class ResetPasswordComponent implements OnInit {

  resetPasswordForm!:FormGroup;
  type:string = "password";
  isDisplay:boolean = false;
  eyeIcon:string = "fa-eye-slash";


  constructor(
    private fb:FormBuilder,
    private toaster:ToastrService,
    private router:Router,
    private auth:AuthService
  ){}

  ngOnInit(): void {
    this.resetPasswordForm = this.fb.group({
      resetCode:["",Validators.required],
      newPassword:["",Validators.required],
      userName:[this.auth.getUserNameByToken(),Validators.required]
    });

  }

  onHideShowPass(){
    this.isDisplay = !this.isDisplay;
    this.type = this.isDisplay ? "text" : "password";
    this.eyeIcon = this.isDisplay ? "fa-eye" : "fa-eye-slash";
  }

  onSubmit() {
      if (this.resetPasswordForm.valid) {
        if (this.auth.checkToken()) {
          this.auth.ResetPassword(this.resetPasswordForm.value).subscribe({
            next: (res) => {
              this.toaster.success(res.message);
              this.resetPasswordForm.reset();
              sessionStorage.removeItem("tempId");
              this.router.navigate(["login"]);
            },
            error: (err) => {
              console.log(err);
              const errorResponse = err?.error || {};
              if (errorResponse.errors) {
                // اگر خطا به صورت آرایه‌ای از پیام‌ها باشد
                errorResponse.errors.forEach((message: string[]) => {
                  message.forEach((m: string) => {
                    this.toaster.error(m);
                  });
                });
              } else if (errorResponse.message) {
                // اگر پیام خطای عمومی وجود داشته باشد
                this.toaster.error(errorResponse.message);
              } else {
                // اگر ساختار خطا متفاوت بود
                this.toaster.error("خطا در ارسال درخواست");
              }
            }
          });
        } else {
          this.router.navigate(['forgetPassword']);
          this.toaster.error("مدت زمان وارد کردن رمز منقضی شده است");
        }
      } else {
        // اعتبارسنجی فرم
        ValidateForm.validateAllFormFileds(this.resetPasswordForm);
      }
  }


}
