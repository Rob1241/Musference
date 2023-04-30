import { Component, NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AccountComponent } from './account/account.component';
import { AskQuestionComponent } from './ask-question/ask-question.component';
import { LoginComponent } from './login/login.component';
import { QuestionComponent } from './question/question.component';
import { SignupComponent } from './signup/signup.component';
import { UsersComponent } from './users/users.component';
import { OneQuestionComponent } from './one-question/one-question.component';
import { TracksComponent } from './tracks/tracks.component';
import { EditProfileComponent } from './edit-profile/edit-profile.component';
import { AddtrackComponent } from './tracks/addtrack/addtrack.component';
import { HomepageComponent } from './homepage/homepage.component';

const routes: Routes = [
  { path:'account/:id',component:AccountComponent},
   { path:'users/:page',component:UsersComponent},
   { path:'users',component:UsersComponent},
   { path:'login',component:LoginComponent},
   { path:'signup',component:SignupComponent},
   { path:'askquestion',component:AskQuestionComponent},
   { path:'questions/:page',component:QuestionComponent},
   { path:'questions',component:QuestionComponent},
   { path:'onequestion/:id/:page',component:OneQuestionComponent},
   {path:'tracks/:page',component:TracksComponent},
   {path:'tracks',component:TracksComponent},
   {path:'edit-profile', component:EditProfileComponent},
   {path:'addtrack', component:AddtrackComponent},
   {path:'',component:HomepageComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
export const routingComponents = [AccountComponent,UsersComponent,
  LoginComponent,SignupComponent,AskQuestionComponent,QuestionComponent
  ,OneQuestionComponent,TracksComponent, AddtrackComponent,HomepageComponent];