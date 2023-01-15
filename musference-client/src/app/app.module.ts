import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';
import {HttpClientModule, HTTP_INTERCEPTORS} from '@angular/common/http';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule, routingComponents } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavbarComponent } from './navbar/navbar.component';
import { SidebarComponent } from './sidebar/sidebar.component';
import { QuestionApiService } from './api-services/question-api.service';
import { OneQuestionComponent } from './one-question/one-question.component';
import { TracksComponent } from './tracks/tracks.component';
import { LoginComponent } from './login/login.component';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatCardModule} from '@angular/material/card';
import { AuthorizationInterceptor } from './interceptors/authorization.interceptor';
import { EditProfileComponent } from './edit-profile/edit-profile.component';
import { AddtrackComponent } from './tracks/addtrack/addtrack.component';

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    SidebarComponent,
    OneQuestionComponent,
    routingComponents,
    TracksComponent,
    LoginComponent,
    EditProfileComponent,
    AddtrackComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatCardModule
  ],
  providers: [{provide:HTTP_INTERCEPTORS,useClass: AuthorizationInterceptor,multi:true},
  QuestionApiService],
  bootstrap: [AppComponent]

})
export class AppModule { }
