import { Component, OnInit } from '@angular/core';
import { FormBuilder,FormGroup,Validators } from '@angular/forms';
import { QuestionApiService } from '../api-services/question-api.service';
import { UserApiService } from '../api-services/user-api.service';
import { SignupModel } from '../models/signup-model';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css']
})
export class SignupComponent implements OnInit {

  form:FormGroup
  constructor(private fb:FormBuilder, private service:UserApiService) {
    this.form = this.fb.group({
      login: ['',Validators.required],
      name: ['',Validators.required],
      email: ['',Validators.required],
      password: ['',Validators.required]
    })
   }

  signup()
  {
    const val = this.form.value;
    let model = <SignupModel>{};
    model.login = val.login;
    model.name = val.name;
    model.email = val.email;
    model.password = val.password;
    if(val.login&&val.name&&val.email&&val.password){
      this.service.Signup(model).subscribe(()=>{
        console.log("User signed up")
      });
    }
  }
  ngOnInit(): void {

  }

}
