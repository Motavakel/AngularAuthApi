import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import ValidateForm from '../../helpers/validateform';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrl: './forgot-password.component.css'
})
export class ForgotPasswordComponent implements OnInit {

  forgetPasswordForm!:FormGroup;

  constructor(
    private fb:FormBuilder,
    private authService : AuthService,
    private router:Router,
    private toaster:ToastrService
  ){}


  ngOnInit(): void {
    this.forgetPasswordForm = this.fb.group({
      identifier:["",Validators.required]
    })
  }




  onSubmit(){

    if(this.forgetPasswordForm.valid){

      this.authService.forgetPassword(this.forgetPasswordForm.value).subscribe({
        next:(res)=>{
         console.log(res);
         this.toaster.success(res.message);
         sessionStorage.setItem('tempId', res.data);
         this.router.navigate(["resetpassword"]);
        },
        error: (err) => {
          console.log(err)
          const errorResponse = err?.error || {};
          if (errorResponse.errors) {
            errorResponse.errors.forEach((message:string[]) => {
                message.forEach((m: string) => {
                  this.toaster.error(m);
                });
            });
          } else {
            this.toaster.error(err?.error.message);
          }
        },
      })
    }else{
      //کلاس و متد استاتیک اعتبارسنجی ورودی از پوشه هلپر
      ValidateForm.validateAllFormFileds(this.forgetPasswordForm);
    }
  }
}
