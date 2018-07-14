import { BrowserModule } from '@angular/platform-browser';
import { NgModule, ErrorHandler } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {MatSliderModule} from '@angular/material/slider';
import { ToastrModule } from 'ngx-toastr';
import { LoadingModule, ANIMATION_TYPES } from 'ngx-loading';
import {MatDialogModule} from '@angular/material/dialog';
import {MatInputModule} from '@angular/material/input';
import {MatButtonModule} from '@angular/material/button';
import {MatStepperModule} from '@angular/material/stepper';
import {MatSelectModule} from '@angular/material/select';
import {MatRadioModule} from '@angular/material/radio';
import { MatDialogRef } from '@angular/material';
import {MatCheckboxModule} from '@angular/material/checkbox';



import { ImageCropperModule } from 'ngx-image-cropper';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { ThumbnailExtractorComponent } from './thumbnail-extractor/thumbnail-extractor.component';
import { ErrorsHandler } from './ErrorsHandler';
import { TokenInterceptor } from './TokenInterceptor';
import { VideoComponent } from './video/video.component';
import { MatIconModule } from '@angular/material/icon';
import { ErrorComponent } from './error/error.component';
import { InfiniteScrollModule } from "ngx-infinite-scroll";
import { CommentsComponent } from './comments/comments.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { UploadComponent } from './upload/upload.component';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';
import { GrowComponent } from './grow/grow.component';
import { AvatarPickerComponent } from './avatar-picker/avatar-picker.component';
import { UserDetailComponent } from './user-detail/user-detail.component';
import { SearchResultComponent } from './search-result/search-result.component';
import { PrivacyComponent } from './privacy/privacy.component';

let dummyDialogRef = { close: () => { 
 }}

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    ThumbnailExtractorComponent,
    VideoComponent,
    ErrorComponent,
    CommentsComponent,
    LoginComponent,
    RegisterComponent,
    UploadComponent,
    ForgotPasswordComponent,
    GrowComponent,
    AvatarPickerComponent,
    UserDetailComponent,
    SearchResultComponent,
    PrivacyComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'Home', component: HomeComponent },
      { path: 'Latest', component: HomeComponent },
      { path: 'Upload', component: UploadComponent },
      { path: 'Login', component: LoginComponent },
      { path: 'Register', component: RegisterComponent },
      { path: 'ForgotPassword', component: ForgotPasswordComponent },
      { path: 'Grow', component: GrowComponent },
      { path: 'Search/:id', component: SearchResultComponent },
      { path: 'thumb', component: ThumbnailExtractorComponent },
      { path: 'video/:id', component: VideoComponent },
      { path: 'user/:id', component: UserDetailComponent },
      { path: 'fetch-data', component: FetchDataComponent },
      { path: 'error', component: ErrorComponent },
      { path: 'comment', component: CommentsComponent },
      { path: 'privacy', component: PrivacyComponent }
    ]),
    BrowserAnimationsModule,
    MatSliderModule,
    ToastrModule.forRoot(),
    MatIconModule,
    MatDialogModule,
    MatInputModule,
    MatButtonModule,
    MatStepperModule,
    MatSelectModule,
    MatCheckboxModule,
    MatRadioModule,
    ImageCropperModule,
    LoadingModule.forRoot({
      animationType: ANIMATION_TYPES.wanderingCubes,
      backdropBackgroundColour: 'rgba(0,0,0,0.1)',
      backdropBorderRadius: '4px',
      primaryColour: '#ffffff',
      secondaryColour: '#ffffff',
      tertiaryColour: '#ffffff'
    }),
    InfiniteScrollModule
  ],
  providers: [
    {
    provide: ErrorHandler,
    useClass: ErrorsHandler,
  },
  {
    provide: HTTP_INTERCEPTORS,
    useClass: TokenInterceptor,
    multi: true,
  },
  { provide: MatDialogRef, useValue: dummyDialogRef }],
  bootstrap: [AppComponent],
  entryComponents: [LoginComponent,AvatarPickerComponent]
})
export class AppModule { }
