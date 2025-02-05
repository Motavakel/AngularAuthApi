import { FormBuilder, FormGroup ,Validators} from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import ValidateForm from '../../helpers/validateform';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})

export class LoginComponent implements OnInit {

  type:string = "password";
  isDisplay:boolean = false;
  eyeIcon:string = "fa-eye-slash";

  loginForm!:FormGroup;

  constructor(
    private fb:FormBuilder,
    private auth:AuthService,
    private router:Router,
    private toastr:ToastrService
  ){}

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      username:["",Validators.required],
      password:["",Validators.required]
    })
  }

  onHideShowPass(){
    this.isDisplay = !this.isDisplay;
    this.type = this.isDisplay ? "text" : "password";
    this.eyeIcon = this.isDisplay ? "fa-eye" : "fa-eye-slash";
  }

  onForgetPassword(){
    this.router.navigate(["forgotpassword"]);
  }

  onSignIn(){
    if(this.loginForm.valid){

      this.auth.signIn(this.loginForm.value)
      .subscribe({
        next:(res)=>{
          this.toastr.success(res.message);
          this.loginForm.reset();
          this.auth.storeToken(res.data)
          this.router.navigate(["dashboard"]);
        },
        error: (err) => {
          console.log(err)
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

    }else{
      //کلاس و متد استاتیک اعتبارسنجی ورودی از پوشه هلپر
      ValidateForm.validateAllFormFileds(this.loginForm);
    }
  }

}
