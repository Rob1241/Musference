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

const routes: Routes = [
  { path:'account',component:AccountComponent},
   { path:'users',component:UsersComponent},
   { path:'login',component:LoginComponent},
   { path:'signup',component:SignupComponent},
   { path:'askquestion',component:AskQuestionComponent},
   { path:'questions',component:QuestionComponent},
   { path:'onequestion',component:OneQuestionComponent},
   {path:'tracks',component:TracksComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
export const routingComponents = [AccountComponent,UsersComponent,
  LoginComponent,SignupComponent,AskQuestionComponent,QuestionComponent,OneQuestionComponent,TracksComponent];