import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import ValidateForm from '../../helpers/validateform';
import { HttpClientModule } from '@angular/common/http';
import { AuthService } from '../../services/auth.service';
import { NgToastModule, NgToastService } from 'ng-angular-popup';


@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule,RouterModule,ReactiveFormsModule,HttpClientModule,NgToastModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent implements OnInit {

  loginForm!: FormGroup;
  constructor(
    private fb: FormBuilder,
    private toast: NgToastService,
    private auth: AuthService,
    private router: Router,
    //private userStore: UserStoreService,
    //private resetPasswordService: ResetPasswordService
    ){

  }
  ngOnInit(): void {
    this.loginForm = this.fb.group({
      username : ['',Validators.required],
      password : ['',Validators.required]
    })
  }

  type: string = "password"
  isText: boolean = false
  eyeIcon : string  = "fa-eye-slach"

  hideShowPassword(){
    this.isText = !this.isText;
    this.isText ? (this.eyeIcon = 'fa-eye') : (this.eyeIcon = 'fa-eye-slash');
    this.isText ? (this.type = 'text') : (this.type = 'password');
  }

  onLogin() {
    if (this.loginForm.valid) {
      console.log(this.loginForm.value);
       this.auth.login(this.loginForm.value)
       .subscribe({
         next: (res) => {
           console.log(res);
           this.loginForm.reset();

           this.auth.storeToken(res.token);

           this.toast.success({detail:"Success",summary:'Login Success',duration:5000});

           this.router.navigate(['dashboard']);
         },
         error:(err) => {
          console.log(err?.error.message);
          this.toast.error({detail:"ERROR",summary:err.error.message,duration:5000});
         }
       });
    } else {
       this.toast.error({
         detail: 'ERROR',
         summary: 'Please fill all details',
         duration: 5000,
       });
      ValidateForm.validateAllFormFields(this.loginForm);
    }
  }

  // public isValidEmail!: boolean;

  // checkValidEmail(event: string) {
  //   const value = event;
  //   const pattern = /^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$/;
  //   this.isValidEmail = pattern.test(value);
  //   return this.isValidEmail;
  // }

  // confirmToReset() {
  //   if (this.checkValidEmail(this.resetPasswordEmail)) {
  //     this.resetPasswordService.sendResetPasswordLink(this.resetPasswordEmail)
  //       .subscribe({
  //         next: (res) => {
  //           const buttonRef = document.getElementById("closeBtn");
  //           buttonRef?.click();
  //         },
  //         error: (err) => {
  //           this.toast.error({
  //             detail: 'ERROR',
  //             summary: 'Something went wrong!',
  //             duration: 5000,
  //           });
  //         }
  //       })
  //   }
  // }


}
