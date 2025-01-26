import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { RegisterUserDto } from '../models/register-user-dto';
import { UserDto } from '../models/user-dto';
import { environment } from '../../environments/environment';
import { ForgetPasswordDto } from '../models/forget-password-dto';
import { ResetPasswordDto } from '../models/reset-password-dto';
import { jwtDecode } from 'jwt-decode';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiKey:string = environment.apiBaseUrl;

  constructor(
    private request: HttpClient,
    private toaster: ToastrService,
    private router: Router
  ) {}

  signUp(signupObj: RegisterUserDto):Observable<any>{
    return this.request.post<any>(
      this.apiKey + 'register',
      signupObj
    );
  }

  signIn(signInObj: UserDto):Observable<any>{
    return this.request.post<any>(this.apiKey + 'authenticate', signInObj);
  }

  storeToken(tokenValue: string) {
    localStorage.setItem('token', tokenValue);
  }

  getToken() {
    return localStorage.getItem('token');
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('token');
  }

  forgetPassword(nameOrEmail:ForgetPasswordDto):Observable<any>{
    return this.request.post<any>(`${this.apiKey}forgetPassword`,nameOrEmail);
  }

  ResetPassword(resetPassObj:ResetPasswordDto):Observable<any>{
    return this.request.post<any>(`${this.apiKey}resetPassword`,resetPassObj);
  }


  checkToken():boolean{
     return !!sessionStorage.getItem('tempId');
  }

  getUserNameByToken():string{
    const token:string|null = sessionStorage.getItem('tempId');

    if(!token){
      this.router.navigate(['forgetPassword']);
      this.toaster.error("مدت زمان وارد کردن رمز منقضی شده است");
      return "";
    }

    const decodeToken:any = jwtDecode(token);
    return decodeToken?.unique_name;
  }

  getDashboardDetail():Observable<any>{

    const token:string|null = localStorage.getItem('token');

    if(!token){
      this.router.navigate(['login']);
      this.toaster.error("شما اجازه دسترسی به این بخش را ندارید");
      return of(null)
    }

    const decodeToken:any = jwtDecode(token);
    const userName = decodeToken?.unique_name;


    return this.request.get<any>(`${this.apiKey}UserDetail/${userName}`);
  }

  signOut(){
    localStorage.removeItem("token");
  }
}
