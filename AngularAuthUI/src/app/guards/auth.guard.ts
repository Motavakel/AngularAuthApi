import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { inject } from '@angular/core';
import { ToastrService } from 'ngx-toastr';


export const authGuard: CanActivateFn = (route, state) => {

  const authService = inject(AuthService);
  const router = inject(Router);
  const toaster = inject(ToastrService);

    if(!authService.isLoggedIn()){
      toaster.info("لطفا ابتدا لاگین کنید");
      router.navigate(["login"])
      return false;
    }

    return true;
};
