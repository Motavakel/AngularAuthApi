import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit {
  userDetail: any;

  constructor(
    private authService: AuthService,
    private router: Router,
    private toaster: ToastrService
  ) {}

  ngOnInit(): void {
    this.authService.getDashboardDetail().subscribe({
      next: (data) => {

        this.userDetail = data.data;
      },
      error: (err) => {
        console.log(err?.error);
      },
    });
  }

  signOut() {
    this.authService.signOut();
    this.toaster.success('خروج از حساب کاریری');
    this.router.navigate(['login']);
  }
}
