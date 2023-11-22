//import { AuthService } from './../../services/auth.service';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
//import { NgToastService } from 'ng-angular-popup';
import { Router, RouterModule } from '@angular/router';
import ValidateForm from '../../helpers/validateform';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-signup',
  standalone: true,
  imports: [CommonModule,RouterModule,ReactiveFormsModule],
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.scss']
})
export class SignupComponent implements OnInit {

  public signUpForm!: FormGroup;
  type: string = 'password';
  isText: boolean = false;
  eyeIcon:string = "fa-eye-slash"
  constructor(
    private fb : FormBuilder,
    //private toast: NgToastService,
    //private auth: AuthService,
    private router: Router
    ) { }

  ngOnInit() {
    this.signUpForm = this.fb.group({
      firstName:['', Validators.required],
      lastName:['', Validators.required],
      userName:['', Validators.required],
      email:['', Validators.required],
      password:['', Validators.required]
    })
  }

  hideShowPassword(){
    this.isText = !this.isText;
    this.isText ? this.eyeIcon = 'fa-eye' : this.eyeIcon = 'fa-eye-slash'
    this.isText ? this.type = 'text' : this.type = 'password'
  }

  onSubmit() {
    if (this.signUpForm.valid) {
      console.log(this.signUpForm.value);

      // let signUpObj = {
      //   ...this.signUpForm.value,
      //   role:'',
      //   token:''
      // }
      // this.auth.signUp(signUpObj)
      // .subscribe({
      //   next:(res=>{
      //     console.log(res);
      //     this.signUpForm.reset();
      //     this.router.navigate(['login'])
      //     this.toast.success({detail:"Success",summary:'Registration Success',duration:5000});
      //   }),
      //   error:(err=>{
      //     console.log(err)
      //     this.toast.error({detail:"ERROR",summary:err.error.message,duration:50000000});
      //   })
      // })
    } else {
      //this.toast.error({detail:"ERROR",summary:'Please fill all details',duration:5000});
      ValidateForm.validateAllFormFields(this.signUpForm); //{7}
    }
  }

}
