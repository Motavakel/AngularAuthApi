import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ErrorHandlerService {

  constructor(
     private toaster: ToastrService,
     private router: Router
    ){}

    handleError(err: HttpErrorResponse): Observable<never> {
      if (err.status === 401) {
        this.toaster.error("توکن منقضی شده لطفا دوباره لاگین کنید");
        this.router.navigate(['login']);
      } else {
        this.toaster.error("خطای غیرمنتظره‌ای رخ داد");
      }
      return throwError(() => err);
    }
}
