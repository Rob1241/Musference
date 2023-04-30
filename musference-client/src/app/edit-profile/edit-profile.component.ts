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
import { UploadService } from '../services/upload.service';
import { changeImage } from '../models/changeImage';
import { Router } from '@angular/router';
import { AuthService } from '../api-services/auth.service';
import { Subject } from 'rxjs';

console.log('dsa');
@Component({
  selector: 'app-edit-profile',
  templateUrl: './edit-profile.component.html',
  styleUrls: ['./edit-profile.component.css'],
  providers:[UploadService]
})
export class EditProfileComponent implements OnInit {
  user:any;
  form:FormGroup;
  imageurl: any;
  constructor(private authService:AuthService ,private router:Router, private service: UserApiService,private upload:UploadService,private fb:FormBuilder) {
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
      this.form.get('name')?.setValue(this.user.result.name)
      this.form.get('description')?.setValue(this.user.result.description)
      this.form.get('email')?.setValue(this.user.result.email)
      this.form.get('country')?.setValue(this.user.result.country)
      this.form.get('city')?.setValue(this.user.result.city)
      this.form.get('contact')?.setValue(this.user.result.contact)
    })}
  }
  files: File[] = [];
	onSelect(event:any) {
		this.files.push(...event.addedFiles);
	}
	onRemove(event:any) {
		this.files.splice(this.files.indexOf(event), 1);
	}
  onUpload(){
    if(!this.files[0]){
      alert('error 404 image not found')
    }
    const file_data = this.files[0];
    const data = new FormData();
    data.append('file',file_data);
    data.append('upload_preset','musference');
    data.append('cloud_name','da1tlcmhr');
    this.upload.uploadImage(data).subscribe((response)=>{
    if(response){
      this.imageurl = response.secure_url
    }
    });
  }
  changeImage(){
    let changeImageModel = <changeImage>{};
    changeImageModel.imageFile = this.imageurl;
    this.service.changeImage(changeImageModel).subscribe();
    this.imageurl=null;
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
    this.service.changeName(changeNameModel).subscribe();
    this.service.changeDescription(changeDescriptionModel).subscribe();
    this.service.changeCity(changeCityModel).subscribe();
    this.service.changeCountry(changeCountryModel).subscribe();
    this.service.changeContact(changeContactModel).subscribe();
    this.service.changeEmail(changeEmailModel).subscribe();
  }
  changePassword(){
    const formm = this.form.value;
    let changePasswordModel = <changePassword>{};
    changePasswordModel.oldPassword=formm.oldpassword;
    changePasswordModel.password=formm.newpassword;
    this.service.changePassword(changePasswordModel).subscribe();
  }
  deleteAccount(){
    const formm = this.form.value;
    let deleteUserModel = <deleteUser>{};
    deleteUserModel.password=formm.deletepassword;
    this.service.deleteUser(deleteUserModel).subscribe();
    this.authService.logout()
    this.router.navigate(['/']);
  }
}

