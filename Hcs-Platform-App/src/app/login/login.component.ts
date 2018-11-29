import { Component, OnInit, Inject } from '@angular/core';
import { UserState } from '../hcs-lib/UserState';
import { Router, ActivatedRoute } from '@angular/router';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'hcs-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  public account: string;
  public password: string;
  public errorMessage: string;
  constructor(public userState: UserState, private router: Router, private route: ActivatedRoute) {

  }
  public login() {
    this.errorMessage = '';
    this.userState.login(this.account, this.password).subscribe(resp => {
      if (resp.Succeeded) {
        this.route.queryParams.subscribe(p => {
          if (p.returnUrl) {
            this.router.navigateByUrl(p.returnUrl);
          } else {
            this.router.navigateByUrl('/');
          }
        });
      } else {
        this.errorMessage = resp.MessageCode;
      }
    });
  }
  ngOnInit() {
  }

}
