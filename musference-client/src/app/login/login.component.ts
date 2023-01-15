import { Component, } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { LoginModel } from '../models/login-model';
import { AuthService } from '../api-services/auth.service';
import { FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  form:FormGroup;

  constructor(private fb:FormBuilder, 
               private authService: AuthService, 
               private router: Router) {
                

      this.form = this.fb.group({
          email: ['',Validators.required],
          password: ['',Validators.required]
      });
  }
  
  login() {
      const val = this.form.value;
      let model = <LoginModel>{};
      model.login = val.email;
      model.password = val.password;
      if (val.email && val.password) {
          this.authService.Login(model)
              .subscribe(
                  () => {
                      console.log("User is logged in");
                      this.router.navigateByUrl('/questions');
                  }
              );
      }
  }
}
