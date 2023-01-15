import { Component, OnInit } from '@angular/core';
import { UserApiService } from '../api-services/user-api.service';
import { Form, FormBuilder, FormGroup,Validators } from '@angular/forms';
import { changeCity } from '../models/changeCity';
import { changeContact } from '../models/changeContact';
import { changeDescription } from '../models/changeDescription';
import { changeEmail } from '../models/changeEmail';
import { changeName } from '../models/changeName';
import { changeCountry } from '../models/changeCountry';
import { changePassword } from '../models/changePasswordModel';
import { deleteUser } from '../models/deleteUser';


console.log('dsa');
@Component({
  selector: 'app-edit-profile',
  templateUrl: './edit-profile.component.html',
  styleUrls: ['./edit-profile.component.css']
})
export class EditProfileComponent implements OnInit {
  user:any;
  form:FormGroup;
  constructor(private service: UserApiService,private fb:FormBuilder) {
    this.form = this.fb.group({
      name: ['',Validators.required],
     description: ['',Validators.required],
     city: ['',Validators.required],
     country: ['',Validators.required],
     contact: ['',Validators.required],
     email: ['',Validators.required],
     delete: ['',Validators.required],
     oldpassword: ['',Validators.required],
     newpassword: ['',Validators.required],
     deletepassword: ['',Validators.required]
    })
   
   }
  ngOnInit(): void {
    let userId= Number(localStorage.getItem("user_id"));
    if(userId){
    this.service.getUser(userId).subscribe(data=>{
      this.user = data;
      console.log(data);
      this.form.get('name')?.setValue(this.user.result.name)
      this.form.get('description')?.setValue(this.user.result.description)
      this.form.get('email')?.setValue(this.user.result.email)
      this.form.get('country')?.setValue(this.user.result.country)
      this.form.get('city')?.setValue(this.user.result.city)
      this.form.get('contact')?.setValue(this.user.result.contact)
    })}
    
  }

  saveChanges(){
    const val = this.form.value;
    let changeNameModel = <changeName>{};
    changeNameModel.name = val.name;
    let changeDescriptionModel = <changeDescription>{};
    changeDescriptionModel.description = val.description;
    let changeCityModel = <changeCity>{};
    changeCityModel.city = val.city;
    let changeCountryModel = <changeCountry>{};
    changeCountryModel.country = val.country;
    let changeContactModel = <changeContact>{};
    changeContactModel.contact = val.contact;
    let changeEmailModel = <changeEmail>{};
    changeEmailModel.email = val.email;
    console.log(changeNameModel);
    this.service.changeName(changeNameModel).subscribe();
    this.service.changeDescription(changeDescriptionModel).subscribe();
    this.service.changeCity(changeCityModel).subscribe();
    this.service.changeCountry(changeCountryModel).subscribe();
    this.service.changeContact(changeContactModel).subscribe();
    this.service.changeEmail(changeEmailModel).subscribe();
  }

  deleteAccount(){
    const formm = this.form.value;
    let deleteUserModel = <deleteUser>{};
    deleteUserModel.password=formm.deletepassword;
    this.service.deleteUser(deleteUserModel).subscribe();
  }

  changePassword(){
    const formm = this.form.value;
    let changePasswordModel = <changePassword>{};
    changePasswordModel.oldPassword=formm.oldpassword;
    changePasswordModel.password=formm.newpassword;
    //console.log(changePasswordModel.oldPassword);
    //console.log(changePasswordModel.password);
    this.service.changePassword(changePasswordModel).subscribe();
  }
}
